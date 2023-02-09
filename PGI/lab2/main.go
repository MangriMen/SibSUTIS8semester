package main

import (
	"fmt"
	"math/rand"
	"path/filepath"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
)

func AddBorderToBmp(image bmp.BMPImage, borderWidth int, borderColorIndex uint8, borderColor bmp.RgbQuad) bmp.BMPImage {
	newImage := image

	var sides int = 2
	newImage.FileInfo.Width += int32(borderWidth * sides)
	newImage.FileInfo.Height += int32(borderWidth * sides)

	newImage.Meta = bmp.NewMeta(newImage.FileInfo.BitCount, newImage.FileInfo.Width)
	newImage.FileInfo.SizeImage = uint32(newImage.Meta.WidthAlignedBytes) * uint32(newImage.FileInfo.Height)

	newImage.RgbQuad = append([]bmp.RgbQuad(nil), image.RgbQuad...)
	newImage.ColorIndexArray = make([]byte, newImage.FileInfo.SizeImage)

	width := int(newImage.FileInfo.Width)
	height := int(newImage.FileInfo.Height)

	if image.FileInfo.BitCount <= 8 {
		for i := 0; i < height; i++ {
			for j := 0; j < width; j++ {
				if (i < borderWidth || i >= height-borderWidth) || (j < borderWidth || j >= width-borderWidth) {
					bmp.SetPixelColorIndex(i, j, borderColorIndex, newImage)
				} else {
					bmp.SetPixelColorIndex(i, j, bmp.GetPixelColorIndex(i-borderWidth, j-borderWidth, image), newImage)
				}
			}
		}
	} else {
		for i := 0; i < height; i++ {
			for j := 0; j < width; j++ {
				if (i < borderWidth || i >= height-borderWidth) || (j < borderWidth || j >= width-borderWidth) {
					bmp.SetPixelColor(i, j, borderColor, newImage)
				} else {
					bmp.SetPixelColor(i, j, bmp.GetPixelColor(i-borderWidth, j-borderWidth, image), newImage)
				}
			}
		}
	}

	return newImage
}

func main() {
	inputFilename, err := filepath.Abs("../_carib_TC.bmp")
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(inputFilename) + "_Border.bmp"

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBmpStructure(image)

	borderWidth := 15
	fmt.Printf("Border width: %dpx\n", borderWidth)

	var borderColorIndex uint8
	var borderColor bmp.RgbQuad

	fmt.Print("Border color: ")
	if image.FileInfo.BitCount <= 8 {
		borderColorIndex = uint8(rand.Intn(len(image.RgbQuad)))
		fmt.Printf("%+v", image.RgbQuad[borderColorIndex])
	} else {
		borderColor = bmp.RgbQuad{RgbBlue: byte(rand.Intn(255)), RgbGreen: byte(rand.Intn(255)), RgbRed: byte(rand.Intn(255)), RgbReserved: 0xff}
		fmt.Printf("%+v", borderColor)
	}
	fmt.Println()

	imageWithBorder := AddBorderToBmp(image, borderWidth, borderColorIndex, borderColor)

	file.Write(outputFilename, bmp.ToBytes(imageWithBorder))
}
