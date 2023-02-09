module rgr_ugi_freestyler

go 1.20

require example.com/images v0.0.0

replace example.com/images v0.0.0 => ../images

replace example.com/pgi_utils v0.0.0 => ../pgi_utils

require golang.org/x/exp v0.0.0-20230206171751-46f607a40771 // indirect
