package file

import (
	"os"
	"path/filepath"
	"strings"
)

func Read(path string) []byte {
	data, err := os.ReadFile(path)

	if err != nil {
		panic(err)
	}

	return data
}

func Write(path string, data []byte) {
	var err error

	err = os.RemoveAll(path)

	if err != nil {
		panic(err)
	}

	err = os.WriteFile(path, data, 0644)

	if err != nil {
		panic(err)
	}
}

func GetFilenameWithoutExt(path string) string {
	return strings.Split(filepath.Base(path), ".")[0]
}
