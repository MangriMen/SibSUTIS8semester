package main

import (
	"path/filepath"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
)

func ConvertToAverageColor(color bmp.RgbQuad) bmp.RgbQuad {
	averageValue := byte((int(color.RgbBlue) + int(color.RgbGreen) + int(color.RgbRed)) / 3)
	return bmp.RgbQuad{RgbBlue: averageValue, RgbGreen: averageValue, RgbRed: averageValue, RgbReserved: color.RgbReserved}
}

func ConvertBmpToBnW(image bmp.BMPImage) bmp.BMPImage {
	bnwImage := bmp.GetCopy(image)
	for i, color := range bnwImage.RgbQuad {
		bnwImage.RgbQuad[i] = ConvertToAverageColor(color)
	}
	return bnwImage
}

func main() {
	inputFilename, err := filepath.Abs("../CAT16.bmp")
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(inputFilename) + "_BnW.bmp"

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBmpStructure(image)

	bnwImage := ConvertBmpToBnW(image)

	file.Write(outputFilename, bmp.ToBytes(bnwImage))
}
