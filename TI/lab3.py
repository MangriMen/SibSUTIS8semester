import csv
from typing import Callable
from bidict import bidict
import utils
import formulas
import coding


def encode(text: str, keywords: bidict):
    return ''.join([keywords[symbol] for symbol in text])


def decode(text: str, keyword: bidict):
    decoded = ""
    symbol_chain = ""
    for symbol in text:
        symbol_chain += symbol
        if symbol_chain in keyword.inverse:
            decoded += keyword.inverse[symbol_chain]
            symbol_chain = ""

    return decoded


def getMethod(text: str, probabilities: tuple[list[str], list[float]],
              table_method: Callable):
    method = {}
    method["table"] = table_method(probabilities)

    alphabet = {
        key: probability
        for key, probability in zip(probabilities[0], probabilities[1])
    }
    method["average_length"] = sum([
        alphabet[symbol] * len(keyword)
        for symbol, keyword in method["table"].items()
    ])

    encoded_huffman = encode(text, method["table"])
    method["entropy_list"] = [
        formulas.calculateEntropy(probabilities) / (i + 1)
        for i, probabilities in enumerate([
            formulas.getSymbolsAndProbabilities(encoded_huffman, symbol_count)
            [1] for symbol_count in range(1, 4)
        ])
    ]

    return method


def main():
    filename = 'lab2/text.txt'

    text = utils.normalize_text(filename)
    probabilities_list = [
        formulas.getSymbolsAndProbabilities(text, symbol_count)
        for symbol_count in range(1, 2)
    ]
    text_entropy = formulas.calculateEntropy(probabilities_list[0][1])

    codings = {
        "huffman":
        getMethod(text, probabilities_list[0], coding.Huffman.GenerateTable),
        "shannon":
        getMethod(text, probabilities_list[0], coding.Shannon.GenerateTable),
    }

    rows: list[list[str]] = []
    for coding_ in codings:
        rows.append([
            coding_, 'Киберзолушка',
            codings[coding_]["average_length"] - text_entropy
        ])
        for entropy in codings[coding_]["entropy_list"]:
            rows[-1].append(str(entropy))

    csv_header = [
        "Метод кодирования",
        "Название текста",
        "Оценка избыточности кодирования",
    ]
    for i, text_entropy in enumerate(codings["shannon"]["entropy_list"]):
        csv_header.append(f"Оценка энтропии H-{i+1}")

    with open('lab3/result.csv', 'w+', encoding='utf-8-sig',
              newline='') as csv_file:
        csv_writer = csv.writer(csv_file, delimiter=';')
        csv_writer.writerow(csv_header)
        csv_writer.writerows(rows)


if __name__ == "__main__":
    main()