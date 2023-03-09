package binary

const BitsPerByte = 8

func SetBit(n int, pos uint) int {
	n |= (1 << pos)
	return n
}

func ClearBit(n int, pos uint) int {
	mask := ^(1 << pos)
	n &= mask
	return n
}

func HasBit(n int, pos uint) bool {
	val := n & (1 << pos)
	return (val > 0)
}

func BytesToUInt32(b []byte) uint32 {
	return uint32(b[0]) | uint32(b[1])<<8 | uint32(b[2])<<16 | uint32(b[3])<<24
}

func BytesToUInt16(b []byte) uint16 {
	return uint16(b[0]) | uint16(b[1])<<8
}

func UInt32ToBytes(n uint32) []byte {
	bytes := make([]byte, 4)
	bytes[0] = byte(n)
	bytes[1] = byte(n >> 8)
	bytes[2] = byte(n >> 16)
	bytes[3] = byte(n >> 24)
	return bytes
}

func UInt16ToBytes(n uint16) []byte {
	bytes := make([]byte, 2)
	bytes[0] = byte(n)
	bytes[1] = byte(n >> 8)
	return bytes
}
