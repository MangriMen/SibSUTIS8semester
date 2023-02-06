#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <regex>

std::vector<std::string> split(std::string s, std::string delimiter) {
	size_t pos_start = 0, pos_end, delim_len = delimiter.length();
	std::string token;
	std::vector<std::string> res;

	while ((pos_end = s.find(delimiter, pos_start)) != std::string::npos) {
		token = s.substr(pos_start, pos_end - pos_start);
		pos_start = pos_end + delim_len;
		res.push_back(token);
	}

	res.push_back(s.substr(pos_start));
	return res;
}

class ComplexEditor
{
private:
	std::string current_number;

public:
	inline static const std::string ZERO = "0+i*0";

	ComplexEditor() {
		clear();
	}

	bool isNull() {
		return split(current_number, " ")[0] == ZERO;
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
		bool isValid = std::regex_match(number, std::regex("[0-9]+\\+i\\*\\(?-?[0-9]+(\\.?[0-9]+)\\)?"));
		if (!isValid) {
			throw std::invalid_argument("Number is not valid");
		}
		current_number = number;
		return current_number;
	}
};

