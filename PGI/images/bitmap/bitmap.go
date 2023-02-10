package bitmap

import (
	"errors"
	"math"

	"example.com/pgi_utils/binary"
	types "example.com/pgi_utils/types"
)

var SignatureBounds = types.Bounds{Begin: 0, End: 2}
var SizeHeaderBounds = types.Bounds{Begin: 2, End: 6}
var Reserved1Bounds = types.Bounds{Begin: 6, End: 8}
var Reserved2Bounds = types.Bounds{Begin: 8, End: 10}
var OffsetBounds = types.Bounds{Begin: 10, End: 14}

var SizeInfoBounds = types.Bounds{Begin: 14, End: 18}
var WidthBounds = types.Bounds{Begin: 18, End: 22}
var HeightBounds = types.Bounds{Begin: 22, End: 26}
var PlanesBounds = types.Bounds{Begin: 26, End: 28}
var BitCountBounds = types.Bounds{Begin: 28, End: 30}
var CompressionBounds = types.Bounds{Begin: 30, End: 34}
var SizeImageBounds = types.Bounds{Begin: 34, End: 38}
var HorizontalResolutionBounds = types.Bounds{Begin: 38, End: 42}
var VerticalResolutionBounds = types.Bounds{Begin: 42, End: 46}
var ColorUsedBounds = types.Bounds{Begin: 46, End: 50}
var ColorImportantBounds = types.Bounds{Begin: 50, End: 54}

var BitmapFileHeaderBounds = types.Bounds{Begin: 0, End: 14}
var BitmapFileInfoBounds = types.Bounds{Begin: 14, End: 54}

const BMPSignature uint16 = 0x4D42

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

type BMPMeta struct {
	BitsPerPixel      int
	BytesPerColor     int
	WidthAligned      int32
	WidthAlignedBytes int32
}

type BMPImage struct {
	FileHeader      BitmapFileHeader
	FileInfo        BitmapFileInfo
	RGBQuad         []types.RGBQuad
	ColorIndexArray []byte
	Meta            BMPMeta
}

func newBitmapFileHeader(data []byte) (BitmapFileHeader, error) {
	fileHeader := BitmapFileHeader{}
	fileHeader.Signature = binary.BytesToUInt16(data[SignatureBounds.Begin:SignatureBounds.End])
	fileHeader.Size = binary.BytesToUInt32(data[SizeHeaderBounds.Begin:SizeHeaderBounds.End])
	fileHeader.Reserved1 = binary.BytesToUInt16(data[Reserved1Bounds.Begin:Reserved1Bounds.End])
	fileHeader.Reserved2 = binary.BytesToUInt16(data[Reserved2Bounds.Begin:Reserved2Bounds.End])
	fileHeader.Offset = binary.BytesToUInt32(data[OffsetBounds.Begin:OffsetBounds.End])

	var err error
	if fileHeader.Signature != BMPSignature {
		err = errors.New("unsupported format. signature does not match BMP format")
	}

	return fileHeader, err
}

func bitmapFileHeaderToBytes(fileHeader BitmapFileHeader) []byte {
	var data []byte
	data = append(data, binary.UInt16ToBytes(fileHeader.Signature)...)
	data = append(data, binary.UInt32ToBytes(fileHeader.Size)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.Reserved1)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.Reserved2)...)
	data = append(data, binary.UInt32ToBytes(fileHeader.Offset)...)
	return data
}

func newBitmapFileInfo(data []byte) BitmapFileInfo {
	fileInfo := BitmapFileInfo{}
	fileInfo.Size = binary.BytesToUInt32(data[SizeInfoBounds.Begin:SizeInfoBounds.End])
	fileInfo.Width = int32(binary.BytesToUInt32(data[WidthBounds.Begin:WidthBounds.End]))
	fileInfo.Height = int32(binary.BytesToUInt32(data[HeightBounds.Begin:HeightBounds.End]))
	fileInfo.Planes = binary.BytesToUInt16(data[PlanesBounds.Begin:PlanesBounds.End])
	fileInfo.BitCount = binary.BytesToUInt16(data[BitCountBounds.Begin:BitCountBounds.End])
	fileInfo.Compression = binary.BytesToUInt32(data[CompressionBounds.Begin:CompressionBounds.End])
	fileInfo.SizeImage = binary.BytesToUInt32(data[SizeImageBounds.Begin:SizeImageBounds.End])
	fileInfo.HorizontalResolution = int32(binary.BytesToUInt32(data[HorizontalResolutionBounds.Begin:HorizontalResolutionBounds.End]))
	fileInfo.VerticalResolution = int32(binary.BytesToUInt32(data[VerticalResolutionBounds.Begin:VerticalResolutionBounds.End]))
	fileInfo.ColorUsed = binary.BytesToUInt32(data[ColorUsedBounds.Begin:ColorUsedBounds.End])
	fileInfo.ColorImportant = binary.BytesToUInt32(data[ColorImportantBounds.Begin:ColorImportantBounds.End])
	return fileInfo
}

