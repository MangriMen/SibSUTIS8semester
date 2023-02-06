#pragma once
#include <iostream>
#include <string>
#include <vector>

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

class PNumber
{
	long double num = 0;
	int base = 0;
	int accuracy = 0;

public:
	PNumber(long double a_ = 0, int b_ = 0, int c_ = 0) {
		if (b_ < 2 || b_ > 16) {
			throw std::invalid_argument("Base must be in range [2..16]");
		}

		num = a_;
		base = b_;
		accuracy = c_;
	}

	PNumber(const std::string& str) {
		auto parsed = split(str, " ");
		long double aParsed = std::stold(parsed[0]);
		int bParsed = std::stoi(parsed[1]);
		int cParsed = std::stoi(parsed[2]);

		if (bParsed < 2 || bParsed > 16) {
			throw std::invalid_argument("Base must be in range [2..16]");
		}

		num = aParsed;
		base = bParsed;
		accuracy = cParsed;
	}

	PNumber(const PNumber& pnumber) {
		num = pnumber.num;
		base = pnumber.base;
		accuracy = pnumber.accuracy;
	}

	PNumber operator+(const PNumber& pnumber) {
		if (!(base == pnumber.base) || !(accuracy == pnumber.accuracy)) {
			throw std::runtime_error("Numbers are raznie");
		}

		return PNumber(num + pnumber.num, base, accuracy);
	}

	PNumber operator-(const PNumber& pnumber) {
		if (!(base == pnumber.base) || !(accuracy == pnumber.accuracy)) {
			throw std::runtime_error("Numbers are raznie");
		}

		return PNumber(num - pnumber.num, base, accuracy);
	}

	PNumber operator*(const PNumber& pnumber) {
		if (!(base == pnumber.base) || !(accuracy == pnumber.accuracy)) {
			throw std::runtime_error("Numbers are raznie");
		}

		return PNumber(num * pnumber.num, base, accuracy);
	}

	PNumber operator/(const PNumber& pnumber) {
		if (!(base == pnumber.base) || !(accuracy == pnumber.accuracy)) {
			throw std::runtime_error("Numbers are raznie");
		}

		return PNumber(num / pnumber.num, base, accuracy);
	}

	PNumber reciprocal() {
		return PNumber(1.0 / num, base, accuracy);
	}

	PNumber pow(int n=2) {
		return PNumber(std::pow(num, n), base, accuracy);
	}

	long double getNum() const {
		return num;
	}

	std::string getNumStr() const {
		return std::to_string(num) + " " + std::to_string(base) + " " + std::to_string(accuracy);
	}

	int getBase() const {
		return base;
	}

	std::string getBaseStr() const {
		return std::to_string(base);
	}

	int getAccuracy() const {
		return accuracy;
	}

	std::string getAccuracyStr() const {
		return std::to_string(accuracy);
	}

	void setBase(int new_base) {
		if (new_base < 2 || new_base > 16) {
			throw std::invalid_argument("Base must be in range [2..16]");
		}

		if (new_base < base) {
			throw std::runtime_error("New base cannot be lesser");
		}

		base = new_base;
	}

	void setBase(const std::string& new_base) {
		setBase(std::stoi(new_base));
	}

	void setAccuracy(int new_accuracy) {
		if (new_accuracy < 0) {
			throw std::invalid_argument("Accuracy must be more or equal zero");
		}

		accuracy = new_accuracy;
	}

	void setAccuracy(const std::string& new_accuracy) {
		setAccuracy(std::stoi(new_accuracy));
	}

	bool operator==(const PNumber& rhs) const {
		if (!(base == rhs.base) || !(accuracy == rhs.accuracy)) {
			throw std::runtime_error("Numbers are raznie");
		}

		return num == rhs.num;
	}

	std::string toString() const {
		return getNumStr();
	}
};
