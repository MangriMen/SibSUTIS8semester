package main

import (
	"fmt"
	"math"
	"path/filepath"
	"sort"

	bmp "example.com/images/bitmap"
	"example.com/images/pcx"
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

func convertPixelsToPalette(colorsCount int, newImage bmp.BMPImage, sourceImage pcx.PCXImage, colorMap map[types.RGBTriple]int) {
	switch colorsCount {
	case 16:
		for i := 0; i < int(sourceImage.FileHeader.YMax); i++ {
			for j := 0; j < int(sourceImage.FileHeader.XMax); j++ {
				color := pcx.EGAToRGB(pcx.RGBtoEGA(pcx.GetPixelColor(i, j, sourceImage)))
				bmp.SetPixelColorIndex(i, j, uint8(colorMap[color]), newImage)
			}
		}
	case 256:
		for i := 0; i < int(sourceImage.FileHeader.YMax); i++ {
			for j := 0; j < int(sourceImage.FileHeader.XMax); j++ {
				color := pcx.GetPixelColor(i, j, sourceImage)
				bmp.SetPixelColorIndex(i, j, uint8(colorMap[color]), newImage)
			}
		}
	}
}

func convertTrueColorPCXToPaletteBMP(sourceImage pcx.PCXImage, bitCount int) bmp.BMPImage {
	colorsCount := int(math.Pow(2, float64(bitCount)))

	newImage := bmp.BMPImage{}

	newImage.FileInfo.Size = uint32(bmp.BitmapFileInfoBounds.End - bmp.BitmapFileHeaderBounds.End)
	newImage.FileInfo.Width = int32(sourceImage.FileHeader.XMax)
	newImage.FileInfo.Height = int32(sourceImage.FileHeader.YMax)
	newImage.FileInfo.Planes = 1
	newImage.FileInfo.BitCount = uint16(bitCount)
	newImage.FileInfo.Compression = 0
	newImage.FileInfo.HorizontalResolution = int32(sourceImage.FileHeader.HScreenSize) * int32(sourceImage.FileHeader.XMax)
	newImage.FileInfo.VerticalResolution = int32(sourceImage.FileHeader.VScreenSize) * int32(sourceImage.FileHeader.YMax)
	newImage.FileInfo.ColorUsed = uint32(colorsCount)
	newImage.FileInfo.ColorImportant = uint32(colorsCount)

	newImage.Meta = bmp.NewMeta(uint16(bitCount), newImage.FileInfo.Width)

	newImage.FileInfo.SizeImage = uint32(newImage.Meta.WidthAlignedBytes) * uint32(newImage.FileInfo.Height)

	newImage.FileHeader.Signature = uint16(bmp.BMPSignature)
	newImage.FileHeader.Offset = uint32(bmp.BitmapFileInfoBounds.End) + uint32(colorsCount)*uint32(types.RGBQuadElementsCount)
	newImage.FileHeader.Size = newImage.FileHeader.Offset + newImage.FileInfo.SizeImage

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

	newImage.RGBQuad = make([]types.RGBQuad, colorsCount)
	for i := 0; i < len(newPalette); i++ {
		newImage.RGBQuad[i] = types.RGBQuad{RGBRed: newPalette[i].RGBTRed, RGBGreen: newPalette[i].RGBTGreen, RGBBlue: newPalette[i].RGBTBlue, RGBReserved: 0xff}
	}

	newImage.ColorIndexArray = make([]byte, newImage.FileInfo.SizeImage)
	convertPixelsToPalette(colorsCount, newImage, sourceImage, colorMap)

	return newImage
}

func main() {
	inputFilename, err := filepath.Abs("../carib_TC.pcx")
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(inputFilename) + "_8bit.bmp"

	image := pcx.FromBytes(file.Read(inputFilename))

	fmt.Println("Original image: ")
	helpers.PrintPCXStructure(image)

	newImage := convertTrueColorPCXToPaletteBMP(image, 8)

	fmt.Println("New image: ")
	helpers.PrintBMPStructure(newImage)

	file.Write(outputFilename, bmp.ToBytes(newImage))
}
