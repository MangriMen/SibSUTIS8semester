#pragma once
#include <iostream>
#include <regex>
#include "../../lab7/lab7/PNumber.h"

const std::string ZERO("0");
const std::string FLOAT_DELIMETER(".");

class PNumberEditor
{
private:
	std::string current_number;

public:
	PNumberEditor() {
		clear();
	}

	bool isNull() {
		return split(current_number, " ")[0] == "0";
	}

	std::string toggleNegative() {
		auto parsed = split(current_number, " ");
		if (parsed[0][0] == '-') {
			current_number.erase(0, 1);
		}
		else {
			current_number.insert(0, "-");
		}
	}

	std::string appendNumber(long long num) {
		current_number += std::to_string(num);
		return current_number;
	}

	std::string appendZero() {
		return appendNumber(0);
	}

	std::string popNumberBack() {
		current_number = current_number.substr(0, current_number.size() - 1);
		return current_number;
	}

	std::string clear() {
		current_number = ZERO;
		return current_number;
	}

	std::string getNumber() {
		return current_number;
	}

	std::string setNumber(std::string number) {
		bool isValid = std::regex_match(number, std::regex("[0-9]+"));
		if (!isValid) {
			throw std::invalid_argument("Number is not valid");
		}
		current_number = number;
		return current_number;
	}
};
