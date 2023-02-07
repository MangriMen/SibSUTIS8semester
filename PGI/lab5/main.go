package main

import (
	"encoding/json"
	"fmt"
	"image/color"
	"os"
	"path/filepath"
	"time"

	"fyne.io/fyne/v2"
	"fyne.io/fyne/v2/app"
	"fyne.io/fyne/v2/canvas"
)

func readFileAsBytes(path string) []byte {
	image, err := os.ReadFile(path)

	if err != nil {
		panic(err)
	}

	return image
}

func printBMPStructure(image BMPImage) {
	prefix := ""
	indent := "  "

	header, _ := json.MarshalIndent(image.FileHeader, prefix, indent)
	info, _ := json.MarshalIndent(image.FileInfo, prefix, indent)
	rgbQuad, _ := json.MarshalIndent(image.RgbQuad, prefix, indent)

	fmt.Printf("File header: %s\n", header)
	fmt.Printf("File info: %s\n", info)
	fmt.Printf("Palette: %s\n", rgbQuad)
}

func main() {
	filename, err := filepath.Abs("../_carib_TC.bmp")
	if err != nil {
		panic(err)
	}

	image := bmpFromBytes(readFileAsBytes(filename))
	printBMPStructure(image)

	width := int(image.FileInfo.Width)
	height := int(image.FileInfo.Height)

	a := app.New()
	w := a.NewWindow("lab 4")
	cnv := w.Canvas()

	var scaleCoeff float32 = 1

	scaledHeight := int(float32(height) * scaleCoeff)
	scaledWidth := int(float32(width) * scaleCoeff)

	fmt.Printf("Scale: %0.2f\n", scaleCoeff)
	fmt.Printf("Resolution: %dx%d\n", scaledWidth, scaledHeight)

	raster := canvas.NewRasterWithPixels(
		func(x, y, w, h int) color.Color {
			if x < scaledWidth && y < scaledHeight {
				pixelColor := GetPixelColor(int(float32(y)/scaleCoeff), int(float32(x)/scaleCoeff), image)

				var alpha byte = 0xff
				if image.FileInfo.BitCount == 32 {
					alpha = pixelColor.RgbReserved
				}

				return color.RGBA{pixelColor.RgbRed, pixelColor.RgbGreen, pixelColor.RgbBlue, alpha}
			}

			return color.RGBA{}
		})

	cnv.SetContent(raster)

	go func() {
		time.Sleep(time.Second)
	}()

	w.Resize(fyne.NewSize(float32(width+12), float32(height+12)))
	w.ShowAndRun()
}
