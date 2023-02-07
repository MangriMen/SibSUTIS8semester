package main

import (
	"encoding/json"
	"fmt"
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

func printBMPStructure(image BMPImage) {
	prefix := ""
	indent := "  "

	header, _ := json.MarshalIndent(image.FileHeader, prefix, indent)
	info, _ := json.MarshalIndent(image.FileInfo, prefix, indent)
	rgbQuad, _ := json.MarshalIndent(image.RgbQuad, prefix, indent)

	fmt.Printf("File header: %s\n", header)
	fmt.Printf("File info: %s\n", info)
	fmt.Printf("Palette: %s\n", rgbQuad)
}

func averageColorValues(rgbQuad RgbQuad) RgbQuad {
	averageValue := (rgbQuad.RgbBlue / 3) + (rgbQuad.RgbGreen / 3 ) + (rgbQuad.RgbRed / 3)
	rgbQuad.RgbBlue = averageValue
	rgbQuad.RgbGreen = averageValue
	rgbQuad.RgbRed = averageValue
	return rgbQuad
}

func convertBMPToBnW(image BMPImage) BMPImage {
	newImage := image
	rgbQuadElementsCount := int(math.Pow(2, float64(newImage.FileInfo.BitCount)))
	for i := 0; i < rgbQuadElementsCount; i++ {
		newImage.RgbQuad[i] = averageColorValues(newImage.RgbQuad[i])
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

	printBMPStructure(image)

	bnwImage := convertBMPToBnW(image)

	writeBMP(outputFilename, bnwImage)
}
