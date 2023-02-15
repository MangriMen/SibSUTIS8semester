import csv
import os

import generator
import formulas


def main():
    os.chdir('lab1')

    symbols = ['a', 'b', 'c', 'd']
    probabilities = [0.2, 0.4, 0.3, 0.1]

    generator.generateFile(10 * 1024, 'uniform.txt', symbols)
    generator.generateFile(10 * 1024, 'probability.txt', symbols,
                           probabilities)

    symbol_count = 2

    probabilities_multiple_characters = formulas.calculateMultipleCharacterProbabilities(
        probabilities, symbol_count)

    probabilities_uniform = [1 / len(symbols) for _ in symbols]
    probabilities_multiple_characters_uniform = formulas.calculateMultipleCharacterProbabilities(
        probabilities_uniform, symbol_count)

    files = ['uniform.txt', 'probability.txt']
    probabilities_for_files = [[
        probabilities_uniform, probabilities_multiple_characters_uniform
    ], [probabilities, probabilities_multiple_characters]]
    symbol_count = 2

    csv_header = [
        "Название файла",
        "Эксперементальная оценка энтропии (частоты отдельных символов)",
        "Теоретическое значение энтропии (отдельные символы)",
        "Эксперементальная оценка энтропии (частоты пар символов)",
        "Теоретическое значение энтропии (для пар символов)"
    ]

    rows: list[list[str]] = []

    for filename, probabilities_for_file in zip(files,
                                                probabilities_for_files):
        theoretical_entropy = formulas.calculateEntropy(
            probabilities_for_file[0])
        theoretical_entropy_multiple_characters = formulas.calculateEntropy(
            probabilities_for_file[1]) / symbol_count

        entropy = formulas.calculateEntropyForFile(filename)
        entropy_multiple_characters = formulas.calculateEntropyForFile(
            filename, symbol_count)

        rows.append([
            filename,
            str(entropy),
            str(theoretical_entropy),
            str(entropy_multiple_characters),
            str(theoretical_entropy_multiple_characters)
        ])

    with open('result.csv', 'w+', newline='') as csv_file:
        csv_writer = csv.writer(csv_file, delimiter=';')
        csv_writer.writerow(csv_header)
        csv_writer.writerows(rows)


if __name__ == "__main__":
    main()