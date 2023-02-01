package main

import (
	"errors"
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
	Signature uint16
	Size      uint32
	Reserved1 uint16
	Reserved2 uint16
	Offset    uint32
}

type BitmapFileInfo struct {
	Size                 uint32
	Width                int32
	Height               int32
	Planes               uint16
	BitCount             uint16
	Compression          uint32
	SizeImage            uint32
	HorizontalResolution int32
	VerticalResolution   int32
	ColorUsed            uint32
	ColorImportant       uint32
}

type RgbQuad struct {
	RgbBlue     byte
	RgbGreen    byte
	RgbRed      byte
	RgbReserved byte
}

type BMPImage struct {
	FileHeader      BitmapFileHeader
	FileInfo        BitmapFileInfo
	RgbQuad         []RgbQuad
	ColorIndexArray []byte
}

const BitsPerByte = 8

func setBit(n int, pos uint) int {
	n |= (1 << pos)
	return n
}

func clearBit(n int, pos uint) int {
	mask := ^(1 << pos)
	n &= mask
	return n
}

func hasBit(n int, pos uint) bool {
	val := n & (1 << pos)
	return (val > 0)
}

func SetPixel(i int, j int, data int, colorIndexes []byte, fileInfo BitmapFileInfo) {
	var bitsPerPixel int
	if fileInfo.BitCount <= BitsPerByte {
		bitsPerPixel = BitsPerByte / int(fileInfo.BitCount)
	} else {
		panic("unsupported bits per pixel")
	}

	row := i * int(fileInfo.Width) / bitsPerPixel
	column := j / bitsPerPixel
	index := row + column

	startBit := int(fileInfo.BitCount) - (j % bitsPerPixel * int(fileInfo.BitCount))
	endBit := startBit + int(fileInfo.BitCount)
	for i, j := startBit, 0; i < endBit; i, j = i+1, j+1 {
		colorIndexes[index] = byte(clearBit(int(colorIndexes[index]), uint(i)))
		if hasBit(data, uint(j)) {
			colorIndexes[index] = byte(setBit(int(colorIndexes[index]), uint(i)))
		}
	}
}

func GetPixel(i int, j int, colorIndexes []byte, fileInfo BitmapFileInfo) byte {
	var bitsPerPixel int
	if fileInfo.BitCount <= BitsPerByte {
		bitsPerPixel = BitsPerByte / int(fileInfo.BitCount)
	} else {
		panic("unsupported bits per pixel")
	}

	row := (int(fileInfo.Height) - i - 1) * int(fileInfo.Width) / bitsPerPixel
	column := j / bitsPerPixel
	index := row + column

	var pixel byte = 0

	startBit := int(fileInfo.BitCount) - (j % bitsPerPixel * int(fileInfo.BitCount))
	endBit := startBit + int(fileInfo.BitCount)
	for i, j := startBit, 0; i < endBit; i, j = i+1, j+1 {
		if hasBit(int(colorIndexes[index]), uint(i)) {
			pixel = byte(setBit(int(pixel), uint(j)))
		}
	}

	return pixel
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
	bytes[0] = rgbQuad.RgbBlue
	bytes[1] = rgbQuad.RgbGreen
	bytes[2] = rgbQuad.RgbRed
	bytes[3] = rgbQuad.RgbReserved
	return bytes
}

func newBitmapFileHeader(data []byte) (BitmapFileHeader, error) {
	fileHeader := BitmapFileHeader{}
	fileHeader.Signature = BytesToUInt16(data[SignatureBounds.begin:SignatureBounds.end])
	fileHeader.Size = BytesToUInt32(data[SizeHeaderBounds.begin:SizeHeaderBounds.end])
	fileHeader.Reserved1 = BytesToUInt16(data[Reserved1Bounds.begin:Reserved1Bounds.end])
	fileHeader.Reserved2 = BytesToUInt16(data[Reserved2Bounds.begin:Reserved2Bounds.end])
	fileHeader.Offset = BytesToUInt32(data[OffsetBounds.begin:OffsetBounds.end])

	var err error
	if fileHeader.Signature != BMPSignature {
		err = errors.New("unsupported format. signature does not match BMP format")
	}

	return fileHeader, err
}

