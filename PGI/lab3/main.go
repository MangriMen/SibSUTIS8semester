package main

import (
	"os"
	"path/filepath"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
	"example.com/pgi_utils/types"
)

type rotationDirection int

const (
	left  rotationDirection = iota
	right rotationDirection = iota
)

func RotateBmp(image bmp.BMPImage, rotationDirection rotationDirection) bmp.BMPImage {
	newImage := image

	newImage.FileInfo.Width, newImage.FileInfo.Height = newImage.FileInfo.Height, newImage.FileInfo.Width

	newImage.Meta = bmp.NewMeta(newImage.FileInfo.BitCount, newImage.FileInfo.Width)
	newImage.FileInfo.SizeImage = uint32(newImage.Meta.WidthAlignedBytes) * uint32(newImage.FileInfo.Height)

	newImage.RGBQuad = append([]types.RGBQuad(nil), image.RGBQuad...)
	newImage.ColorIndexArray = make([]byte, newImage.FileInfo.SizeImage)

	width := int(image.FileInfo.Width)
	height := int(image.FileInfo.Height)

	if rotationDirection == right {
		if newImage.FileInfo.BitCount <= 8 {
			for i := 0; i < height; i++ {
				for j := 0; j < width; j++ {
					bmp.SetPixelColorIndex(j, i, bmp.GetPixelColorIndex(height-i-1, j, image), newImage)
				}
			}
		} else {
			for i := 0; i < height; i++ {
				for j := 0; j < width; j++ {
					bmp.SetPixelColor(j, i, bmp.GetPixelColor(height-i-1, j, image), newImage)
				}
			}
		}
	} else {
		if newImage.FileInfo.BitCount <= 8 {
			for i := 0; i < height; i++ {
				for j := 0; j < width; j++ {
					bmp.SetPixelColorIndex(j, i, bmp.GetPixelColorIndex(i, width-j-1, image), newImage)
				}
			}
		} else {
			for i := 0; i < height; i++ {
				for j := 0; j < width; j++ {
					bmp.SetPixelColor(j, i, bmp.GetPixelColor(i, width-j-1, image), newImage)
				}
			}
		}
	}

	return newImage
}

func main() {
	filename := "../_carib_TC.bmp"
	direction := left

	if len(os.Args) > 2 {
		filename = os.Args[1]

		switch os.Args[2] {
		case "left":
			direction = left
		case "right":
			direction = right
		}
	}

	inputFilename, err := filepath.Abs(filename)
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(inputFilename) + "_Rotation.bmp"

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBMPStructure(image)

	rotatedImage := RotateBmp(image, direction)

	file.Write(outputFilename, bmp.ToBytes(rotatedImage))
}
