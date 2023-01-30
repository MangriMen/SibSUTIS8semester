package main

import (
	"math"
	"os"
	"path/filepath"
	"strings"
)

func readFileAsBytes(path string) []byte {
	image, err := os.ReadFile(path)

	if err != nil {
		panic(err)
	}

	return image
}

func writeFileAsBytes(path string, data []byte) {
	var err error

	err = os.RemoveAll(path)

	if err != nil {
		panic(err)
	}

	err = os.WriteFile(path, data, 0644)

	if err != nil {
		panic(err)
	}
}

func readBMP(path string) BMPImage {
	return bmpFromBytes(readFileAsBytes(path))
}

func writeBMP(path string, image BMPImage) {
	writeFileAsBytes(path, bmpToBytes(image))
}

func averageColorValues(rgbQuad RgbQuad) RgbQuad {
	averageValue := (rgbQuad.rgbBlue + rgbQuad.rgbGreen + rgbQuad.rgbRed) / 3
	rgbQuad.rgbBlue = averageValue
	rgbQuad.rgbGreen = averageValue
	rgbQuad.rgbRed = averageValue
	return rgbQuad
}

func convertBMPToBnW(image BMPImage) BMPImage {
	newImage := image
	rgbQuadElementsCount := int(math.Pow(2, float64(newImage.fileInfo.bitCount)))
	for i := 0; i < rgbQuadElementsCount; i++ {
		newImage.rgbQuad[i] = averageColorValues(newImage.rgbQuad[i])
	}
	return newImage
}

func main() {
	filename, err := filepath.Abs("../CAT16.bmp")
	if err != nil {
		panic(err)
	}

	filenameWithoutExt := strings.Split(filepath.Base(filename), ".")[0]
	outputFilename := filenameWithoutExt + "_BnW.bmp"

	image := readBMP(filename)

	bnwImage := convertBMPToBnW(image)

	writeBMP(outputFilename, bnwImage)
}
