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

func main() {
	filename, err := filepath.Abs("../CAT16.pcx")
	if err != nil {
		panic(err)
	}

	image := PcxFromBytes(readFileAsBytes(filename))

	header, _ := json.MarshalIndent(image.FileHeader, "", "  ")
	fmt.Printf("%s", header)

	width := int(image.FileHeader.XMax)
	height := int(image.FileHeader.YMax)

	a := app.New()
	w := a.NewWindow("lab 4")
	cnv := w.Canvas()

	raster := canvas.NewRasterWithPixels(
		func(x, y, w, h int) color.Color {
			if x < width && y < height {
				pixelColor := PcxGetPixelColor(x, y, image)

				return color.RGBA{pixelColor.RgbRed, pixelColor.RgbGreen, pixelColor.RgbBlue, 0xff}
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
