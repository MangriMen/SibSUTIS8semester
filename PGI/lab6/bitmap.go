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

const BitsPerByte = 8
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

type BMPMeta struct {
	bitsPerPixel      int
	bytesPerColor     int
	widthAligned      uint32
	widthAlignedBytes uint32
}

type BMPImage struct {
	FileHeader      BitmapFileHeader
	FileInfo        BitmapFileInfo
	RgbQuad         []RgbQuad
	ColorIndexArray []byte
	Meta            BMPMeta
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

func newMeta(bitCount uint16, width int32) BMPMeta {
	meta := BMPMeta{}

	meta.bitsPerPixel = getBitsPerPixel(int(bitCount))
	meta.bytesPerColor = int(bitCount / BitsPerByte)

	meta.widthAlignedBytes = 4 * uint32(math.Ceil(float64(int(width)*int(bitCount))/32))
	if bitCount < BitsPerByte {
		meta.widthAligned = meta.widthAlignedBytes * uint32(meta.bitsPerPixel)
	} else {
		meta.widthAligned = meta.widthAlignedBytes / uint32(meta.bytesPerColor)
	}

	return meta
}

func newRgbQuadPalette(data []byte) []RgbQuad {
	rgbQuad := make([]RgbQuad, len(data)/RGBQuadElementsCount)
	for i, j := 0, 0; i < len(rgbQuad); i, j = i+1, j+RGBQuadElementsCount {
		rgbQuad[i] = newRGBQuad(data[j : j+RGBQuadElementsCount])
	}
	return rgbQuad
}

func newImageData(data []byte) BMPImage {
	fileHeader, _ := newBitmapFileHeader(data[:BitmapFileHeaderBounds.end])
	fileInfo := newBitmapFileInfo(data[:BitmapFileInfoBounds.end])
	rgbQuad := newRgbQuadPalette(data[BitmapFileInfoBounds.end:fileHeader.Offset])
	colorIndexArray := data[fileHeader.Offset:]
	meta := newMeta(fileInfo.BitCount, fileInfo.Width)

	imageData := BMPImage{FileHeader: fileHeader, FileInfo: fileInfo, RgbQuad: rgbQuad, ColorIndexArray: colorIndexArray, Meta: meta}
	return imageData
}

func bmpFromBytes(data []byte) BMPImage {
	return newImageData(data)
}

func RGBQuadToBytes(rgbQuad RgbQuad) []byte {
	bytes := make([]byte, 4)
	bytes[0] = rgbQuad.RgbBlue
	bytes[1] = rgbQuad.RgbGreen
	bytes[2] = rgbQuad.RgbRed
	bytes[3] = rgbQuad.RgbReserved
	return bytes
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

func getBitsPerPixel(bitCount int) int {
	if bitCount < BitsPerByte {
		return BitsPerByte / bitCount
	}

	return 1
}

func getPixelIndex(i int, j int, image BMPImage) int {
	height := int(image.FileInfo.Height)

	var row int = (height - i - 1) * int(image.Meta.widthAligned) / image.Meta.bitsPerPixel
	var column int = j / image.Meta.bitsPerPixel

	if image.FileInfo.BitCount >= BitsPerByte {
		row = (height - i - 1) * int(image.Meta.widthAlignedBytes)
		column = j * image.Meta.bytesPerColor
	}

	index := row + column

	return index
}

func getPixelBounds(j int, image BMPImage) (int, int) {
	if image.FileInfo.BitCount == BitsPerByte {
		return 0, BitsPerByte
	}
	startBit := int(image.FileInfo.BitCount) - (j % image.Meta.bitsPerPixel * int(image.FileInfo.BitCount))
	endBit := startBit + int(image.FileInfo.BitCount)

	return startBit, endBit
}

func setColorIndexToPixel(i int, j int, data int, image BMPImage) {
	index := getPixelIndex(i, j, image)
	startBit, endBit := getPixelBounds(j, image)

	for i, j := startBit, 0; i < endBit; i, j = i+1, j+1 {
		image.ColorIndexArray[index] = byte(clearBit(int(image.ColorIndexArray[index]), uint(i)))
		if hasBit(data, uint(j)) {
			image.ColorIndexArray[index] = byte(setBit(int(image.ColorIndexArray[index]), uint(i)))
		}
	}
}

func getColorIndexFromPixel(i int, j int, image BMPImage) byte {
	index := getPixelIndex(i, j, image)
	startBit, endBit := getPixelBounds(j, image)

	if image.Meta.bitsPerPixel == 1 {
		startBit = 0
		endBit = 8
	}

	var pixel byte = 0
	for i, j := startBit, 0; i < endBit; i, j = i+1, j+1 {
		if hasBit(int(image.ColorIndexArray[index]), uint(i)) {
			pixel = byte(setBit(int(pixel), uint(j)))
		}
	}

	return pixel
}

func getPixel(i int, j int, image BMPImage) RgbQuad {
	index := getPixelIndex(i, j, image)

	var pixel RgbQuad = RgbQuad{}
	pixel.RgbBlue = image.ColorIndexArray[index]
	pixel.RgbGreen = image.ColorIndexArray[index+1]
	pixel.RgbRed = image.ColorIndexArray[index+2]

	if image.FileInfo.BitCount == 32 {
		pixel.RgbReserved = image.ColorIndexArray[index+3]
	}

	return pixel
}

func setPixel(i int, j int, color RgbQuad, image BMPImage) {
	index := getPixelIndex(i, j, image)

	image.ColorIndexArray[index] = color.RgbBlue
	image.ColorIndexArray[index+1] = color.RgbGreen
	image.ColorIndexArray[index+2] = color.RgbRed

	if image.FileInfo.BitCount == 32 {
		image.ColorIndexArray[index+3] = color.RgbReserved
	}
}

func GetPixelColor(i int, j int, image BMPImage) RgbQuad {
	if image.FileInfo.BitCount <= BitsPerByte {
		colorIndex := getColorIndexFromPixel(i, j, image)
		pixelColor := image.RgbQuad[colorIndex]
		return pixelColor
	}

	color := getPixel(i, j, image)
	return color
}

func SetPixelColor(i int, j int, color RgbQuad, image BMPImage) {
	setPixel(i, j, color, image)
}
