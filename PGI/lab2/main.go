package main

import (
	"encoding/json"
	"fmt"
	"math/rand"
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

func createBorderOnBMP(image BMPImage) BMPImage {
	newImage := image

	width := int(newImage.FileInfo.Width)
	height := int(newImage.FileInfo.Height)

	borderColor := byte(rand.Intn(len(newImage.RgbQuad)))
	borderWidth := 15

	for i := 0; i < height; i++ {
		for j := 0; j < width; j++ {
			if (i < borderWidth || i >= height-borderWidth) || (j < borderWidth || j >= width-borderWidth) {
				index := i*(width/2) + j/2
				for k := 0; k < int(newImage.FileInfo.BitCount); k++ {
					SetPixel(newImage.ColorIndexArray[index:index+1], borderColor, k, newImage.FileInfo.BitCount)
				}
			}
		}
	}

	// for i := 0; i < height; i++ {
	// 	for j := 0; j < width/2; j++ {
	// 		if (i < borderWidth || i >= height-borderWidth) || (j < borderWidth/2 || j >= width/2-borderWidth/2) {
	// 			newImage.ColorIndexArray[i*(width/2)+j] = borderColor<<4 | borderColor
	// 		}
	// 	}
	// }

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

	bnwImage := createBorderOnBMP(image)

	writeBMP(outputFilename, bnwImage)
}
