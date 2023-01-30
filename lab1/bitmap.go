package main

import (
	"errors"
	"fmt"
	"math"
)

type Bounds struct {
	begin, end int
}

var SignatureBounds = Bounds{0, 2}
var SizeHeaderBounds = Bounds{2, 6}
var Reserved1Bounds = Bounds{6, 8}
var Reserved2Bounds = Bounds{8, 10}
var OffsetBounds = Bounds{10, 14}

var SizeInfoBounds = Bounds{14, 18}
var WidthBounds = Bounds{18, 22}
var HeightBounds = Bounds{22, 26}
var PlanesBounds = Bounds{26, 28}
var BitCountBounds = Bounds{28, 30}
var CompressionBounds = Bounds{30, 34}
var SizeImageBounds = Bounds{34, 38}
var HorizontalResolutionBounds = Bounds{38, 42}
var VerticalResolutionBounds = Bounds{42, 46}
var ColorUsedBounds = Bounds{46, 50}
var ColorImportantBounds = Bounds{50, 54}

var BitmapFileHeaderBounds = Bounds{0, 14}
var BitmapFileInfoBounds = Bounds{14, 54}

const BMPSignature uint16 = 0x4D42
const RGBQuadElementsCount int = 4

type BitmapFileHeader struct {
	signature uint16
	size      uint32
	reserved1 uint16
	reserved2 uint16
	offset    uint32
}

type BitmapFileInfo struct {
	size                 uint32
	width                int32
	height               int32
	planes               uint16
	bitCount             uint16
	compression          uint32
	sizeImage            uint32
	horizontalResolution int32
	verticalResolution   int32
	colorUsed            uint32
	colorImportant       uint32
}

type RgbQuad struct {
	rgbBlue     byte
	rgbGreen    byte
	rgbRed      byte
	rgbReserved byte
}

type BMPImage struct {
	fileHeader      BitmapFileHeader
	fileInfo        BitmapFileInfo
	rgbQuad         []RgbQuad
	colorIndexArray []byte
}

func BytesToUInt32(b []byte) uint32 {
	return uint32(b[0]) | uint32(b[1])<<8 | uint32(b[2])<<16 | uint32(b[3])<<24
}

func BytesToUInt16(b []byte) uint16 {
	return uint16(b[0]) | uint16(b[1])<<8
}

func UInt32ToBytes(n uint32) []byte {
	bytes := make([]byte, 4)
	bytes[0] = byte(n)
	bytes[1] = byte(n >> 8)
	bytes[2] = byte(n >> 16)
	bytes[3] = byte(n >> 24)
	return bytes
}

func UInt16ToBytes(n uint16) []byte {
	bytes := make([]byte, 2)
	bytes[0] = byte(n)
	bytes[1] = byte(n >> 8)
	return bytes
}

func RGBQuadToBytes(rgbQuad RgbQuad) []byte {
	bytes := make([]byte, 4)
	bytes[0] = rgbQuad.rgbBlue
	bytes[1] = rgbQuad.rgbGreen
	bytes[2] = rgbQuad.rgbRed
	bytes[3] = rgbQuad.rgbReserved
	return bytes
}

func newBitmapFileHeader(data []byte) (BitmapFileHeader, error) {
	fileHeader := BitmapFileHeader{}
	fileHeader.signature = BytesToUInt16(data[SignatureBounds.begin:SignatureBounds.end])
	fileHeader.size = BytesToUInt32(data[SizeHeaderBounds.begin:SizeHeaderBounds.end])
	fileHeader.reserved1 = BytesToUInt16(data[Reserved1Bounds.begin:Reserved1Bounds.end])
	fileHeader.reserved2 = BytesToUInt16(data[Reserved2Bounds.begin:Reserved2Bounds.end])
	fileHeader.offset = BytesToUInt32(data[OffsetBounds.begin:OffsetBounds.end])

	var err error
	if fileHeader.signature != BMPSignature {
		err = errors.New("unsupported format. signature does not match BMP format")
	}

	return fileHeader, err
}

