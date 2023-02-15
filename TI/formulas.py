import itertools
import math


def calculateJointProbabiliy(probA: float, probB: float):
    return probA + probB - (probA * probB)


def calculateMultipleCharacterProbabilities(probabilities: list[float],
                                            symbol_count: int):
    return list(
        map(math.prod,
            list(itertools.product(probabilities, repeat=symbol_count))))


def calculateEntropy(probabilities: list[float]):
    return abs(sum(map(lambda item: item * math.log2(item), probabilities)))


def getSymbolsAndProbabilities(data: str,
                               symbol_count: int = 1
                               ) -> tuple[list[str], list[float]]:
    symbolsAndProbabilities = dict[str, int]()

    for i in range(0, len(data) - symbol_count):
        substr = data[i:i + symbol_count]
        if substr not in symbolsAndProbabilities:
            symbolsAndProbabilities[substr] = 1
        else:
            symbolsAndProbabilities[substr] += 1

    symbolsAndProbabilities = dict[str, int](sorted(
        symbolsAndProbabilities.items()))

    symbols = list[str](symbolsAndProbabilities.keys())
    probabilities = list[float](map(
        lambda symbolCount: symbolCount / len(data),
        symbolsAndProbabilities.values()))

    return symbols, probabilities


def calculateEntropyForFile(path: str, symbol_count: int = 1):
    text = ""
    with open(path, 'r') as file:
        text = ''.join(file.readlines())
    _, probabilities = getSymbolsAndProbabilities(text, symbol_count)
    return calculateEntropy(probabilities) / symbol_count