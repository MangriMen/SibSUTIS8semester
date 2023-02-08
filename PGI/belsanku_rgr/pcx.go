package main

import "sort"

const RGBTripleELementsCount = 3
const OptionalPaletteSizeBytes = 768
const OptionalPaletteSize = OptionalPaletteSizeBytes / RGBTripleELementsCount

type RgbTriple struct {
	RgbRed   uint8
	RgbGreen uint8
	RgbBlue  uint8
}

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
	OptionalPalette []RgbTriple
	DecodedData     []byte
}

func newRgbTriple(data []byte) RgbTriple {
	rgbTriple := RgbTriple{RgbRed: data[0], RgbGreen: data[1], RgbBlue: data[2]}
	return rgbTriple
}

func newPCXHeader(data []byte) PCXHeader {
	fileHeader := PCXHeader{}
	fileHeader.ID = uint8(data[0])
	fileHeader.Version = uint8(data[1])
	fileHeader.Coding = uint8(data[2])
	fileHeader.BitPerPixel = uint8(data[3])
	fileHeader.XMin = BytesToUInt16(data[4:6])
	fileHeader.YMin = BytesToUInt16(data[6:8])
	fileHeader.XMax = BytesToUInt16(data[8:10])
	fileHeader.YMax = BytesToUInt16(data[10:12])
	fileHeader.HRes = BytesToUInt16(data[12:14])
	fileHeader.VRes = BytesToUInt16(data[14:16])
	fileHeader.Palette = [48]byte(data[16:64])
	fileHeader.Reserved = uint8(data[64])
	fileHeader.Planes = uint8(data[65])
	fileHeader.BytePerLine = BytesToUInt16(data[66:68])
	fileHeader.PaletteInfo = BytesToUInt16(data[68:70])
	fileHeader.HScreenSize = BytesToUInt16(data[70:72])
	fileHeader.VScreenSize = BytesToUInt16(data[72:74])
	fileHeader.Filler = [54]byte(data[74:128])
	return fileHeader
}

func newPCXImage(data []byte) PCXImage {
	imageDataRightBound := len(data) - OptionalPaletteSizeBytes

	fileHeader := newPCXHeader(data[:128])
	imageData := data[128:imageDataRightBound]

	var optionalPalette = []RgbTriple{}
	if data[imageDataRightBound-1] == 0x0C {
		optionalPalette = make([]RgbTriple, OptionalPaletteSize)
		for i, j := 0, imageDataRightBound; i < len(optionalPalette); i, j = i+1, j+RGBTripleELementsCount {
			optionalPalette[i] = newRgbTriple(data[j : j+RGBTripleELementsCount])
		}
	}

	decodedData := []byte{}
	for i := 0; i < len(imageData); i++ {
		if (imageData[i] >> 6) == 0b11 {
			count := imageData[i] & 0b00111111
			for j := 0; j < int(count); j++ {
				decodedData = append(decodedData, imageData[i+1])
			}
			i++
		} else {
			decodedData = append(decodedData, imageData[i])
		}
	}

	return PCXImage{FileHeader: fileHeader, Data: imageData, OptionalPalette: optionalPalette, DecodedData: decodedData}
}

func PcxFromBytes(data []byte) PCXImage {
	return newPCXImage(data)
}

