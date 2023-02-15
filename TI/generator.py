import random


def getSymbol(symbols: list[str],
              probabilities: list[float] | None = None) -> str:
    if probabilities is None:
        return random.choice(symbols)
    return random.choices(symbols, probabilities)[0]


def generateFile(file_size: int,
                 path: str,
                 symbols: list[str],
                 probabilities: list[float] | None = None):
    with open(path, 'w+') as file:
        if probabilities is None:
            for _ in range(0, file_size):
                file.write(getSymbol(symbols))
        else:
            assert len(probabilities) == len(
                symbols) and abs(sum(probabilities) - 1.0) < 0.001
            for _ in range(0, file_size):
                file.write(getSymbol(symbols, probabilities))