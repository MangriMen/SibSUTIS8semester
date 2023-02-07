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

type rotationDirection int

const (
	left  rotationDirection = iota
	right rotationDirection = iota
)

func rotateBMP(image BMPImage, rotationDirection rotationDirection) BMPImage {
	newImage := BMPImage{}

	newImage.FileHeader = image.FileHeader
	newImage.FileInfo = image.FileInfo
	newImage.Meta = image.Meta

	newImage.FileInfo.Width, newImage.FileInfo.Height = newImage.FileInfo.Height, newImage.FileInfo.Width

	bytesPerRowAligned := 4 * int32(math.Ceil(float64(int(newImage.FileInfo.Width)*int(newImage.FileInfo.BitCount))/32))
	newImage.Meta.widthAlignedBytes = bytesPerRowAligned
	if newImage.FileInfo.BitCount < BitsPerByte {
		newImage.Meta.widthAligned = bytesPerRowAligned * int32(newImage.Meta.bitsPerPixel)
	} else {
		newImage.Meta.widthAligned = int32(math.Round(float64(bytesPerRowAligned) / float64(newImage.Meta.bytesPerColor)))
	}

	newImage.FileInfo.SizeImage = uint32(bytesPerRowAligned) * uint32(newImage.FileInfo.Height)

	newImage.ColorIndexArray = make([]byte, 0)
	newImage.RgbQuad = make([]RgbQuad, 0)

	newImage.ColorIndexArray = append(newImage.ColorIndexArray, image.ColorIndexArray...)
	newImage.RgbQuad = append(newImage.RgbQuad, image.RgbQuad...)

	width := int(image.FileInfo.Width)
	height := int(image.FileInfo.Height)

	if rotationDirection == right {
		if newImage.FileInfo.BitCount <= 8 {
			for i := 0; i < height; i++ {
				for j := 0; j < width; j++ {
					setColorIndexToPixel(j, i, int(getPixelIndex(height-i-1, j, image)), newImage)
				}
			}
		} else {
			for i := 0; i < height; i++ {
				for j := 0; j < width; j++ {
					SetPixelColor(j, i, GetPixelColor(height-i-1, j, image), newImage)
				}
			}
		}
	} else {
		if newImage.FileInfo.BitCount <= 8 {
			for i := 0; i < height; i++ {
				for j := 0; j < width; j++ {
					setColorIndexToPixel(j, i, int(getPixelIndex(i, width-j-1, image)), newImage)
				}
			}
		} else {
			for i := 0; i < height; i++ {
				for j := 0; j < width; j++ {
					SetPixelColor(j, i, GetPixelColor(i, width-j-1, image), newImage)
				}
			}
		}
	}

	return newImage
}

func main() {
	filename, err := filepath.Abs("../CAT16.bmp")
	if err != nil {
		panic(err)
	}

	filenameWithoutExt := strings.Split(filepath.Base(filename), ".")[0]
	outputFilename := filenameWithoutExt + "_Rotation.bmp"

	image := readBMP(filename)

	printBMPStructure(image)

	bnwImage := rotateBMP(image, left)

	writeBMP(outputFilename, bnwImage)
}
