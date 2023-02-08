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

func splitByColorMedian(rgbQuad []RgbQuad, median int, colorName string) ([]RgbQuad, []RgbQuad) {
	partA := []RgbQuad{}
	partB := []RgbQuad{}

	switch colorName {
	case "red":
		for _, color := range rgbQuad {
			if int(color.RgbRed) >= median {
				partA = append(partA, color)
			} else {
				partB = append(partB, color)
			}
		}
	case "green":
		for _, color := range rgbQuad {
			if int(color.RgbGreen) >= median {
				partA = append(partA, color)
			} else {
				partB = append(partB, color)
			}
		}
	case "blue":
		for _, color := range rgbQuad {
			if int(color.RgbBlue) >= median {
				partA = append(partA, color)
			} else {
				partB = append(partB, color)
			}
		}
	}

	return partA, partB
}

func medianCut(rgbQuad []RgbQuad, level int) []RgbQuad {
	bounds := ColorBounds{Bounds{255, 0}, Bounds{255, 0}, Bounds{255, 0}}
	for _, color := range rgbQuad {
		if int(color.RgbRed) > bounds.Red.end {
			bounds.Red.end = int(color.RgbRed)
		}
		if int(color.RgbRed) < bounds.Red.begin {
			bounds.Red.begin = int(color.RgbRed)
		}

		if int(color.RgbGreen) > bounds.Green.end {
			bounds.Green.end = int(color.RgbGreen)
		}
		if int(color.RgbGreen) < bounds.Green.begin {
			bounds.Green.begin = int(color.RgbGreen)
		}

		if int(color.RgbBlue) > bounds.Blue.end {
			bounds.Blue.end = int(color.RgbBlue)
		}
		if int(color.RgbBlue) < bounds.Blue.begin {
			bounds.Blue.begin = int(color.RgbBlue)
		}
	}

	redLength := bounds.Red.end - bounds.Red.begin
	greenLength := bounds.Green.end - bounds.Green.begin
	blueLength := bounds.Blue.end - bounds.Blue.begin

	maxLength := int(math.Max(float64(redLength), math.Max(float64(blueLength), float64(greenLength))))

	colorName := ""
	median := 0
	switch maxLength {
	case redLength:
		colorName = "red"
		median = (bounds.Red.begin + bounds.Red.end) / 2
	case greenLength:
		colorName = "green"
		median = (bounds.Green.begin + bounds.Green.end) / 2
	case blueLength:
		colorName = "blue"
		median = (bounds.Blue.begin + bounds.Blue.end) / 2
	}

	if level >= 8 {
		/* Average color from cube */
		// averageRed := 0
		// averageGreen := 0
		// averageBlue := 0
		// for _, color := range rgbQuad {
		// 	averageRed += int(color.RgbRed)
		// 	averageGreen += int(color.RgbGreen)
		// 	averageBlue += int(color.RgbBlue)
		// }
		// return []RgbQuad{{RgbRed: byte(averageRed / len(rgbQuad)), RgbGreen: byte(averageGreen / len(rgbQuad)), RgbBlue: byte(averageBlue / len(rgbQuad)), RgbReserved: 0xff}}

		/* Middle color from cube */
		// switch colorName {
		// case "red":
		// 	sort.SliceStable(rgbQuad, func(i, j int) bool {
		// 		return rgbQuad[i].RgbRed > rgbQuad[j].RgbRed
		// 	})
		// case "green":
		// 	sort.SliceStable(rgbQuad, func(i, j int) bool {
		// 		return rgbQuad[i].RgbGreen > rgbQuad[j].RgbGreen
		// 	})
		// case "blue":
		// 	sort.SliceStable(rgbQuad, func(i, j int) bool {
		// 		return rgbQuad[i].RgbBlue > rgbQuad[j].RgbBlue
		// 	})
		// }

		// return []RgbQuad{rgbQuad[len(rgbQuad)/2]}

		/* ??? */
		if len(rgbQuad) > 0 {
			return []RgbQuad{rgbQuad[0]}
		}
		return []RgbQuad{}
	}

	partA, partB := splitByColorMedian(rgbQuad, median, colorName)

	palette := []RgbQuad{}
	palette = append(palette, medianCut(partA, level+1)...)
	palette = append(palette, medianCut(partB, level+1)...)
	return palette
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
	newImage.RgbQuad = medianCut(frequentColors, 0)

	newImage.ColorIndexArray = make([]byte, newImage.FileInfo.SizeImage)
	for i := 0; i < int(newImage.FileInfo.Height); i++ {
		for j := 0; j < int(newImage.FileInfo.Width); j++ {
			color := GetPixelColor(i, j, image)
			similarColorIndex := getIndexOfSimilarColor(color, newImage.RgbQuad)
			setColorIndexToPixel(i, j, similarColorIndex, newImage)
		}
	}

	return newImage
}

func main() {
	filename, err := filepath.Abs("../Belsanku.bmp")
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
