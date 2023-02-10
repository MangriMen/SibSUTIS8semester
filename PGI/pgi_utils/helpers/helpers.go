package helpers

import (
	"encoding/json"
	"fmt"

	bmp "example.com/images/bitmap"
	"example.com/images/pcx"
)

func PrintBMPStructure(image bmp.BMPImage) {
	prefix := ""
	indent := "  "

	header, _ := json.MarshalIndent(image.FileHeader, prefix, indent)
	info, _ := json.MarshalIndent(image.FileInfo, prefix, indent)
	rgbQuad, _ := json.MarshalIndent(image.RGBQuad, prefix, indent)

	fmt.Printf("File header: %s\n", header)
	fmt.Printf("File info: %s\n", info)
	fmt.Printf("Palette: %s\n", rgbQuad)
}

func PrintPCXStructure(image pcx.PCXImage) {
	prefix := ""
	indent := "  "

	header, _ := json.MarshalIndent(image.FileHeader, prefix, indent)
	fmt.Printf("%s", header)
}
