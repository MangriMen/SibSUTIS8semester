package main

import (
	"fmt"
	"math"
	"path/filepath"
	"sort"

	bmp "example.com/images/bitmap"

	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
	"example.com/pgi_utils/types"
)

func getFrequentColorKeys(image bmp.BMPImage) []types.RGBQuad {
	colorFrequency := make(map[types.RGBQuad]int)
	for i := 0; i < int(image.FileInfo.Height); i++ {
		for j := 0; j < int(image.FileInfo.Width); j++ {
			colorFrequency[bmp.GetPixelColor(i, j, image)]++
		}
	}

	keys := make([]types.RGBQuad, 0, len(colorFrequency))
	for key := range colorFrequency {
		keys = append(keys, key)
	}

	sort.SliceStable(keys, func(i, j int) bool {
		return colorFrequency[keys[i]] > colorFrequency[keys[j]]
	})

	return keys
}

type ColorBounds struct {
	Red   types.Bounds
	Green types.Bounds
	Blue  types.Bounds
}

func splitCube(bucket []types.RGBQuad) ([]types.RGBQuad, []types.RGBQuad) {
	bounds := ColorBounds{types.Bounds{Begin: 255, End: 0}, types.Bounds{Begin: 255, End: 0}, types.Bounds{Begin: 255, End: 0}}

	for _, color := range bucket {
		if int(color.RGBRed) > bounds.Red.End {
			bounds.Red.End = int(color.RGBRed)
		}
		if int(color.RGBRed) < bounds.Red.Begin {
			bounds.Red.Begin = int(color.RGBRed)
		}

		if int(color.RGBGreen) > bounds.Green.End {
			bounds.Green.End = int(color.RGBGreen)
		}
		if int(color.RGBGreen) < bounds.Green.Begin {
			bounds.Green.Begin = int(color.RGBGreen)
		}

		if int(color.RGBBlue) > bounds.Blue.End {
			bounds.Blue.End = int(color.RGBBlue)
		}
		if int(color.RGBBlue) < bounds.Blue.Begin {
			bounds.Blue.Begin = int(color.RGBBlue)
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
			return bucket[i].RGBRed > bucket[j].RGBRed
		})
	case "green":
		sort.Slice(bucket, func(i, j int) bool {
			return bucket[i].RGBGreen > bucket[j].RGBGreen
		})
	case "blue":
		sort.Slice(bucket, func(i, j int) bool {
			return bucket[i].RGBBlue > bucket[j].RGBBlue
		})
	}

	return bucket[:len(bucket)/2], bucket[len(bucket)/2:]
}

func medianCut(rgbQuad []types.RGBQuad, colors int) ([]types.RGBQuad, map[types.RGBQuad]int) {
	cubes := [][]types.RGBQuad{rgbQuad}
	for len(cubes) < colors {
		newCubes := [][]types.RGBQuad{}
		for _, bucket := range cubes {
			partA, partB := splitCube(bucket)
			newCubes = append(newCubes, partA)
			newCubes = append(newCubes, partB)
		}
		cubes = newCubes
	}

	palette := make([]types.RGBQuad, colors)
	colorMap := make(map[types.RGBQuad]int, 0)

	for i, bucket := range cubes {
		var avgRed float64 = 0
		var avgGreen float64 = 0
		var avgBlue float64 = 0

		for _, pixel := range bucket {
			avgRed += float64(pixel.RGBRed) / float64(len(bucket))
			avgGreen += float64(pixel.RGBGreen) / float64(len(bucket))
			avgBlue += float64(pixel.RGBBlue) / float64(len(bucket))
			colorMap[pixel] = i
		}
		palette[i] = types.RGBQuad{RGBRed: byte(avgRed), RGBGreen: byte(avgGreen), RGBBlue: byte(avgBlue), RGBReserved: 0xff}
	}

	return palette, colorMap
}

func convertTrueColorBMPToPaletteBMP(image bmp.BMPImage, bitCount int) bmp.BMPImage {
	var newBitCount uint16 = uint16(bitCount)
	var colorsCount uint32 = uint32(math.Pow(2, float64(newBitCount)))

	var newImage bmp.BMPImage

	newImage.FileHeader = image.FileHeader
	newImage.FileInfo = image.FileInfo
	newImage.Meta = bmp.NewMeta(newBitCount, image.FileInfo.Width)

	newImage.FileInfo.BitCount = newBitCount
	newImage.FileInfo.SizeImage = uint32(newImage.Meta.WidthAligned) * uint32(newImage.FileInfo.Height) / uint32(newImage.Meta.BitsPerPixel)
	newImage.FileInfo.ColorUsed = colorsCount
	newImage.FileInfo.ColorImportant = colorsCount

	newImage.FileHeader.Offset = uint32(bmp.BitmapFileInfoBounds.End) + colorsCount*uint32(types.RGBQuadElementsCount)
	newImage.FileHeader.Size = newImage.FileInfo.SizeImage + newImage.FileHeader.Offset

	frequentColors := getFrequentColorKeys(image)
	newPalette, colorMap := medianCut(frequentColors, int(colorsCount))

	newImage.RGBQuad = newPalette

	newImage.ColorIndexArray = make([]byte, newImage.FileInfo.SizeImage)
	for i := 0; i < int(newImage.FileInfo.Height); i++ {
		for j := 0; j < int(newImage.FileInfo.Width); j++ {
			color := bmp.GetPixelColor(i, j, image)
			bmp.SetPixelColorIndex(i, j, uint8(colorMap[color]), newImage)
		}
	}

	return newImage
}

func main() {
	inputFilename, err := filepath.Abs("../CAT256.bmp")
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(inputFilename) + "_To_16C.bmp"

	image := bmp.FromBytes(file.Read(inputFilename))

	fmt.Println("Original image: ")
	helpers.PrintBMPStructure(image)

	convertedImage := convertTrueColorBMPToPaletteBMP(image, 4)

	fmt.Println("Converted 256 colors to 16 colors image: ")
	helpers.PrintBMPStructure(convertedImage)

	file.Write(outputFilename, bmp.ToBytes(convertedImage))
}
