package main

import (
	"encoding/json"
	"fmt"
	"image/color"
	"math/rand"
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

func readBMP(path string) BMPImage {
	return bmpFromBytes(readFileAsBytes(path))
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
	a := app.New()
	w := a.NewWindow("lab 4")
	cnv := w.Canvas()

	filename, err := filepath.Abs("../CAT16.bmp")
	if err != nil {
		panic(err)
	}

	image := readBMP(filename)
	printBMPStructure(image)

	width := int(image.FileInfo.Width)
	height := int(image.FileInfo.Height)

	raster := canvas.NewRasterWithPixels(
		func(x, y, w, h int) color.Color {

			if x < height && y < width {
				colorIndex := GetPixel(x, y, image.ColorIndexArray, image.FileInfo)
				pixelColor := image.RgbQuad[colorIndex]

				return color.RGBA{pixelColor.RgbRed, pixelColor.RgbGreen, pixelColor.RgbBlue, 0xff}
			}

			return color.RGBA{uint8(rand.Intn(255)), uint8(rand.Intn(255)), uint8(rand.Intn(255)), 0xff}
		})

	cnv.SetContent(raster)

	go func() {
		time.Sleep(time.Second)
	}()

	w.Resize(fyne.NewSize(float32(height), float32(width)))
	w.ShowAndRun()
}
