from __future__ import annotations
from dataclasses import dataclass
import math
import pprint
import struct
from bidict import bidict


@dataclass
class Node():
    data: str
    freq: float

    left: Node | None = None
    right: Node | None = None

    def __init__(self, data: str, freq: float) -> None:
        self.data = data
        self.freq = freq


class Huffman:

    @staticmethod
    def GenerateTree(alphabet: list[Node]) -> Node:
        while len(alphabet) > 1:
            node_first, node_second = alphabet[:2]

            merge_node = Node(node_first.data + node_second.data,
                              node_first.freq + node_second.freq)

            merge_node.left = node_first
            merge_node.right = node_second

            alphabet.remove(node_first)
            alphabet.remove(node_second)

            alphabet.append(merge_node)

            alphabet = sorted(alphabet, key=lambda x: (x.freq, x.data))

        return alphabet[0]

    @staticmethod
    def GenerateKeyCodes(node: Node, keywords: bidict, keyword: str):
        if node.left is not None and node.right is not None:
            Huffman.GenerateKeyCodes(node.left, keywords, f"{keyword}0")
            Huffman.GenerateKeyCodes(node.right, keywords, f"{keyword}1")
        else:
            keywords[node.data] = keyword

    @staticmethod
    def GenerateTable(probabilities: tuple[list[str], list[float]]):
        alphabet_tree = sorted([
            Node(symbol, freq)
            for symbol, freq in zip(probabilities[0], probabilities[1])
        ],
                               key=lambda x: x.freq)

        tree = Huffman.GenerateTree(alphabet_tree)

        Huffman.GenerateKeyCodes(tree, keywords := bidict(), "")

        return keywords


def decToBinConversion(no, precision):
    binary = ""
    IntegralPart = int(no)
    fractionalPart = no - IntegralPart
    #to convert an integral part to binary equivalent
    while (IntegralPart):
        re = IntegralPart % 2
        binary += str(re)
        IntegralPart //= 2
    binary = binary[::-1]
    binary += '.'
    #to convert an fractional part to binary equivalent
    while (precision):
        fractionalPart *= 2
        bit = int(fractionalPart)
        if (bit == 1):
            fractionalPart -= bit
            binary += '1'
        else:
            binary += '0'
        precision -= 1
    return binary


class Shannon:

    @staticmethod
    def GenerateTable(probabilities: tuple[list[str], list[float]]):
        alphabet = {
            key: probability
            for key, probability in zip(probabilities[0], probabilities[1])
        }

        alphabet_sorted = sorted(alphabet.items(),
                                 key=lambda x: x[1],
                                 reverse=True)

        b = [x[1] for x in alphabet_sorted]
        b.insert(0, 0)

        new_alphabet = {
            key: sum(b[:i + 1])
            for i, (key, _) in enumerate(alphabet_sorted)
        }

        alphabet_sorted = sorted(
            new_alphabet.items(),
            key=lambda x: x[1],
        )

        keywords = {}
        for (key, code) in alphabet_sorted:
            bin_fraction = decToBinConversion(code, 18).split(".")[1]
            l_num = math.ceil(-math.log2(alphabet[key]))
            keywords[key] = bin_fraction[:l_num]

        return bidict(keywords)