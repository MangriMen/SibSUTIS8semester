package main

import (
	"fmt"
	"math"
	"path/filepath"
	"sort"

	"example.com/images/pcx"
	"example.com/pgi_utils/binary"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
	"example.com/pgi_utils/types"
	"golang.org/x/exp/slices"
)

type ColorBounds struct {
	Red   types.Bounds
	Green types.Bounds
	Blue  types.Bounds
}

func split_bucket(bucket []types.RGBTriple) ([]types.RGBTriple, []types.RGBTriple) {
	bounds := ColorBounds{types.Bounds{Begin: 255, End: 0}, types.Bounds{Begin: 255, End: 0}, types.Bounds{Begin: 255, End: 0}}

	for _, color := range bucket {
		if int(color.RGBTRed) > bounds.Red.End {
			bounds.Red.End = int(color.RGBTRed)
		}
		if int(color.RGBTRed) < bounds.Red.Begin {
			bounds.Red.Begin = int(color.RGBTRed)
		}

		if int(color.RGBTGreen) > bounds.Green.End {
			bounds.Green.End = int(color.RGBTGreen)
		}
		if int(color.RGBTGreen) < bounds.Green.Begin {
			bounds.Green.Begin = int(color.RGBTGreen)
		}

		if int(color.RGBTBlue) > bounds.Blue.End {
			bounds.Blue.End = int(color.RGBTBlue)
		}
		if int(color.RGBTBlue) < bounds.Blue.Begin {
			bounds.Blue.Begin = int(color.RGBTBlue)
		}
	}

	redLength := bounds.Red.End - bounds.Red.Begin
	greenLength := bounds.Green.End - bounds.Green.Begin
	blueLength := bounds.Blue.End - bounds.Blue.Begin

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
			return bucket[i].RGBTRed > bucket[j].RGBTRed
		})
	case "green":
		sort.Slice(bucket, func(i, j int) bool {
			return bucket[i].RGBTGreen > bucket[j].RGBTGreen
		})
	case "blue":
		sort.Slice(bucket, func(i, j int) bool {
			return bucket[i].RGBTBlue > bucket[j].RGBTBlue
		})
	}

	return bucket[:len(bucket)/2], bucket[len(bucket)/2:]
}

func medianCut(rgbQuad []types.RGBTriple, colors int) ([]types.RGBTriple, map[types.RGBTriple]int) {
	buckets := [][]types.RGBTriple{rgbQuad}
	for len(buckets) < colors {
		new_buckets := [][]types.RGBTriple{}
		for _, bucket := range buckets {
			partA, partB := split_bucket(bucket)
			new_buckets = append(new_buckets, partA)
			new_buckets = append(new_buckets, partB)
		}
		buckets = new_buckets
	}

	palette := make([]types.RGBTriple, colors)
	colorMap := make(map[types.RGBTriple]int, 0)

	for i, bucket := range buckets {
		var avgRed float64 = 0
		var avgGreen float64 = 0
		var avgBlue float64 = 0

		for _, pixel := range bucket {
			avgRed += float64(pixel.RGBTRed) / float64(len(bucket))
			avgGreen += float64(pixel.RGBTGreen) / float64(len(bucket))
			avgBlue += float64(pixel.RGBTBlue) / float64(len(bucket))
			colorMap[pixel] = i
		}
		palette[i] = types.RGBTriple{RGBTRed: byte(avgRed), RGBTGreen: byte(avgGreen), RGBTBlue: byte(avgBlue)}
	}

	return palette, colorMap
}

func getFrequentColorKeysTrueColor(image pcx.PCXImage) []types.RGBTriple {
	colorFrequency := make(map[types.RGBTriple]int)
	for i := 0; i < int(image.FileHeader.YMax); i++ {
		for j := 0; j < int(image.FileHeader.XMax); j++ {
			colorFrequency[pcx.GetPixelColor(i, j, image)]++
		}
	}

	keys := make([]types.RGBTriple, 0, len(colorFrequency))
	for key := range colorFrequency {
		keys = append(keys, key)
	}

	sort.SliceStable(keys, func(i, j int) bool {
		return colorFrequency[keys[i]] > colorFrequency[keys[j]]
	})

	return keys
}

