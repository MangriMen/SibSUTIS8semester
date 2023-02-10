package main

import (
	"os"
	"path/filepath"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
	"example.com/pgi_utils/types"
)

func ConvertToAverageColor(color types.RGBQuad) types.RGBQuad {
	averageValue := byte((int(color.RGBBlue) + int(color.RGBGreen) + int(color.RGBRed)) / 3)
	return types.RGBQuad{RGBBlue: averageValue, RGBGreen: averageValue, RGBRed: averageValue, RGBReserved: color.RGBReserved}
}

func ConvertBmpToBnW(image bmp.BMPImage) bmp.BMPImage {
	bnwImage := bmp.GetCopy(image)
	for i, color := range bnwImage.RGBQuad {
		bnwImage.RGBQuad[i] = ConvertToAverageColor(color)
	}
	return bnwImage
}

func main() {
	filename := "../CAT16.bmp"
	if len(os.Args) > 1 {
		filename = os.Args[1]
	}

	inputFilename, err := filepath.Abs(filename)
	if err != nil {
		panic(err)
	}

	outputFilename := file.GetFilenameWithoutExt(inputFilename) + "_BnW.bmp"

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBMPStructure(image)

	bnwImage := ConvertBmpToBnW(image)

	file.Write(outputFilename, bmp.ToBytes(bnwImage))
}
