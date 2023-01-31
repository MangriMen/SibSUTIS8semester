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

func loadBMP() {
	filename, err := filepath.Abs("../CAT16.bmp")
	if err != nil {
		panic(err)
	}

	image := readBMP(filename)
	printBMPStructure(image)
}

func main() {
	a := app.New()
	w := a.NewWindow("lab 4")
	cnv := w.Canvas()

	raster := canvas.NewRasterWithPixels(
		func(x, y, w, h int) color.Color {
			if x < 300 {
				return color.RGBA{255, 255, 255, 0xff}
			}

			return color.RGBA{uint8(rand.Intn(255)), uint8(rand.Intn(255)), uint8(rand.Intn(255)), 0xff}
		})

	cnv.SetContent(raster)

	go func() {
		time.Sleep(time.Second)
	}()

	w.Resize(fyne.NewSize(800, 600))
	w.ShowAndRun()
}
