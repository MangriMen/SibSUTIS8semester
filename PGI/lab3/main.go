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

type rotationDirection int

const (
	left  rotationDirection = iota
	right rotationDirection = iota
)

func rotateBMP(image BMPImage, rotationDirection rotationDirection) BMPImage {
	newImage := BMPImage{}

	newImage.FileHeader = image.FileHeader
	newImage.FileInfo = image.FileInfo
	newImage.FileInfo.Width, newImage.FileInfo.Height = newImage.FileInfo.Height, newImage.FileInfo.Width

	newImage.ColorIndexArray = make([]byte, 0)
	newImage.RgbQuad = make([]RgbQuad, 0)

	newImage.ColorIndexArray = append(newImage.ColorIndexArray, image.ColorIndexArray...)
	newImage.RgbQuad = append(newImage.RgbQuad, image.RgbQuad...)

	width := int(image.FileInfo.Width)
	height := int(image.FileInfo.Height)

	if rotationDirection == left {
		for i := 0; i < height; i++ {
			for j := 0; j < width; j++ {
				SetPixel(j, i, int(GetPixel(height-i-1, j, image.ColorIndexArray, image.FileInfo)), newImage.ColorIndexArray, newImage.FileInfo)
			}
		}
	} else {
		for i := 0; i < height; i++ {
			for j := 0; j < width; j++ {
				SetPixel(j, i, int(GetPixel(i, width-j-1, image.ColorIndexArray, image.FileInfo)), newImage.ColorIndexArray, newImage.FileInfo)
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
	outputFilename := filenameWithoutExt + "_Rotation.bmp"

	image := readBMP(filename)

	printBMPStructure(image)

	bnwImage := rotateBMP(image, right)

	writeBMP(outputFilename, bnwImage)
}
