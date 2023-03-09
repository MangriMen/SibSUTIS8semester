package pcx

import (
	"example.com/pgi_utils/binary"
	"example.com/pgi_utils/types"
)

const OptionalPaletteSizeBytes = 768
const OptionalPaletteSize = OptionalPaletteSizeBytes / types.RGBTripleELementsCount

const rleMask byte = 0b11000000
const pixelRunLengthMask byte = 0b00111111

const minPixelRunLength = 2
const maxPixelRunLength = 63

type PCXHeader struct {
	ID          uint8
	Version     uint8
	Coding      uint8
	BitPerPixel uint8
	XMin        uint16
	YMin        uint16
	XMax        uint16
	YMax        uint16
	HRes        uint16
	VRes        uint16
	Palette     [48]byte
	Reserved    uint8
	Planes      uint8
	BytePerLine uint16
	PaletteInfo uint16
	HScreenSize uint16
	VScreenSize uint16
	Filler      [54]byte
}

type PCXImage struct {
	FileHeader      PCXHeader
	Data            []byte
	OptionalPalette []types.RGBTriple
	DecodedData     []byte
}

func newPCXHeader(data []byte) PCXHeader {
	fileHeader := PCXHeader{}
	fileHeader.ID = uint8(data[0])
	fileHeader.Version = uint8(data[1])
	fileHeader.Coding = uint8(data[2])
	fileHeader.BitPerPixel = uint8(data[3])
	fileHeader.XMin = binary.BytesToUInt16(data[4:6])
	fileHeader.YMin = binary.BytesToUInt16(data[6:8])
	fileHeader.XMax = binary.BytesToUInt16(data[8:10])
	fileHeader.YMax = binary.BytesToUInt16(data[10:12])
	fileHeader.HRes = binary.BytesToUInt16(data[12:14])
	fileHeader.VRes = binary.BytesToUInt16(data[14:16])
	fileHeader.Palette = [48]byte(data[16:64])
	fileHeader.Reserved = uint8(data[64])
	fileHeader.Planes = uint8(data[65])
	fileHeader.BytePerLine = binary.BytesToUInt16(data[66:68])
	fileHeader.PaletteInfo = binary.BytesToUInt16(data[68:70])
	fileHeader.HScreenSize = binary.BytesToUInt16(data[70:72])
	fileHeader.VScreenSize = binary.BytesToUInt16(data[72:74])
	fileHeader.Filler = [54]byte(data[74:128])
	return fileHeader
}

func pcxHeaderToBytes(fileHeader PCXHeader) []byte {
	var data []byte
	data = append(data, fileHeader.ID)
	data = append(data, fileHeader.Version)
	data = append(data, fileHeader.Coding)
	data = append(data, fileHeader.BitPerPixel)
	data = append(data, binary.UInt16ToBytes(fileHeader.XMin)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.YMin)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.XMax)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.YMax)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.HRes)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.VRes)...)
	data = append(data, fileHeader.Palette[:]...)
	data = append(data, fileHeader.Reserved)
	data = append(data, fileHeader.Planes)
	data = append(data, binary.UInt16ToBytes(fileHeader.BytePerLine)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.PaletteInfo)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.HScreenSize)...)
	data = append(data, binary.UInt16ToBytes(fileHeader.VScreenSize)...)
	data = append(data, fileHeader.Filler[:]...)
	return data
}

func newOptionalPalette(data []byte, imageDataRightBound int) []types.RGBTriple {
	if data[imageDataRightBound-1] == 0x0C {
		optionalPalette := make([]types.RGBTriple, OptionalPaletteSize)
		for i, j := 0, imageDataRightBound; i < len(optionalPalette); i, j = i+1, j+types.RGBTripleELementsCount {
			optionalPalette[i] = types.NewRGBTriple(data[j : j+types.RGBTripleELementsCount])
		}
		return optionalPalette
	}

	return []types.RGBTriple{}
}

func optionalPaletteToBytes(optionalPalette []types.RGBTriple) []byte {
	var data []byte
	for _, color := range optionalPalette {
		data = append(data, types.RGBTripleToBytes(color)...)
	}
	return data
}

func newPCXImage(data []byte) PCXImage {
	imageDataRightBound := len(data) - OptionalPaletteSizeBytes

	fileHeader := newPCXHeader(data[:128])
	imageData := data[128:imageDataRightBound]
	optionalPalette := newOptionalPalette(data, imageDataRightBound)
	decodedData := decodeRLE(imageData)

	return PCXImage{FileHeader: fileHeader, Data: imageData, OptionalPalette: optionalPalette, DecodedData: decodedData}
}

func FromBytes(data []byte) PCXImage {
	return newPCXImage(data)
}

func ToBytes(image PCXImage) []byte {
	image.Data = encodeRLE(image.DecodedData)

	var data []byte
	data = append(data, pcxHeaderToBytes(image.FileHeader)...)
	data = append(data, image.Data...)
	if len(image.OptionalPalette) > 0 {
		data = append(data, 0x0C)
	}
	data = append(data, optionalPaletteToBytes(image.OptionalPalette)...)
	return data
}

func decodeRLE(data []byte) []byte {
	decodedData := []byte{}
	for i := 0; i < len(data); i++ {
		if (data[i] & rleMask) == rleMask {
			count := data[i] & pixelRunLengthMask
			for j := 0; j < int(count); j++ {
				decodedData = append(decodedData, data[i+1])
			}
			i++
		} else {
			decodedData = append(decodedData, data[i])
		}
	}
	return decodedData
}