func bitmapFileInfoToBytes(fileInfo BitmapFileInfo) []byte {
	var data []byte
	data = append(data, binary.UInt32ToBytes(fileInfo.Size)...)
	data = append(data, binary.UInt32ToBytes(uint32(fileInfo.Width))...)
	data = append(data, binary.UInt32ToBytes(uint32(fileInfo.Height))...)
	data = append(data, binary.UInt16ToBytes(fileInfo.Planes)...)
	data = append(data, binary.UInt16ToBytes(fileInfo.BitCount)...)
	data = append(data, binary.UInt32ToBytes(fileInfo.Compression)...)
	data = append(data, binary.UInt32ToBytes(fileInfo.SizeImage)...)
	data = append(data, binary.UInt32ToBytes(uint32(fileInfo.HorizontalResolution))...)
	data = append(data, binary.UInt32ToBytes(uint32(fileInfo.VerticalResolution))...)
	data = append(data, binary.UInt32ToBytes(fileInfo.ColorUsed)...)
	data = append(data, binary.UInt32ToBytes(fileInfo.ColorImportant)...)
	return data
}

func NewMeta(bitCount uint16, width int32) BMPMeta {
	meta := BMPMeta{}

	meta.BitsPerPixel = getBitsPerPixel(int(bitCount))
	meta.BytesPerColor = int(bitCount / binary.BitsPerByte)

	bitsInRow := int(width) * int(bitCount)
	rowMultiplicityBytes := 4
	rowMultiplicityBits := rowMultiplicityBytes * binary.BitsPerByte

	meta.WidthAlignedBytes = int32(rowMultiplicityBytes) * int32(math.Ceil(float64(bitsInRow)/float64(rowMultiplicityBits)))

	if bitCount < binary.BitsPerByte {
		meta.WidthAligned = meta.WidthAlignedBytes * int32(meta.BitsPerPixel)
	} else {
		meta.WidthAligned = meta.WidthAlignedBytes / int32(meta.BytesPerColor)
	}

	return meta
}

func newRGBQuadPalette(data []byte) []types.RGBQuad {
	rgbQuad := make([]types.RGBQuad, len(data)/types.RGBQuadElementsCount)
	for i, j := 0, 0; i < len(rgbQuad); i, j = i+1, j+types.RGBQuadElementsCount {
		rgbQuad[i] = types.NewRGBQuad(data[j : j+types.RGBQuadElementsCount])
	}
	return rgbQuad
}

func rgbQuadPaletteToBytes(palette []types.RGBQuad) []byte {
	var data []byte
	for _, color := range palette {
		data = append(data, types.RGBQuadToBytes(color)...)
	}
	return data
}

func newImageData(data []byte) BMPImage {
	fileHeader, _ := newBitmapFileHeader(data[:BitmapFileHeaderBounds.End])
	fileInfo := newBitmapFileInfo(data[:BitmapFileInfoBounds.End])
	rgbQuad := newRGBQuadPalette(data[BitmapFileInfoBounds.End:fileHeader.Offset])
	colorIndexArray := data[fileHeader.Offset:]
	meta := NewMeta(fileInfo.BitCount, fileInfo.Width)

	imageData := BMPImage{FileHeader: fileHeader, FileInfo: fileInfo, RGBQuad: rgbQuad, ColorIndexArray: colorIndexArray, Meta: meta}
	return imageData
}

func FromBytes(data []byte) BMPImage {
	return newImageData(data)
}

