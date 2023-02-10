package main

import (
	"fmt"
	"math/rand"
	"os"
	"path/filepath"
	"strconv"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
	"example.com/pgi_utils/types"
)

func AddBorderToBmp(image bmp.BMPImage, borderWidth int, borderColorIndex uint8, borderColor types.RGBQuad) bmp.BMPImage {
	newImage := image

	var sides int = 2
	newImage.FileInfo.Width += int32(borderWidth * sides)
	newImage.FileInfo.Height += int32(borderWidth * sides)

	newImage.Meta = bmp.NewMeta(newImage.FileInfo.BitCount, newImage.FileInfo.Width)
	newImage.FileInfo.SizeImage = uint32(newImage.Meta.WidthAlignedBytes) * uint32(newImage.FileInfo.Height)

	newImage.RGBQuad = append([]types.RGBQuad(nil), image.RGBQuad...)
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
	filename := "../_carib_TC.bmp"
	borderWidth := 15

	if len(os.Args) > 2 {
		filename = os.Args[1]

		var atoiError error
		borderWidth, atoiError = strconv.Atoi(os.Args[2])

		if atoiError != nil {
			panic(atoiError)
		}
	}

	inputFilename, err := filepath.Abs(filename)
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(inputFilename) + "_Border.bmp"

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBMPStructure(image)

	fmt.Printf("Border width: %dpx\n", borderWidth)

	var borderColorIndex uint8
	var borderColor types.RGBQuad

	fmt.Print("Border color: ")
	if image.FileInfo.BitCount <= 8 {
		borderColorIndex = uint8(rand.Intn(len(image.RGBQuad)))
		fmt.Printf("%+v", image.RGBQuad[borderColorIndex])
	} else {
		borderColor = types.RGBQuad{RGBBlue: byte(rand.Intn(255)), RGBGreen: byte(rand.Intn(255)), RGBRed: byte(rand.Intn(255)), RGBReserved: 0xff}
		fmt.Printf("%+v", borderColor)
	}
	fmt.Println()

	imageWithBorder := AddBorderToBmp(image, borderWidth, borderColorIndex, borderColor)

	file.Write(outputFilename, bmp.ToBytes(imageWithBorder))
}
