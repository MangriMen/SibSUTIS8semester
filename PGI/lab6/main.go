package main

import (
	"encoding/json"
	"fmt"
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

func addLogoToBmp(logo BMPImage, opacity float64, x, y int, image BMPImage) BMPImage {
	newImage := BMPImage{}

	newImage.FileHeader = image.FileHeader
	newImage.FileInfo = image.FileInfo
	newImage.RgbQuad = append(newImage.RgbQuad, image.RgbQuad...)
	newImage.ColorIndexArray = append(newImage.ColorIndexArray, image.ColorIndexArray...)
	newImage.Meta = newMeta(image.FileInfo.BitCount, image.FileInfo.Width)

	backgroundColor := RgbQuad{0xff, 0xff, 0xff, 0}

	logoStartX := x
	logoStartY := y

	for i := 0; i < int(logo.FileInfo.Height); i++ {
		for j := 0; j < int(logo.FileInfo.Width); j++ {
			logoPixelColor := GetPixelColor(i, j, logo)
			if logoPixelColor == backgroundColor {
				continue
			}

			imagePixelColor := GetPixelColor(logoStartX+i, logoStartY+j, newImage)

			newPixelColor := RgbQuad{}
			newPixelColor.RgbRed = byte(float64(imagePixelColor.RgbRed)*opacity + float64(logoPixelColor.RgbRed)*(1-opacity))
			newPixelColor.RgbGreen = byte(float64(imagePixelColor.RgbGreen)*opacity + float64(logoPixelColor.RgbGreen)*(1-opacity))
			newPixelColor.RgbBlue = byte(float64(imagePixelColor.RgbBlue)*opacity + float64(logoPixelColor.RgbBlue)*(1-opacity))

			SetPixelColor(logoStartX+i, logoStartY+j, newPixelColor, newImage)
		}
	}

	return newImage
}

func main() {
	imageFilename, err := filepath.Abs("../_carib_TC.bmp")
	if err != nil {
		panic(err)
	}

	logoFilename, err := filepath.Abs("logo.bmp")
	if err != nil {
		panic(err)
	}

	filenameWithoutExt := strings.Split(filepath.Base(imageFilename), ".")[0]
	outputFilename := filenameWithoutExt + "_With_Logo.bmp"

	image := bmpFromBytes(readFileAsBytes(imageFilename))
	logo := bmpFromBytes(readFileAsBytes(logoFilename))
	printBMPStructure(image)

	convertedImage := addLogoToBmp(logo, 0.5, 200, 200, image)

	writeFileAsBytes(outputFilename, bmpToBytes(convertedImage))
}
