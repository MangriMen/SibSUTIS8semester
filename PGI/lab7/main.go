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

func injectTextInBmp(image BMPImage, text []byte) BMPImage {
	newImage := BMPImage{}

	newImage.FileHeader = image.FileHeader
	newImage.FileInfo = image.FileInfo
	newImage.RgbQuad = append(newImage.RgbQuad, image.RgbQuad...)
	newImage.ColorIndexArray = append(newImage.ColorIndexArray, image.ColorIndexArray...)
	newImage.Meta = newMeta(image.FileInfo.BitCount, image.FileInfo.Width)

	bitsCountForTextData := int(math.Round(float64(len(text)) / float64(newImage.FileInfo.SizeImage) * float64(BitsPerByte)))

	currentTextArrayIndex := 0
	currentTextBit := 0
	for i := 0; i < len(newImage.ColorIndexArray); i++ {
		if currentTextArrayIndex == len(text) {
			break
		}
		for j := 0; j < bitsCountForTextData; j++ {
			newImage.ColorIndexArray[i] = byte(clearBit(int(newImage.ColorIndexArray[i]), uint(j)))
			if hasBit(int(text[currentTextArrayIndex]), uint(currentTextBit)) {
				newImage.ColorIndexArray[i] = byte(setBit(int(newImage.ColorIndexArray[i]), uint(j)))
			}
			currentTextBit++
			if currentTextBit >= 8 {
				currentTextBit = 0
				currentTextArrayIndex++
			}
		}
	}

	for i := currentTextArrayIndex; i < len(text); i++ {
		for j := currentTextBit; j < len(text); j++ {
			fmt.Println("kek")
		}
	}

	return newImage
}

func getTextFromBmp(image BMPImage, textFileSize int) []byte {
	text := make([]byte, textFileSize/8)

	bitsCountForTextData := int(math.Round(float64(len(text)) / float64(image.FileInfo.SizeImage) * float64(BitsPerByte)))

	currentTextArrayIndex := 0
	currentTextBit := 0
	for i := 0; i < len(image.ColorIndexArray); i++ {
		if currentTextArrayIndex == len(text) {
			break
		}
		for j := 0; j < bitsCountForTextData; j++ {
			if hasBit(int(image.ColorIndexArray[i]), uint(j)) {
				text[currentTextArrayIndex] = byte(setBit(int(text[currentTextArrayIndex]), uint(currentTextBit)))
			}
			currentTextBit++
			if currentTextBit >= 8 {
				currentTextBit = 0
				currentTextArrayIndex++
			}
		}
	}

	return text
}

func main() {
	filename, err := filepath.Abs("../_carib_TC.bmp")
	if err != nil {
		panic(err)
	}

	filenameWithoutExt := strings.Split(filepath.Base(filename), ".")[0]

	image := bmpFromBytes(readFileAsBytes(filename))
	printBMPStructure(image)

	sizes := []string{"0.1.txt", "0.2.txt", "0.5.txt"}

	for _, size := range sizes {
		textFilename, err := filepath.Abs(size)
		if err != nil {
			panic(err)
		}

		txtFile := readFileAsBytes(textFilename)
		convertedImage := injectTextInBmp(image, txtFile)

		outputFilename := filenameWithoutExt + "_" + size + "_Stenography.bmp"
		writeFileAsBytes(outputFilename, bmpToBytes(convertedImage))

		textFromBmp := getTextFromBmp(convertedImage, len(txtFile)*8)
		writeFileAsBytes(size+"_from_bmp.txt", textFromBmp)
	}
}
