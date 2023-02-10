module lab3

go 1.20

require (
	example.com/images v0.0.0
	example.com/pgi_utils v0.0.0
)

replace example.com/images v0.0.0 => ../images

replace example.com/pgi_utils v0.0.0 => ../pgi_utils