func encodeRLE(decodedData []byte) []byte {
	var data []byte
	dataIndex := 0

	decodedDataLength := len(decodedData) - 1
	i := 0
	for i < decodedDataLength {
		data = append(data, rleMask)

		startColorIndex := i
		for i < decodedDataLength && i-startColorIndex <= (maxPixelRunLength-2) && decodedData[i] == decodedData[i+1] {
			i++
		}

		sameColorCount := 0
		if i > startColorIndex {
			sameColorCount = i - startColorIndex + 1
		}

		if decodedData[startColorIndex] >= rleMask && sameColorCount < minPixelRunLength {
			data[dataIndex] += 1
			data = append(data, decodedData[startColorIndex])
			dataIndex++
		} else if sameColorCount >= minPixelRunLength {
			data[dataIndex] += byte(sameColorCount)
			data = append(data, decodedData[startColorIndex+1])
			dataIndex++
		} else {
			data[dataIndex] = decodedData[startColorIndex]
		}

		i++

		dataIndex++
	}

	return data
}

func GetCopy(image PCXImage) PCXImage {
	imageCopy := image
	imageCopy.Data = append([]byte(nil), image.Data...)
	imageCopy.OptionalPalette = append([]types.RGBTriple(nil), image.OptionalPalette...)
	imageCopy.DecodedData = append([]byte(nil), image.DecodedData...)
	return imageCopy
}

func EGAToRGB(egaColor byte) types.RGBTriple {
	rgbColor := types.RGBTriple{}
	rgbColor.RGBTRed = 85 * (((egaColor >> 1) & 2) | (egaColor>>5)&1)
	rgbColor.RGBTGreen = 85 * ((egaColor & 2) | (egaColor>>4)&1)
	rgbColor.RGBTBlue = 85 * (((egaColor << 1) & 2) | (egaColor>>3)&1)
	return rgbColor
}

func RGBtoEGA(color types.RGBTriple) byte {
	colorRed := (((color.RGBTRed / 85) & 2) << 1) | (((color.RGBTRed / 85) & 1) << 5)
	colorGreen := ((color.RGBTGreen / 85) & 2) | (((color.RGBTGreen / 85) & 1) << 4)
	colorBlue := (((color.RGBTBlue / 85) & 2) >> 1) | (((color.RGBTBlue / 85) & 1) << 3)
	egaPalette := colorRed | colorGreen | colorBlue

	return egaPalette
}

func getBitsPerPixel(bitCount int) int {
	if bitCount < binary.BitsPerByte {
		return binary.BitsPerByte / bitCount
	}

	return 1
}

func getPixelIndex(i, j int, image PCXImage) int {
	var row int
	var column int

	if image.FileHeader.Planes == 1 {
		row = i * int(image.FileHeader.BytePerLine)
		column = j
	} else {
		row = i * int(image.FileHeader.Planes) * int(image.FileHeader.BytePerLine)
		column = j
	}

	index := row + column
	return index
}

func getPixelBounds(j int, image PCXImage) (int, int) {
	if image.FileHeader.BitPerPixel == binary.BitsPerByte {
		return 0, binary.BitsPerByte
	}

	bitsPerPixel := getBitsPerPixel(int(image.FileHeader.BitPerPixel))

	startBit := int(image.FileHeader.BitPerPixel) - (j % bitsPerPixel * int(image.FileHeader.BitPerPixel))
	endBit := startBit + int(image.FileHeader.BitPerPixel)
	return startBit, endBit
}

func GetPixelColorIndex(i, j int, image PCXImage) uint8 {
	index := getPixelIndex(i, j, image)
	startBit, endBit := getPixelBounds(j, image)

	var colorIndex uint8
	for i, j := startBit, 0; i < endBit; i, j = i+1, j+1 {
		if binary.HasBit(int(image.DecodedData[index]), uint(i)) {
			colorIndex = byte(binary.SetBit(int(colorIndex), uint(j)))
		}
	}

	return colorIndex
}

func SetPixelColorIndex(i, j int, colorIndex uint8, image PCXImage) {
	index := getPixelIndex(i, j, image)
	startBit, endBit := getPixelBounds(j, image)

	for i, j := startBit, 0; i < endBit; i, j = i+1, j+1 {
		image.DecodedData[index] = byte(binary.ClearBit(int(image.DecodedData[index]), uint(i)))
		if binary.HasBit(int(colorIndex), uint(j)) {
			image.DecodedData[index] = byte(binary.SetBit(int(image.DecodedData[index]), uint(i)))
		}
	}
}

func getPixel(i, j int, image PCXImage) types.RGBTriple {
	index := getPixelIndex(i, j, image)

	var color types.RGBTriple
	color.RGBTRed = image.DecodedData[index]
	color.RGBTGreen = image.DecodedData[index+int(image.FileHeader.BytePerLine)]
	color.RGBTBlue = image.DecodedData[index+(int(image.FileHeader.BytePerLine)*2)]

	return color
}

func setPixel(i, j int, color types.RGBTriple, image PCXImage) {
	index := getPixelIndex(i, j, image)

	image.DecodedData[index] = color.RGBTRed
	image.DecodedData[index+int(image.FileHeader.BytePerLine)] = color.RGBTGreen
	image.DecodedData[index+(int(image.FileHeader.BytePerLine)*2)] = color.RGBTBlue
}

func GetPixelColor(i, j int, image PCXImage) types.RGBTriple {
	if image.FileHeader.Planes == 1 {
		colorIndex := GetPixelColorIndex(i, j, image)

		var color types.RGBTriple
		if len(image.OptionalPalette) > 0 {
			color = image.OptionalPalette[colorIndex]
		} else {
			color = EGAToRGB(image.FileHeader.Palette[colorIndex])
		}

		return color
	}

	color := getPixel(i, j, image)
	return color
}

func SetPixelColor(i int, j int, color types.RGBTriple, image PCXImage) {
	setPixel(i, j, color, image)
}
