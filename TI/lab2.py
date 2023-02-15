import csv
import os

import generator
import formulas
import utils


def main():
    os.chdir('lab2')

    symbol_count = 2

    text = utils.normalize_text('text.txt')

    _, probabilities = formulas.getSymbolsAndProbabilities(text)
    _, probabilities_multiple_characters = formulas.getSymbolsAndProbabilities(
        text, symbol_count)

    alphabet_size = 32

    entropy = formulas.calculateEntropy(probabilities)
    entropy_multiple_characters = formulas.calculateEntropy(
        probabilities_multiple_characters) / symbol_count

    rows: list[list[str]] = []
    rows.append([
        'Мартин Иден',
        str(alphabet_size),
        str(
            formulas.calculateEntropy(
                [1 / alphabet_size for _ in range(0, alphabet_size)])),
        str(entropy),
        str(entropy_multiple_characters),
    ])

    csv_header = [
        "Название файла", "Размер алфавита",
        "Максимально возможное значение энтропии",
        "Оценка энтропии (одиночные символы)",
        "Оценка энтропии (частоты пар символов)"
    ]

    with open('result.csv', 'w+', newline='') as csv_file:
        csv_writer = csv.writer(csv_file, delimiter=';')
        csv_writer.writerow(csv_header)
        csv_writer.writerows(rows)


if __name__ == "__main__":
    main()