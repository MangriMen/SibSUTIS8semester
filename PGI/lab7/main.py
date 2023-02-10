
filler = "Я Валера хроносфера"
fillerSize = len(filler)

fileSize = 1_440_000

sizes = [0.1, 0.2, 0.5]

for size in sizes:
    with open(f"{size}.txt", 'w') as file:
        needSize = fileSize * size
        fillerCount = int(needSize / fillerSize)
        remainder = int(needSize % fillerSize)
        file.write(filler * fillerCount)
        file.write("a" * remainder)