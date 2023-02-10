package main

import (
	"os"
	"path/filepath"
	"strconv"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
	"example.com/pgi_utils/types"
)

func BlendPixelColor(imageColorComponent byte, logoColorComponent, opacity float64) byte {
	return byte(float64(imageColorComponent)*opacity + float64(logoColorComponent)*(1-opacity))
}

func AddLogoToBmp(logo bmp.BMPImage, opacity float64, x, y int, image bmp.BMPImage) bmp.BMPImage {
	newImage := bmp.GetCopy(image)

	backgroundColor := types.RGBQuad{RGBBlue: 0xff, RGBGreen: 0xff, RGBRed: 0xff, RGBReserved: 0}

	for i := 0; i < int(logo.FileInfo.Height); i++ {
		for j := 0; j < int(logo.FileInfo.Width); j++ {
			logoPixel := bmp.GetPixelColor(i, j, logo)
			if logoPixel == backgroundColor {
				continue
			}

			imagePixel := bmp.GetPixelColor(x+i, y+j, newImage)

			newColor := types.RGBQuad{}
			newColor.RGBRed = BlendPixelColor(imagePixel.RGBRed, float64(logoPixel.RGBRed), opacity)
			newColor.RGBGreen = BlendPixelColor(imagePixel.RGBGreen, float64(logoPixel.RGBGreen), opacity)
			newColor.RGBBlue = BlendPixelColor(imagePixel.RGBBlue, float64(logoPixel.RGBBlue), opacity)

			bmp.SetPixelColor(x+i, y+j, newColor, newImage)
		}
	}

	return newImage
}

func main() {
	filename := "../_carib_TC.bmp"
	x := 200
	y := 200
	opacity := 0.5

	if len(os.Args) > 4 {
		filename = os.Args[1]

		xPos, err := strconv.Atoi(os.Args[2])
		if err != nil {
			panic(err)
		}

		yPos, err := strconv.Atoi(os.Args[3])
		if err != nil {
			panic(err)
		}

		opac, err := strconv.ParseFloat(os.Args[4], 32)
		if err != nil {
			panic(err)
		}

		x = int(xPos)
		y = int(yPos)
		opacity = opac
	}

	imageFilename, err := filepath.Abs(filename)
	if err != nil {
		panic(err)
	}

	logoFilename, err := filepath.Abs("logo.bmp")
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(imageFilename) + "_With_Logo.bmp"

	image := bmp.FromBytes(file.Read(imageFilename))
	helpers.PrintBMPStructure(image)

	logo := bmp.FromBytes(file.Read(logoFilename))

	convertedImage := AddLogoToBmp(logo, opacity, x, y, image)

	file.Write(outputFilename, bmp.ToBytes(convertedImage))
}
