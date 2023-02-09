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
	redComponent := math.Pow(float64(b.RgbRed-a.RgbRed), 2)
	greenComponent := math.Pow(float64(b.RgbGreen-a.RgbGreen), 2)
	blueComponent := math.Pow(float64(b.RgbBlue-a.RgbBlue), 2)
	return 30*redComponent + 59*greenComponent + 11*blueComponent
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
	for i, minDelta := 0, 1e18; i < len(rgbQuad); i++ {
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
			similarColorIndex := uint64(getIndexOfSimilarColor(color, newImage.RgbQuad))
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

func getFrequentColorKeysTrueColor(image BMPImage) []RgbQuad {
	colorFrequency := make(map[RgbQuad]int)
	for i := 0; i < int(image.FileInfo.Height); i++ {
		for j := 0; j < int(image.FileInfo.Width); j++ {
			colorFrequency[GetPixelColor(i, j, image)]++
		}
	}

	keys := make([]RgbQuad, 0, len(colorFrequency))
	for key := range colorFrequency {
		keys = append(keys, key)
	}

	sort.SliceStable(keys, func(i, j int) bool {
		return colorFrequency[keys[i]] > colorFrequency[keys[j]]
	})

	return keys
}

type ColorBounds struct {
	Red   Bounds
	Green Bounds
	Blue  Bounds
}

func splitByColorMedian(rgbQuad []RgbQuad, median uint64, colorName string) ([]RgbQuad, []RgbQuad) {
	partA := []RgbQuad{}
	partB := []RgbQuad{}

	switch colorName {
	case "red":
		for _, color := range rgbQuad {
			if uint64(color.RgbRed) >= median {
				partA = append(partA, color)
			} else {
				partB = append(partB, color)
			}
		}
	case "green":
		for _, color := range rgbQuad {
			if uint64(color.RgbGreen) >= median {
				partA = append(partA, color)
			} else {
				partB = append(partB, color)
			}
		}
	case "blue":
		for _, color := range rgbQuad {
			if uint64(color.RgbBlue) >= median {
				partA = append(partA, color)
			} else {
				partB = append(partB, color)
			}
		}
	}

	return partA, partB
}

func split_bucket(bucket []RgbQuad) ([]RgbQuad, []RgbQuad) {
	bounds := ColorBounds{Bounds{255, 0}, Bounds{255, 0}, Bounds{255, 0}}

	for _, color := range bucket {
		if int64(color.RgbRed) > int64(bounds.Red.end) {
			bounds.Red.end = uint64(color.RgbRed)
		}
		if int64(color.RgbRed) < int64(bounds.Red.begin) {
			bounds.Red.begin = uint64(color.RgbRed)
		}

		if int64(color.RgbGreen) > int64(bounds.Green.end) {
			bounds.Green.end = uint64(color.RgbGreen)
		}
		if int64(color.RgbGreen) < int64(bounds.Green.begin) {
			bounds.Green.begin = uint64(color.RgbGreen)
		}

		if int64(color.RgbBlue) > int64(bounds.Blue.end) {
			bounds.Blue.end = uint64(color.RgbBlue)
		}
		if int64(color.RgbBlue) < int64(bounds.Blue.begin) {
			bounds.Blue.begin = uint64(color.RgbBlue)
		}
	}

	redLength := bounds.Red.end - bounds.Red.begin
	greenLength := bounds.Green.end - bounds.Green.begin
	blueLength := bounds.Blue.end - bounds.Blue.begin

	maxLength := int64(math.Round(math.Max(float64(redLength), math.Max(float64(blueLength), float64(greenLength)))))

	colorName := ""
	switch maxLength {
	case int64(redLength):
		colorName = "red"
	case int64(greenLength):
		colorName = "green"
	case int64(blueLength):
		colorName = "blue"
	}

	switch colorName {
	case "red":
		sort.Slice(bucket, func(i, j int) bool {
			return bucket[i].RgbRed > bucket[j].RgbRed
		})
	case "green":
		sort.Slice(bucket, func(i, j int) bool {
			return bucket[i].RgbGreen > bucket[j].RgbGreen
		})
	case "blue":
		sort.Slice(bucket, func(i, j int) bool {
			return bucket[i].RgbBlue > bucket[j].RgbBlue
		})
	}

	return bucket[:len(bucket)/2], bucket[len(bucket)/2:]
}

func medianCut(rgbQuad []RgbQuad, colors int) ([]RgbQuad, map[RgbQuad]int) {
	buckets := [][]RgbQuad{rgbQuad}
	for len(buckets) < colors {
		new_buckets := [][]RgbQuad{}
		for _, bucket := range buckets {
			partA, partB := split_bucket(bucket)
			new_buckets = append(new_buckets, partA)
			new_buckets = append(new_buckets, partB)
		}
		buckets = new_buckets
	}

	palette := make([]RgbQuad, colors)
	colorMap := make(map[RgbQuad]int, 0)

	for i, bucket := range buckets {
		var avgRed float64 = 0
		var avgGreen float64 = 0
		var avgBlue float64 = 0

		for _, pixel := range bucket {
			avgRed += float64(pixel.RgbRed) / float64(len(bucket))
			avgGreen += float64(pixel.RgbGreen) / float64(len(bucket))
			avgBlue += float64(pixel.RgbBlue) / float64(len(bucket))
			colorMap[pixel] = i
		}
		palette[i] = RgbQuad{RgbRed: byte(avgRed), RgbGreen: byte(avgGreen), RgbBlue: byte(avgBlue), RgbReserved: 0xff}
	}

	return palette, colorMap
}

func convertTrueColorBmpTo256CBmp(image BMPImage) BMPImage {
	var newBitCount uint16 = 8
	var colorsCount uint32 = uint32(math.Pow(2, float64(newBitCount)))

	var newImage BMPImage

	newImage.FileHeader = image.FileHeader
	newImage.FileInfo = image.FileInfo
	newImage.Meta = newMeta(newBitCount, image.FileInfo.Width)

	newImage.FileInfo.BitCount = newBitCount
	newImage.FileInfo.SizeImage = newImage.Meta.widthAligned * uint32(newImage.FileInfo.Height) / uint32(newImage.Meta.bitsPerPixel)
	newImage.FileInfo.ColorUsed = colorsCount
	newImage.FileInfo.ColorImportant = colorsCount

	newImage.FileHeader.Offset = uint32(BitmapFileInfoBounds.end) + colorsCount*uint32(RGBQuadElementsCount)
	newImage.FileHeader.Size = newImage.FileInfo.SizeImage + newImage.FileHeader.Offset

	frequentColors := getFrequentColorKeysTrueColor(image)
	newPalette, colorMap := medianCut(frequentColors, int(colorsCount))

	newImage.RgbQuad = newPalette

	newImage.ColorIndexArray = make([]byte, newImage.FileInfo.SizeImage)
	for i := 0; i < int(newImage.FileInfo.Height); i++ {
		for j := 0; j < int(newImage.FileInfo.Width); j++ {
			color := GetPixelColor(i, j, image)
			setColorIndexToPixel(i, j, uint64(colorMap[color]), newImage)
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
	outputFilename := filenameWithoutExt + "_To_256C.bmp"

	fmt.Println("Original image: ")
	image := bmpFromBytes(readFileAsBytes(filename))
	printBMPStructure(image)

	fmt.Println("Converted 256 colors to 16 colors image: ")
	convertedImage := convertTrueColorBmpTo256CBmp(image)
	printBMPStructure(convertedImage)

	writeFileAsBytes(outputFilename, bmpToBytes(convertedImage))
}