func newBitmapFileInfo(data []byte) BitmapFileInfo {
	fileInfo := BitmapFileInfo{}
	fileInfo.Size = BytesToUInt32(data[SizeInfoBounds.begin:SizeInfoBounds.end])
	fileInfo.Width = int32(BytesToUInt32(data[WidthBounds.begin:WidthBounds.end]))
	fileInfo.Height = int32(BytesToUInt32(data[HeightBounds.begin:HeightBounds.end]))
	fileInfo.Planes = BytesToUInt16(data[PlanesBounds.begin:PlanesBounds.end])
	fileInfo.BitCount = BytesToUInt16(data[BitCountBounds.begin:BitCountBounds.end])
	fileInfo.Compression = BytesToUInt32(data[CompressionBounds.begin:CompressionBounds.end])
	fileInfo.SizeImage = BytesToUInt32(data[SizeImageBounds.begin:SizeImageBounds.end])
	fileInfo.HorizontalResolution = int32(BytesToUInt32(data[HorizontalResolutionBounds.begin:HorizontalResolutionBounds.end]))
	fileInfo.VerticalResolution = int32(BytesToUInt32(data[VerticalResolutionBounds.begin:VerticalResolutionBounds.end]))
	fileInfo.ColorUsed = BytesToUInt32(data[ColorUsedBounds.begin:ColorUsedBounds.end])
	fileInfo.ColorImportant = BytesToUInt32(data[ColorImportantBounds.begin:ColorImportantBounds.end])
	return fileInfo
}

func newRGBQuad(data []byte) RgbQuad {
	rgbQuad := RgbQuad{RgbBlue: data[0], RgbGreen: data[1], RgbRed: data[2], RgbReserved: data[3]}
	return rgbQuad
}

func newImageData(data []byte) BMPImage {
	fileHeader, fileHeaderErr := newBitmapFileHeader(data[:BitmapFileHeaderBounds.end])
	if fileHeaderErr != nil {
		panic(fileHeaderErr)
	}

	fileInfo := newBitmapFileInfo(data[:BitmapFileInfoBounds.end])

	rgbQuadElementsCount := int(math.Pow(2, float64(fileInfo.BitCount)))
	rgbQuad := make([]RgbQuad, rgbQuadElementsCount)
	for i, j := 0, BitmapFileInfoBounds.end; i < rgbQuadElementsCount; i, j = i+1, j+RGBQuadElementsCount {
		rgbQuad[i] = newRGBQuad(data[j : j+RGBQuadElementsCount])
	}

	colorIndexArray := data[fileHeader.Offset:]

	imageData := BMPImage{FileHeader: fileHeader, FileInfo: fileInfo, RgbQuad: rgbQuad, ColorIndexArray: colorIndexArray}
	return imageData
}

func bmpFromBytes(data []byte) BMPImage {
	return newImageData(data)
}

func bmpToBytes(image BMPImage) []byte {
	var data []byte

	data = append(data, UInt16ToBytes(image.FileHeader.Signature)...)
	data = append(data, UInt32ToBytes(image.FileHeader.Size)...)
	data = append(data, UInt16ToBytes(image.FileHeader.Reserved1)...)
	data = append(data, UInt16ToBytes(image.FileHeader.Reserved2)...)
	data = append(data, UInt32ToBytes(image.FileHeader.Offset)...)

	data = append(data, UInt32ToBytes(image.FileInfo.Size)...)
	data = append(data, UInt32ToBytes(uint32(image.FileInfo.Width))...)
	data = append(data, UInt32ToBytes(uint32(image.FileInfo.Height))...)
	data = append(data, UInt16ToBytes(image.FileInfo.Planes)...)
	data = append(data, UInt16ToBytes(image.FileInfo.BitCount)...)
	data = append(data, UInt32ToBytes(image.FileInfo.Compression)...)
	data = append(data, UInt32ToBytes(image.FileInfo.SizeImage)...)
	data = append(data, UInt32ToBytes(uint32(image.FileInfo.HorizontalResolution))...)
	data = append(data, UInt32ToBytes(uint32(image.FileInfo.VerticalResolution))...)
	data = append(data, UInt32ToBytes(image.FileInfo.ColorUsed)...)
	data = append(data, UInt32ToBytes(image.FileInfo.ColorImportant)...)

	for i := 0; i < len(image.RgbQuad); i++ {
		data = append(data, RGBQuadToBytes(image.RgbQuad[i])...)
	}

	data = append(data, image.ColorIndexArray...)

	return data
}
