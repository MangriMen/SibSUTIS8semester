package main

import (
	"encoding/json"
	"fmt"
	"math"
	"os"
	"path/filepath"
	"sort"
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

func getColorDelta(a, b RgbQuad) float64 {
	redComponent := math.Pow(float64(a.RgbRed-b.RgbRed), 2)
	greenComponent := math.Pow(float64(a.RgbGreen-b.RgbGreen), 2)
	blueComponent := math.Pow(float64(a.RgbBlue-b.RgbBlue), 2)
	return redComponent + greenComponent + blueComponent
}

func getFrequentColorKeys(image BMPImage) []int {
	colorFrequency := make(map[int]int)
	for i := 0; i < int(image.FileInfo.Height); i++ {
		for j := 0; j < int(image.FileInfo.Width); j++ {
			colorFrequency[int(getColorIndexFromPixel(i, j, image))] += 1
		}
	}

	keys := make([]int, 0, len(colorFrequency))
	for key := range colorFrequency {
		keys = append(keys, key)
	}

	sort.SliceStable(keys, func(i, j int) bool {
		return colorFrequency[keys[i]] > colorFrequency[keys[j]]
	})

	return keys
}

func getIndexOfSimilarColor(color RgbQuad, rgbQuad []RgbQuad) int {
	var minColorIndexDelta int
	for i, minDelta := 0, 1e9; i < len(rgbQuad); i++ {
		delta := getColorDelta(color, rgbQuad[i])
		if delta < minDelta {
			minDelta = delta
			minColorIndexDelta = i
		}
	}
	return minColorIndexDelta
}

func convert256CBmpTo16CBmp(image BMPImage) BMPImage {
	var newBitCount uint16 = 4
	var colorsCount uint32 = uint32(math.Pow(float64(newBitCount), 2))

	var newImage BMPImage

	newImage.FileHeader = image.FileHeader
	newImage.FileInfo = image.FileInfo
	newImage.Meta = newMeta(newBitCount, image.FileInfo.Width)

	newImage.FileHeader.Offset = uint32(BitmapFileInfoBounds.end) + colorsCount*uint32(RGBQuadElementsCount)
	newImage.FileHeader.Size = newImage.FileInfo.SizeImage + newImage.FileHeader.Offset

	newImage.FileInfo.BitCount = newBitCount
	newImage.FileInfo.SizeImage = newImage.Meta.widthAligned * uint32(newImage.FileInfo.Height) / uint32(newImage.Meta.bitsPerPixel)
	newImage.FileInfo.ColorUsed = colorsCount
	newImage.FileInfo.ColorImportant = colorsCount

	frequentColorKeys := getFrequentColorKeys(image)
	newImage.RgbQuad = make([]RgbQuad, colorsCount)
	for i := 0; i < len(newImage.RgbQuad); i++ {
		newImage.RgbQuad[i] = image.RgbQuad[frequentColorKeys[i]]
	}

	newImage.ColorIndexArray = make([]byte, newImage.FileInfo.SizeImage)
	for i := 0; i < int(newImage.FileInfo.Height); i++ {
		for j := 0; j < int(newImage.FileInfo.Width); j++ {
			color := image.RgbQuad[getColorIndexFromPixel(i, j, image)]
			similarColorIndex := getIndexOfSimilarColor(color, newImage.RgbQuad)
			setColorIndexToPixel(i, j, similarColorIndex, newImage)
		}
	}

	colorFrequency := make(map[int]int)
	for i := 0; i < int(newImage.FileInfo.Height); i++ {
		for j := 0; j < int(newImage.FileInfo.Width); j++ {
			colorFrequency[int(getColorIndexFromPixel(i, j, newImage))] += 1
		}
	}

	fmt.Printf("Converted image use %d colors\n", len(colorFrequency))

	return newImage
}

func main() {
	filename, err := filepath.Abs("../CAT256.bmp")
	if err != nil {
		panic(err)
	}

	filenameWithoutExt := strings.Split(filepath.Base(filename), ".")[0]
	outputFilename := filenameWithoutExt + "_To_CAT16.bmp"

	fmt.Println("Original image: ")
	image := bmpFromBytes(readFileAsBytes(filename))
	printBMPStructure(image)

	fmt.Println("Converted 256 colors to 16 colors image: ")
	convertedImage := convert256CBmpTo16CBmp(image)
	printBMPStructure(convertedImage)

	writeFileAsBytes(outputFilename, bmpToBytes(convertedImage))
}
