#pragma once
#include <iostream>
#include <vector>

class Class1 {
public:
	enum class ShiftDirection {
		left,
		right
	};

	static int GetNumberRankCount(int number) {
		int rank_count = 0;
		while (number) {
			number /= 10;
			rank_count++;
		}
		return rank_count != 0 ? rank_count : 1;
	}

	static int ShiftLeft(int number, int shiftCount, int base = 10) {
		int rank_count = GetNumberRankCount(number);
		int position = static_cast<unsigned int>(std::pow(base, rank_count - 1));

		for (int i = 0;
			i < (shiftCount % rank_count);
			i++) {
			int tmp = number / position;
			number %= position;
			number = number * base + tmp;
		}
		return number;
	}

	static int ShiftRight(int number, int shiftCount, int base = 10) {
		int rank_count = GetNumberRankCount(number);
		int position = static_cast<unsigned int>(std::pow(base, rank_count - 1));

		for (int i = 0;
			i < (shiftCount % rank_count);
			i++) {
			int tmp = number % base;
			number /= base;
			number = tmp * position + number;
		}
		return number;
	}

	// Tasks

	static int ShiftInt(int number, int shiftCount, ShiftDirection direction) {
		switch (direction)
		{
		case ShiftDirection::left:
			return ShiftLeft(number, shiftCount);
		case ShiftDirection::right:
			return ShiftRight(number, shiftCount);
		default:
			throw std::invalid_argument("Unknown direction");
		}
	}

	static int FibNumber(int n) {
		int a = 0;
		int b = 1;
		for (int i = 0;
			i < n;
			i++) {
			a = a + b;
			b = a - b;
		}
		return a;
	}

	static int DelDecimalInt(int number, int position, int count) {
		int rank_count = GetNumberRankCount(number);

		int zero_count = static_cast<unsigned int>(std::pow(10, rank_count - position - count));

		int last_part = number % zero_count;
		int first_part = number / static_cast<unsigned int>(std::pow(10, rank_count - position));

		return first_part * zero_count + last_part;
	}

	static int GetEvenSumTopAndSecondaryDiagonalMatrix(std::vector<std::vector<int>> matrix) {
		int sum = 0;
		for (int i = 0;
			i < matrix.size();
			i++) {
			if (matrix[i].size() - i - 1 > matrix[i].size()) {
				continue;
			}
			for (int j = 0;
				j < matrix[i].size() - i - 1;
				j++) {
				if (!(i % 2) && !(j % 2)) {
					sum += matrix[i][j];
				}
			}
		}
		return sum;
	}
};