func PcxToBytes(image PCXImage) []byte {
	var data []byte

	data = append(data, image.FileHeader.ID)

	data = append(data, image.FileHeader.Version)
	data = append(data, image.FileHeader.Coding)
	data = append(data, image.FileHeader.BitPerPixel)
	data = append(data, UInt16ToBytes(image.FileHeader.XMin)...)
	data = append(data, UInt16ToBytes(image.FileHeader.YMin)...)
	data = append(data, UInt16ToBytes(image.FileHeader.XMax)...)
	data = append(data, UInt16ToBytes(image.FileHeader.YMax)...)
	data = append(data, UInt16ToBytes(image.FileHeader.HRes)...)
	data = append(data, UInt16ToBytes(image.FileHeader.VRes)...)
	data = append(data, image.FileHeader.Palette[:]...)
	data = append(data, image.FileHeader.Reserved)
	data = append(data, image.FileHeader.Planes)
	data = append(data, UInt16ToBytes(image.FileHeader.BytePerLine)...)
	data = append(data, UInt16ToBytes(image.FileHeader.PaletteInfo)...)
	data = append(data, UInt16ToBytes(image.FileHeader.HScreenSize)...)
	data = append(data, UInt16ToBytes(image.FileHeader.VScreenSize)...)
	data = append(data, image.FileHeader.Filler[:]...)

	image.Data = make([]byte, 0)
	dataIndex := 0

	decodedLen := len(image.DecodedData) - 1

	i := 0
	for i < decodedLen {
		image.Data = append(image.Data, 0)

		colorStartIndex := 0
		for j := 0; i < decodedLen && j < 3; j++ {
			if image.DecodedData[i] == image.DecodedData[i+1] {
				colorStartIndex++
				i++
			}
		}
		i -= colorStartIndex

		if image.DecodedData[i] >= 192 && colorStartIndex < 3 {
			image.Data[dataIndex] = 0b11000000
			image.Data = append(image.Data, image.DecodedData[i])
		}

		if colorStartIndex == 3 {
			for i < decodedLen && image.DecodedData[i] == image.DecodedData[i+1] {
				image.Data[dataIndex]++
				i++
			}
			image.Data[dataIndex] += 0b11000000
		}
		dataIndex++
	}

	data = append(data, []byte{0}...)

	return data
}

func EGAToRGB(egaColor byte) RgbTriple {
	rgbColor := RgbTriple{}
	rgbColor.RgbRed = 85 * (((egaColor >> 1) & 2) | (egaColor>>5)&1)
	rgbColor.RgbGreen = 85 * ((egaColor & 2) | (egaColor>>4)&1)
	rgbColor.RgbBlue = 85 * (((egaColor << 1) & 2) | (egaColor>>3)&1)
	return rgbColor
}

func RGBtoEGA(color RgbTriple) byte {
	colorRed := (((color.RgbRed / 85) & 2) << 1) | (((color.RgbRed / 85) & 1) << 5)
	colorGreen := ((color.RgbGreen / 85) & 2) | (((color.RgbGreen / 85) & 1) << 4)
	colorBlue := (((color.RgbBlue / 85) & 2) >> 1) | (((color.RgbBlue / 85) & 1) << 3)
	egaPalette := colorRed | colorGreen | colorBlue

	return egaPalette
}

func PcxGetPixelColor(i int, j int, image PCXImage) RgbTriple {
	var color RgbTriple

	if image.FileHeader.Planes == 1 {
		row := j * int(image.FileHeader.BytePerLine)
		column := i

		index := row + column

		colorIndex := image.DecodedData[index]

		if len(image.OptionalPalette) > 0 {
			color = image.OptionalPalette[colorIndex]
		} else {
			color = EGAToRGB(image.FileHeader.Palette[colorIndex])
		}

	} else if image.FileHeader.Planes == 3 {
		rowR := j * 3 * int(image.FileHeader.BytePerLine)
		rowG := rowR + int(image.FileHeader.BytePerLine)
		rowB := rowG + int(image.FileHeader.BytePerLine)
		column := i

		color.RgbRed = image.DecodedData[rowR+column]
		color.RgbGreen = image.DecodedData[rowG+column]
		color.RgbBlue = image.DecodedData[rowB+column]
	}

	return color
}

func PcxSetPixelColor(i int, j int, color RgbTriple, image PCXImage) {
	if image.FileHeader.Planes == 1 {
		row := j * int(image.FileHeader.BytePerLine)
		column := i

		index := row + column
		colorIndex := 0

		if len(image.OptionalPalette) > 0 {
			sort.Search(len(image.OptionalPalette), func(i int) bool { return image.OptionalPalette[i] == color })
		} else {
			sort.Search(len(image.FileHeader.Palette), func(i int) bool { return image.FileHeader.Palette[i] == RGBtoEGA(color) })
		}

		image.DecodedData[index] = byte(colorIndex)

	} else if image.FileHeader.Planes == 3 {
		rowR := j * 3 * int(image.FileHeader.BytePerLine)
		rowG := rowR + int(image.FileHeader.BytePerLine)
		rowB := rowG + int(image.FileHeader.BytePerLine)
		column := i

		image.DecodedData[rowR+column] = color.RgbRed
		image.DecodedData[rowG+column] = color.RgbGreen
		image.DecodedData[rowB+column] = color.RgbBlue
	}
}
