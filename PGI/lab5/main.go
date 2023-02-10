package main

import (
	"fmt"
	"image/color"
	"os"
	"path/filepath"
	"strconv"
	"time"

	bmp "example.com/images/bitmap"
	"example.com/pgi_utils/file"
	"example.com/pgi_utils/helpers"
	"fyne.io/fyne/v2"
	"fyne.io/fyne/v2/app"
	"fyne.io/fyne/v2/canvas"
)

func main() {
	filename := "../_carib_TC.bmp"
	var scaleCoeff float32 = 0.5

	if len(os.Args) > 2 {
		filename = os.Args[1]

		number, err := strconv.ParseFloat(os.Args[2], 32)
		if err != nil {
			panic(err)
		}

		scaleCoeff = float32(number)
	}

	inputFilename, err := filepath.Abs(filename)
	if err != nil {
		panic(err)
	}

	image := bmp.FromBytes(file.Read(inputFilename))
	helpers.PrintBMPStructure(image)

	width := int(image.FileInfo.Width)
	height := int(image.FileInfo.Height)

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
