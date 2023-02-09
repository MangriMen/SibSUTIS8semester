package main

import (
	"path/filepath"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
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

	newImage.RgbQuad = append([]bmp.RgbQuad(nil), image.RgbQuad...)
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
	inputFilename, err := filepath.Abs("../_carib_TC.bmp")
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(inputFilename) + "_Rotation.bmp"

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBmpStructure(image)

	rotatedImage := RotateBmp(image, left)

	file.Write(outputFilename, bmp.ToBytes(rotatedImage))
}
