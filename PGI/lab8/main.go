package main

import (
	"image/color"
	"path/filepath"
	"time"

	"example.com/images/pcx"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
	"fyne.io/fyne/v2"
	"fyne.io/fyne/v2/app"
	"fyne.io/fyne/v2/canvas"
)

func main() {
	inputFilename, err := filepath.Abs("../CAT16.pcx")
	if err != nil {
		panic(err)
	}

	image := pcx.FromBytes(file.Read(inputFilename))
	helpers.PrintPcxStructure(image)

	width := int(image.FileHeader.XMax)
	height := int(image.FileHeader.YMax)

	a := app.New()
	w := a.NewWindow("lab 4")
	cnv := w.Canvas()

	raster := canvas.NewRasterWithPixels(
		func(x, y, w, h int) color.Color {
			if x < width && y < height {
				pixelColor := pcx.GetPixelColor(x, y, image)

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
