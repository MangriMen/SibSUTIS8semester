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


def main():
    filename = 'lab2/text.txt'

    text = utils.normalize_text(filename)

    symbol_group_bound = (1, 6)
    probabilities_list = [
        formulas.getSymbolsAndProbabilities(text, symbol_count)
        for symbol_count in range(symbol_group_bound[0], symbol_group_bound[1])
    ]

    encoded_huffman = encode(
        text, coding.Huffman.GenerateTable(probabilities_list[0]))

    encoded_shannon = encode(
        text, coding.Shannon.GenerateTable(probabilities_list[0]))


if __name__ == "__main__":
    main()