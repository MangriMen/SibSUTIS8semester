#pragma once
#include <iostream>
#include <string>

class SimplePoly
{
private:
	long long _coeff;
	long long _degree;
public:
	SimplePoly(long long coeff = 0, long long degree = 0) : _coeff(coeff), _degree(degree) {
		if (_coeff == 0) {
			_degree = 0;
		}
	}

	long long getDegree() const {
		return _degree;
	}

	void setDegree(long long degree) {
		_degree = degree;
	}

	long long getCoeff() const {
		return _coeff;
	}

	void setCoeff(long long coeff) {
		_coeff = coeff;
	}

	bool operator==(const SimplePoly& rhs) const {
		return _coeff == rhs._coeff && _degree == rhs._degree;
	}

	bool operator!=(const SimplePoly& rhs) const {
		return _coeff != rhs._coeff || _degree != rhs._degree;
	}

	SimplePoly diff() {
		return SimplePoly(_coeff * (_degree), _degree - 1);
	}

	long long calc(long long x) const {
		return (_coeff * static_cast<long long>(std::pow(x, _degree)));
	}

	std::string ToString(const std::string& x = "x") {
		return std::to_string(_coeff) + x + "^" + std::to_string(_degree);
	}

	bool operator>(const SimplePoly& rhs) {
		return calc(1) > rhs.calc(1);
	}

	bool operator<(const SimplePoly& rhs) {
		return calc(1) < rhs.calc(1);
	}
};

