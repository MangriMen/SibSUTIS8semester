#pragma once
#include <iostream>
#include <string>
#include <numeric>
#include <algorithm>
#include <sstream>

class FracException : public std::exception {
private:
	const char* message;

public:
	FracException(const char* msg) : message(msg) {}
	const char* what() {
		return message;
	}
};

class Fraction
{
private:
	long long _nominator = 0;
	long long _denominator = 0;

	void reduce() {
		long long GCD = std::gcd(std::abs(_nominator), _denominator);

		if (GCD != 1)
		{
			_nominator /= GCD;
			_denominator /= GCD;
		}
	}

public:
	Fraction(long long a = 0, long long b = 1) {
		if (b == 0) {
			throw FracException("Denominator is null");
		}

		_nominator = a;
		_denominator = b;

		reduce();
	}

	Fraction(const std::string& f) {
		size_t delimeter_pos = f.find('/');

		if (delimeter_pos == std::string::npos) {
			throw FracException("Invalid string, this is not fraction");
		}

		_nominator = std::stoll(f.substr(0, delimeter_pos));
		_denominator = std::stoll(f.substr(delimeter_pos + 1));

		reduce();
	}

	Fraction(const Fraction& x)
	{
		_nominator = x._nominator;
		_denominator = x._denominator;
	}

	Fraction operator+(const Fraction& rhs)
	{
		long long union_denom = std::lcm(_denominator, rhs._denominator);

		long long rel_num = _nominator * union_denom / _denominator;
		long long mul_num = rhs._nominator * union_denom / rhs._denominator;

		Fraction fraction(rel_num + mul_num, union_denom);
		return fraction;
	}

	Fraction operator-(const Fraction& rhs)
	{
		long long unionDenom = std::lcm(_denominator, rhs._denominator);

		long long relNum = _nominator * unionDenom / _denominator;
		long long mulNum = rhs._nominator * unionDenom / rhs._denominator;

		Fraction fraction(mulNum - relNum, unionDenom);
		return fraction;
	}

	Fraction operator*(const Fraction& rhs)
	{
		Fraction fraction(_nominator * rhs._nominator, _denominator * rhs._denominator);
		return fraction;
	}

	Fraction operator/(const Fraction& rhs)
	{
		long long nominator = _nominator * rhs._denominator;
		long long denominator = _denominator * rhs._nominator;

		if (denominator < 0)
		{
			nominator *= -1;
			denominator *= -1;
		}

		Fraction fraction(_nominator * rhs._denominator, _denominator * rhs._nominator);
		return fraction;
	}

	static Fraction pow(const Fraction& fraction_, long long n = 2) {
		Fraction fraction(static_cast<long long>(std::pow(fraction_._nominator, n)), static_cast<long long>(std::pow(fraction_._denominator, n)));
		return fraction;
	}

	static Fraction reciprocal(const Fraction& fraction_) {
		Fraction fraction(fraction_._denominator, fraction_._nominator);
		return fraction;
	}


	bool isEqual(const Fraction& fraction_) const {
		return (_nominator == fraction_._nominator && _denominator == fraction_._denominator);
	}

	long long getNominator() const {
		return _nominator;
	}

	long long getDenominator() const {
		return _denominator;
	}

	std::string getNominatorString() const {
		return std::to_string(_nominator);
	}

	std::string getDenominatorString() const {
		return std::to_string(_denominator);
	}

	std::string toString() const {
		const std::string nominator = getNominatorString();
		const std::string denominator = getDenominatorString();

		std::stringstream out;

		if (_nominator != 0 && _denominator == 1) {
			out << nominator;
		}
		else {
			out << nominator << "/" << denominator;
		}

		return out.str();
	}
  
	bool operator==(const Fraction& rhs) const {
		return isEqual(rhs);
	}

	friend std::ostream& operator<<(std::ostream&, const Fraction&);
};
