package main

import (
	"encoding/json"
	"fmt"
	"math"
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

func createBorderOnBMP(image BMPImage, borderWidth int, borderColorIndex int, borderColor RgbQuad) BMPImage {
	newImage := BMPImage{}
	newImage.FileHeader = image.FileHeader
	newImage.FileInfo = image.FileInfo
	newImage.Meta = image.Meta

	newImage.FileInfo.Width += int32(borderWidth) * 2
	newImage.FileInfo.Height += int32(borderWidth) * 2

	bytesPerRowAligned := 4 * int32(math.Ceil(float64(int(newImage.FileInfo.Width)*int(newImage.FileInfo.BitCount))/32))

	if newImage.FileInfo.BitCount < BitsPerByte {
		newImage.Meta.widthAligned = bytesPerRowAligned * int32(newImage.Meta.bitsPerPixel)
	} else {
		newImage.Meta.widthAligned = bytesPerRowAligned / int32(newImage.Meta.bytesPerColor)
	}

	newImage.FileInfo.SizeImage = uint32(bytesPerRowAligned) * uint32(newImage.FileInfo.Height)

	newImage.RgbQuad = image.RgbQuad

	newImage.ColorIndexArray = make([]byte, newImage.FileInfo.SizeImage)

	width := int(newImage.FileInfo.Width)
	height := int(newImage.FileInfo.Height)

	if image.FileInfo.BitCount <= 8 {
		for i, oldI := borderWidth, 0; i < height-borderWidth; i, oldI = i+1, oldI+1 {
			for j, oldJ := borderWidth, 0; j < width-borderWidth; j, oldJ = j+1, oldJ+1 {
				setColorIndexToPixel(i, j, int(getColorIndexFromPixel(oldI, oldJ, image)), newImage)
			}
		}
	} else {
		for i, oldI := borderWidth, 0; i < height-borderWidth; i, oldI = i+1, oldI+1 {
			for j, oldJ := borderWidth, 0; j < width-borderWidth; j, oldJ = j+1, oldJ+1 {
				SetPixelColor(i, j, GetPixelColor(oldI, oldJ, image), newImage)
			}
		}
	}

	if image.FileInfo.BitCount <= 8 {
		for i := 0; i < height; i++ {
			for j := 0; j < width; j++ {
				if (i < borderWidth || i >= height-borderWidth) || (j < borderWidth || j >= width-borderWidth) {
					setColorIndexToPixel(i, j, borderColorIndex, newImage)
				}
			}
		}
	} else {
		for i := 0; i < height; i++ {
			for j := 0; j < width; j++ {
				if (i < borderWidth || i >= height-borderWidth) || (j < borderWidth || j >= width-borderWidth) {
					SetPixelColor(i, j, borderColor, newImage)
				}
			}
		}
	}

	return newImage
}

func main() {
	filename, err := filepath.Abs("../_carib_TC.bmp")
	if err != nil {
		panic(err)
	}

	filenameWithoutExt := strings.Split(filepath.Base(filename), ".")[0]
	outputFilename := filenameWithoutExt + "_Border.bmp"

	image := readBMP(filename)

	printBMPStructure(image)

	borderWidth := 50
	fmt.Printf("Border width: %dpx\n", borderWidth)

	var borderColorIndex int = 0
	var borderColor RgbQuad = RgbQuad{}

	fmt.Print("Border color: ")
	if image.FileInfo.BitCount <= 8 {
		borderColorIndex = rand.Intn(len(image.RgbQuad))
		fmt.Printf("%+v", image.RgbQuad[borderColorIndex])
	} else {
		borderColor = RgbQuad{byte(rand.Intn(255)), byte(rand.Intn(255)), byte(rand.Intn(255)), 0xff}
		fmt.Printf("%+v", borderColor)
	}
	fmt.Println()

	bnwImage := createBorderOnBMP(image, borderWidth, borderColorIndex, borderColor)

	writeBMP(outputFilename, bnwImage)
}
