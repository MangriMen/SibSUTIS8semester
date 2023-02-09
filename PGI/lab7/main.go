package main

import (
	"math"
	"path/filepath"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/binary"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
)

func InjectTextInBmp(image bmp.BMPImage, text []byte) bmp.BMPImage {
	newImage := bmp.GetCopy(image)

	bitsCountForTextData := int(math.Round(float64(len(text)) / float64(newImage.FileInfo.SizeImage) * float64(binary.BitsPerByte)))
	textBitLength := len(text) * binary.BitsPerByte

	for i, textBit := 0, 0; textBit < textBitLength && i < len(newImage.ColorIndexArray); i++ {
		for j := 0; j < bitsCountForTextData; j++ {
			newImage.ColorIndexArray[i] = byte(binary.ClearBit(int(newImage.ColorIndexArray[i]), uint(j)))

			if binary.HasBit(int(text[textBit/binary.BitsPerByte]), uint(textBit%binary.BitsPerByte)) {
				newImage.ColorIndexArray[i] = byte(binary.SetBit(int(newImage.ColorIndexArray[i]), uint(j)))
			}

			textBit++
		}
	}

	return newImage
}

func GetTextFromBmp(image bmp.BMPImage, textFileSize int) []byte {
	text := make([]byte, textFileSize/8)

	bitsCountForTextData := int(math.Round(float64(len(text)) / float64(image.FileInfo.SizeImage) * float64(binary.BitsPerByte)))

	for i, textBit := 0, 0; textBit < textFileSize && i < len(image.ColorIndexArray); i++ {
		for j := 0; j < bitsCountForTextData; j++ {
			if binary.HasBit(int(image.ColorIndexArray[i]), uint(j)) {
				text[textBit/binary.BitsPerByte] = byte(binary.SetBit(int(text[textBit/binary.BitsPerByte]), uint(textBit%binary.BitsPerByte)))
			}

			textBit++
		}
	}

	return text
}

func main() {
	inputFilename, err := filepath.Abs("../_carib_TC.bmp")
	if err != nil {
		panic(err)
	}

	filenameWithoutExt := file.GetFilenameWithoutExt(inputFilename)

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBmpStructure(image)

	sizes := []string{"0.1.txt", "0.2.txt", "0.5.txt"}

	for _, size := range sizes {
		textFilename, err := filepath.Abs(size)
		if err != nil {
			panic(err)
		}

		txtFile := file.Read(textFilename)
		convertedImage := InjectTextInBmp(image, txtFile)

		outputFilename := filenameWithoutExt + "_" + size + "_Stenography.bmp"
		file.Write(outputFilename, bmp.ToBytes(convertedImage))

		textFromBmp := GetTextFromBmp(convertedImage, len(txtFile)*8)
		file.Write(size+"_from_bmp.txt", textFromBmp)
	}
}
