package main

import (
	"image/color"
	"os"
	"path/filepath"
	"time"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
	"fyne.io/fyne/v2"
	"fyne.io/fyne/v2/app"
	"fyne.io/fyne/v2/canvas"
)

func main() {
	filename := "../CAT16.bmp"

	if len(os.Args) > 1 {
		filename = os.Args[1]
	}

	inputFilename, err := filepath.Abs(filename)
	if err != nil {
		panic(err)
	}

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBMPStructure(image)

	width := int(image.FileInfo.Width)
	height := int(image.FileInfo.Height)

	a := app.New()
	w := a.NewWindow("lab 4")
	cnv := w.Canvas()

	raster := canvas.NewRasterWithPixels(
		func(x, y, w, h int) color.Color {
			if x < width && y < height {
				pixelColor := bmp.GetPixelColor(y, x, image)

				var alpha byte = 0xff
				if image.FileInfo.BitCount == 32 {
					alpha = pixelColor.RGBReserved
				}

				return color.RGBA{pixelColor.RGBRed, pixelColor.RGBGreen, pixelColor.RGBBlue, alpha}
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
