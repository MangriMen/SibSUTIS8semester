package main

import (
	"fmt"
	"image/color"
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
	inputFilename, err := filepath.Abs("../_carib_TC.bmp")
	if err != nil {
		panic(err)
	}

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBmpStructure(image)

	width := int(image.FileInfo.Width)
	height := int(image.FileInfo.Height)

	var scaleCoeff float32 = 1

	scaledHeight := int(float32(height) * scaleCoeff)
	scaledWidth := int(float32(width) * scaleCoeff)

	fmt.Printf("Scale: %0.2f\n", scaleCoeff)
	fmt.Printf("Resolution: %dx%d\n", scaledWidth, scaledHeight)

	a := app.New()
	w := a.NewWindow("lab 4")
	cnv := w.Canvas()

	raster := canvas.NewRasterWithPixels(
		func(x, y, w, h int) color.Color {
			if x < scaledWidth && y < scaledHeight {
				pixelColor := bmp.GetPixelColor(int(float32(y)/scaleCoeff), int(float32(x)/scaleCoeff), image)

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