func ToBytes(image BMPImage) []byte {
	var data []byte
	data = append(data, bitmapFileHeaderToBytes(image.FileHeader)...)
	data = append(data, bitmapFileInfoToBytes(image.FileInfo)...)
	data = append(data, rgbQuadPaletteToBytes(image.RGBQuad)...)
	data = append(data, image.ColorIndexArray...)
	return data
}

func GetCopy(image BMPImage) BMPImage {
	imageCopy := image
	imageCopy.RGBQuad = append([]types.RGBQuad(nil), image.RGBQuad...)
	imageCopy.ColorIndexArray = append([]byte(nil), image.ColorIndexArray...)
	return imageCopy
}

func getBitsPerPixel(bitCount int) int {
	if bitCount < binary.BitsPerByte {
		return binary.BitsPerByte / bitCount
	}

	return 1
}

func getPixelIndex(i int, j int, image BMPImage) int {
	invertedI := (int(image.FileInfo.Height) - i - 1)

	var row int
	var column int

	if image.FileInfo.BitCount >= binary.BitsPerByte {
		row = invertedI * int(image.Meta.WidthAlignedBytes)
		column = j * image.Meta.BytesPerColor
	} else {
		row = invertedI * int(image.Meta.WidthAligned) / image.Meta.BitsPerPixel
		column = j / image.Meta.BitsPerPixel
	}

	index := row + column
	return index
}

func getPixelBounds(j int, image BMPImage) (int, int) {
	if image.FileInfo.BitCount == binary.BitsPerByte {
		return 0, binary.BitsPerByte
	}

	startBit := int(image.FileInfo.BitCount) - (j % image.Meta.BitsPerPixel * int(image.FileInfo.BitCount))
	endBit := startBit + int(image.FileInfo.BitCount)
	return startBit, endBit
}

func SetPixelColorIndex(i int, j int, colorIndex uint8, image BMPImage) {
	index := getPixelIndex(i, j, image)
	startBit, endBit := getPixelBounds(j, image)

	for i, j := startBit, 0; i < endBit; i, j = i+1, j+1 {
		image.ColorIndexArray[index] = byte(binary.ClearBit(int(image.ColorIndexArray[index]), uint(i)))
		if binary.HasBit(int(colorIndex), uint(j)) {
			image.ColorIndexArray[index] = byte(binary.SetBit(int(image.ColorIndexArray[index]), uint(i)))
		}
	}
}

func GetPixelColorIndex(i int, j int, image BMPImage) uint8 {
	index := getPixelIndex(i, j, image)
	startBit, endBit := getPixelBounds(j, image)

	if image.FileInfo.BitCount == binary.BitsPerByte {
		startBit = 0
		endBit = binary.BitsPerByte
	}

	var colorIndex uint8
	for i, j := startBit, 0; i < endBit; i, j = i+1, j+1 {
		if binary.HasBit(int(image.ColorIndexArray[index]), uint(i)) {
			colorIndex = byte(binary.SetBit(int(colorIndex), uint(j)))
		}
	}

	return colorIndex
}

func getPixel(i int, j int, image BMPImage) types.RGBQuad {
	index := getPixelIndex(i, j, image)

	var color types.RGBQuad
	color.RGBBlue = image.ColorIndexArray[index]
	color.RGBGreen = image.ColorIndexArray[index+1]
	color.RGBRed = image.ColorIndexArray[index+2]

	if image.FileInfo.BitCount == 32 {
		color.RGBReserved = image.ColorIndexArray[index+3]
	}

	return color
}

func setPixel(i int, j int, color types.RGBQuad, image BMPImage) {
	index := getPixelIndex(i, j, image)

	image.ColorIndexArray[index] = color.RGBBlue
	image.ColorIndexArray[index+1] = color.RGBGreen
	image.ColorIndexArray[index+2] = color.RGBRed

	if image.FileInfo.BitCount == 32 {
		image.ColorIndexArray[index+3] = color.RGBReserved
	}
}

func GetPixelColor(i int, j int, image BMPImage) types.RGBQuad {
	if image.FileInfo.BitCount <= binary.BitsPerByte {
		colorIndex := GetPixelColorIndex(i, j, image)
		color := image.RGBQuad[colorIndex]
		return color
	}

	color := getPixel(i, j, image)
	return color
}

func SetPixelColor(i int, j int, color types.RGBQuad, image BMPImage) {
	setPixel(i, j, color, image)
}
