package types

type Bounds struct {
	Begin, End int
}

const RGBQuadElementsCount int = 4

type RGBQuad struct {
	RGBBlue     byte
	RGBGreen    byte
	RGBRed      byte
	RGBReserved byte
}

func NewRGBQuad(data []byte) RGBQuad {
	return RGBQuad{RGBBlue: data[0], RGBGreen: data[1], RGBRed: data[2], RGBReserved: data[3]}
}

func RGBQuadToBytes(rgbQuad RGBQuad) []byte {
	bytes := make([]byte, 4)
	bytes[0] = rgbQuad.RGBBlue
	bytes[1] = rgbQuad.RGBGreen
	bytes[2] = rgbQuad.RGBRed
	bytes[3] = rgbQuad.RGBReserved
	return bytes
}

const RGBTripleELementsCount = 3

type RGBTriple struct {
	RGBTBlue  uint8
	RGBTGreen uint8
	RGBTRed   uint8
}

func NewRGBTriple(data []byte) RGBTriple {
	return RGBTriple{RGBTRed: data[0], RGBTGreen: data[1], RGBTBlue: data[2]}
}

func RGBTripleToBytes(rgbTriple RGBTriple) []byte {
	bytes := make([]byte, 3)
	bytes[0] = rgbTriple.RGBTBlue
	bytes[1] = rgbTriple.RGBTGreen
	bytes[2] = rgbTriple.RGBTRed
	return bytes
}
