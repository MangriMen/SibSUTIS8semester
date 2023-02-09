package pcx

import "example.com/pgi_utils/binary"

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

func FromBytes(data []byte) PCXImage {
	return newPCXImage(data)
}

func EGAToRGB(egaColor byte) RgbTriple {
	rgbColor := RgbTriple{}
	rgbColor.RgbRed = 85 * (((egaColor >> 1) & 2) | (egaColor>>5)&1)
	rgbColor.RgbGreen = 85 * ((egaColor & 2) | (egaColor>>4)&1)
	rgbColor.RgbBlue = 85 * (((egaColor << 1) & 2) | (egaColor>>3)&1)
	return rgbColor
}

func GetPixelColor(i int, j int, image PCXImage) RgbTriple {
	row := j * int(image.FileHeader.BytePerLine)
	column := i

	index := row + column

	colorIndex := image.DecodedData[index]

	var color RgbTriple
	if len(image.OptionalPalette) > 0 {
		color = image.OptionalPalette[colorIndex]
	} else {
		color = EGAToRGB(image.FileHeader.Palette[colorIndex])
	}

	return color
}
