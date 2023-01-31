package main

import (
	"encoding/json"
	"fmt"
	"math/rand"
	"os"
	"path/filepath"
	"strings"
	"time"
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

func createBorderOnBMP(image BMPImage, borderColorIndex int, borderWidth int) BMPImage {
	newImage := image

	width := int(newImage.FileInfo.Width)
	height := int(newImage.FileInfo.Height)

	for i := 0; i < height; i++ {
		for j := 0; j < width; j++ {
			if (i < borderWidth || i >= height-borderWidth) || (j < borderWidth || j >= width-borderWidth) {
				SetPixel(i, j, borderColorIndex, newImage.ColorIndexArray, newImage.FileInfo)
			}
		}
	}

	return newImage
}

func main() {
	rand.Seed(time.Now().UnixNano())

	filename, err := filepath.Abs("../CAT16.bmp")
	if err != nil {
		panic(err)
	}

	filenameWithoutExt := strings.Split(filepath.Base(filename), ".")[0]
	outputFilename := filenameWithoutExt + "_Border.bmp"

	image := readBMP(filename)

	printBMPStructure(image)

	borderColorIndex := rand.Intn(len(image.RgbQuad))
	borderWidth := 15
	fmt.Printf("Border color: %+v", image.RgbQuad[borderColorIndex])

	bnwImage := createBorderOnBMP(image, borderColorIndex, borderWidth)

	writeBMP(outputFilename, bnwImage)
}