func newBitmapFileInfo(data []byte) BitmapFileInfo {
	fileInfo := BitmapFileInfo{}
	fileInfo.size = BytesToUInt32(data[SizeInfoBounds.begin:SizeInfoBounds.end])
	fileInfo.width = int32(BytesToUInt32(data[WidthBounds.begin:WidthBounds.end]))
	fileInfo.height = int32(BytesToUInt32(data[HeightBounds.begin:HeightBounds.end]))
	fileInfo.planes = BytesToUInt16(data[PlanesBounds.begin:PlanesBounds.end])
	fileInfo.bitCount = BytesToUInt16(data[BitCountBounds.begin:BitCountBounds.end])
	fileInfo.compression = BytesToUInt32(data[CompressionBounds.begin:CompressionBounds.end])
	fileInfo.sizeImage = BytesToUInt32(data[SizeImageBounds.begin:SizeImageBounds.end])
	fileInfo.horizontalResolution = int32(BytesToUInt32(data[HorizontalResolutionBounds.begin:HorizontalResolutionBounds.end]))
	fileInfo.verticalResolution = int32(BytesToUInt32(data[VerticalResolutionBounds.begin:VerticalResolutionBounds.end]))
	fileInfo.colorUsed = BytesToUInt32(data[ColorUsedBounds.begin:ColorUsedBounds.end])
	fileInfo.colorImportant = BytesToUInt32(data[ColorImportantBounds.begin:ColorImportantBounds.end])
	return fileInfo
}

func newRGBQuad(data []byte) RgbQuad {
	rgbQuad := RgbQuad{rgbBlue: data[0], rgbGreen: data[1], rgbRed: data[2], rgbReserved: data[3]}
	return rgbQuad
}

func newImageData(data []byte) BMPImage {
	fileHeader, fileHeaderErr := newBitmapFileHeader(data[:BitmapFileHeaderBounds.end])
	if fileHeaderErr != nil {
		panic(fileHeaderErr)
	}

	fileInfo := newBitmapFileInfo(data[:BitmapFileInfoBounds.end])

	fmt.Printf("%+v\n", fileHeader)
	fmt.Printf("%+v\n", fileInfo)

	rgbQuadElementsCount := int(math.Pow(2, float64(fileInfo.bitCount)))
	rgbQuad := make([]RgbQuad, rgbQuadElementsCount)
	for i, j := 0, BitmapFileInfoBounds.end; i < rgbQuadElementsCount; i, j = i+1, j+RGBQuadElementsCount {
		rgbQuad[i] = newRGBQuad(data[j : j+RGBQuadElementsCount])
	}

	colorIndexArray := data[fileHeader.offset:]

	imageData := BMPImage{fileHeader: fileHeader, fileInfo: fileInfo, rgbQuad: rgbQuad, colorIndexArray: colorIndexArray}
	return imageData
}

func bmpFromBytes(data []byte) BMPImage {
	return newImageData(data)
}

func bmpToBytes(image BMPImage) []byte {
	var data []byte

	data = append(data, UInt16ToBytes(image.fileHeader.signature)...)
	data = append(data, UInt32ToBytes(image.fileHeader.size)...)
	data = append(data, UInt16ToBytes(image.fileHeader.reserved1)...)
	data = append(data, UInt16ToBytes(image.fileHeader.reserved2)...)
	data = append(data, UInt32ToBytes(image.fileHeader.offset)...)

	data = append(data, UInt32ToBytes(image.fileInfo.size)...)
	data = append(data, UInt32ToBytes(uint32(image.fileInfo.width))...)
	data = append(data, UInt32ToBytes(uint32(image.fileInfo.height))...)
	data = append(data, UInt16ToBytes(image.fileInfo.planes)...)
	data = append(data, UInt16ToBytes(image.fileInfo.bitCount)...)
	data = append(data, UInt32ToBytes(image.fileInfo.compression)...)
	data = append(data, UInt32ToBytes(image.fileInfo.sizeImage)...)
	data = append(data, UInt32ToBytes(uint32(image.fileInfo.horizontalResolution))...)
	data = append(data, UInt32ToBytes(uint32(image.fileInfo.verticalResolution))...)
	data = append(data, UInt32ToBytes(image.fileInfo.colorUsed)...)
	data = append(data, UInt32ToBytes(image.fileInfo.colorImportant)...)

	for i := 0; i < len(image.rgbQuad); i++ {
		data = append(data, RGBQuadToBytes(image.rgbQuad[i])...)
	}

	data = append(data, image.colorIndexArray...)

	return data
}
