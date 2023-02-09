package types

type Bounds struct {
	Begin, End int
}

const RGBQuadElementsCount int = 4

type RgbQuad struct {
	RgbBlue     byte
	RgbGreen    byte
	RgbRed      byte
	RgbReserved byte
}

func NewRgbQuad(data []byte) RgbQuad {
	rgbQuad := RgbQuad{RgbBlue: data[0], RgbGreen: data[1], RgbRed: data[2], RgbReserved: data[3]}
	return rgbQuad
}

func RgbQuadToBytes(rgbQuad RgbQuad) []byte {
	bytes := make([]byte, 4)
	bytes[0] = rgbQuad.RgbBlue
	bytes[1] = rgbQuad.RgbGreen
	bytes[2] = rgbQuad.RgbRed
	bytes[3] = rgbQuad.RgbReserved
	return bytes
}
