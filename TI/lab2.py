import csv
import os

import generator
import formulas
import utils


def main():
    os.chdir('lab2')

    text = utils.normalize_text('text.txt')

    probabilities_list = [
        formulas.getSymbolsAndProbabilities(text, symbol_count)[1]
        for symbol_count in range(1, 6)
    ]

    entropy_list = [
        formulas.calculateEntropy(probabilities) / (i + 1)
        for i, probabilities in enumerate(probabilities_list)
    ]

    alphabet_size = 32

    rows: list[list[str]] = []
    rows.append([
        'КиберЗолушка',
        str(alphabet_size),
        str(
            formulas.calculateEntropy(
                [1 / alphabet_size for _ in range(0, alphabet_size)])),
    ])

    for entropy in entropy_list:
        rows[0].append(str(entropy))

    csv_header = [
        "Название файла",
        "Размер алфавита",
        "Максимально возможное значение энтропии",
    ]

    for i, entropy in enumerate(entropy_list):
        csv_header.append(f"Оценка энтропии H-{i+1}")

    with open('result.csv', 'w+', newline='') as csv_file:
        csv_writer = csv.writer(csv_file, delimiter=';')
        csv_writer.writerow(csv_header)
        csv_writer.writerows(rows)


if __name__ == "__main__":
    main()