package main

import (
	"path/filepath"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
)

func BlendPixelColor(imageColorComponent byte, logoColorComponent, opacity float64) byte {
	return byte(float64(imageColorComponent)*opacity + float64(logoColorComponent)*(1-opacity))
}

func AddLogoToBmp(logo bmp.BMPImage, opacity float64, x, y int, image bmp.BMPImage) bmp.BMPImage {
	newImage := bmp.GetCopy(image)

	backgroundColor := bmp.RgbQuad{RgbBlue: 0xff, RgbGreen: 0xff, RgbRed: 0xff, RgbReserved: 0}

	for i := 0; i < int(logo.FileInfo.Height); i++ {
		for j := 0; j < int(logo.FileInfo.Width); j++ {
			logoPixel := bmp.GetPixelColor(i, j, logo)
			if logoPixel == backgroundColor {
				continue
			}

			imagePixel := bmp.GetPixelColor(x+i, y+j, newImage)

			newColor := bmp.RgbQuad{}
			newColor.RgbRed = BlendPixelColor(imagePixel.RgbRed, float64(logoPixel.RgbRed), opacity)
			newColor.RgbGreen = BlendPixelColor(imagePixel.RgbGreen, float64(logoPixel.RgbGreen), opacity)
			newColor.RgbBlue = BlendPixelColor(imagePixel.RgbBlue, float64(logoPixel.RgbBlue), opacity)

			bmp.SetPixelColor(x+i, y+j, newColor, newImage)
		}
	}

	return newImage
}

func main() {
	imageFilename, err := filepath.Abs("../_carib_TC.bmp")
	if err != nil {
		panic(err)
	}

	logoFilename, err := filepath.Abs("logo.bmp")
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(imageFilename) + "_With_Logo.bmp"

	image := bmp.FromBytes(file.Read(imageFilename))
	helpers.PrintBmpStructure(image)

	logo := bmp.FromBytes(file.Read(logoFilename))

	x := 200
	y := 200
	opacity := 0.5

	convertedImage := AddLogoToBmp(logo, opacity, x, y, image)

	file.Write(outputFilename, bmp.ToBytes(convertedImage))
}