func convertPixelsToPalette(colorsCount int, newImage, sourceImage pcx.PCXImage, colorMap map[types.RGBTriple]int) {
	switch colorsCount {
	case 16:
		for i := 0; i < int(sourceImage.FileHeader.YMax); i++ {
			for j := 0; j < int(sourceImage.FileHeader.XMax); j++ {
				color := pcx.EGAToRGB(pcx.RGBtoEGA(pcx.GetPixelColor(i, j, sourceImage)))
				pcx.SetPixelColorIndex(i, j, uint8(colorMap[color]), newImage)
			}
		}
	case 256:
		for i := 0; i < int(sourceImage.FileHeader.YMax); i++ {
			for j := 0; j < int(sourceImage.FileHeader.XMax); j++ {
				color := pcx.GetPixelColor(i, j, sourceImage)
				pcx.SetPixelColorIndex(i, j, uint8(colorMap[color]), newImage)
			}
		}
	}
}

func convertTrueColorPcxToPalettePcx(sourceImage pcx.PCXImage, bitCount int) pcx.PCXImage {
	colorsCount := int(math.Pow(2, float64(bitCount)))

	newImage := pcx.GetCopy(sourceImage)

	newImage.FileHeader.BitPerPixel = 8
	newImage.FileHeader.Planes = 1
	newImage.FileHeader.BytePerLine = sourceImage.FileHeader.BytePerLine / uint16(binary.BitsPerByte/newImage.FileHeader.BitPerPixel)

	frequentColors := getFrequentColorKeysTrueColor(sourceImage)

	switch colorsCount {
	case 16:
		uniqueEgaColors := []byte{}
		for _, color := range frequentColors {
			egaColor := pcx.RGBtoEGA(color)
			if !slices.Contains(uniqueEgaColors, egaColor) {
				uniqueEgaColors = append(uniqueEgaColors, egaColor)
			}
		}

		sort.SliceStable(uniqueEgaColors, func(i, j int) bool {
			return uniqueEgaColors[i] < uniqueEgaColors[j]
		})

		frequentColors = []types.RGBTriple{}
		for _, color := range uniqueEgaColors {
			rgbColor := pcx.EGAToRGB(color)
			if !slices.Contains(frequentColors, rgbColor) {
				frequentColors = append(frequentColors, rgbColor)
			}
		}
	}

	newPalette, colorMap := medianCut(frequentColors, int(colorsCount))

	switch colorsCount {
	case 16:
		for i := 0; i < len(newPalette); i++ {
			newImage.FileHeader.Palette[i] = pcx.RGBtoEGA(newPalette[i])
		}
	case 256:
		newImage.OptionalPalette = make([]types.RGBTriple, len(newPalette))
		for i := 0; i < len(newPalette); i++ {
			newImage.OptionalPalette[i] = types.RGBTriple{RGBTRed: newPalette[i].RGBTBlue, RGBTGreen: newPalette[i].RGBTGreen, RGBTBlue: newPalette[i].RGBTRed}
		}
	}

	newImage.DecodedData = make([]byte, int(newImage.FileHeader.BytePerLine)*(int(newImage.FileHeader.YMax))*2)
	convertPixelsToPalette(colorsCount, newImage, sourceImage, colorMap)

	return newImage
}

func main() {
	inputFilename, err := filepath.Abs("../carib_TC.pcx")
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(inputFilename) + "_4bit.pcx"

	image := pcx.FromBytes(file.Read(inputFilename))

	fmt.Println("Original image: ")
	helpers.PrintPCXStructure(image)

	newImage := convertTrueColorPcxToPalettePcx(image, 4)

	fmt.Println("New image: ")
	helpers.PrintPCXStructure(newImage)

	file.Write(outputFilename, pcx.ToBytes(newImage))
}
