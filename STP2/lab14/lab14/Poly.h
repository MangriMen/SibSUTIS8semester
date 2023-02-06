#pragma once
#include <iostream>
#include <vector>
#include <algorithm>
#include "SimplePoly.h"

class Poly
{
private:
	std::vector<SimplePoly> pol;
public:
	Poly() = default;

	Poly(long long coeff, long long degree) {
		pol.push_back(SimplePoly(coeff, degree));
	}

	long long degree() const {
		if (pol.size() == 0) {
			return 0;
		}

		return (*std::max_element(pol.begin(), pol.end(),
			[](const SimplePoly& a, const SimplePoly& b)
			{
				return a.getDegree() < b.getDegree();
			}
		)).getDegree();
	}

	long long coeff(long long degree) {
		if (pol.size() == 0) {
			return 0;
		}

		auto el = std::find_if(pol.begin(), pol.end(),
			[degree](const SimplePoly& a)
			{
				return a.getDegree() == degree;
			}
		);

		return el != pol.end() ? (*el).getCoeff() : 0;
	}

	void clear() {
		pol.clear();
	}

	Poly operator+(const Poly& rhs) {
		auto maxDegree = static_cast<size_t>(std::max(degree(), rhs.degree()));
		size_t maxSize = std::max(pol.size(), rhs.pol.size());

		bool isRHSPol = pol.size() > rhs.pol.size();

		auto newPoly = Poly();

		for (size_t i = 0; i <= maxDegree; i++)
		{
			bool atLeastOne = false;
			long long sum = 0;
			for (size_t j = 0; j < pol.size(); j++) {
				if (pol[j].getDegree() == i) {
					sum += pol[j].getCoeff();
					atLeastOne = true;
				}
			}
			for (size_t j = 0; j < rhs.pol.size(); j++) {
				if (rhs.pol[j].getDegree() == i) {
					sum += rhs.pol[j].getCoeff();
					atLeastOne = true;
				}
			}
			if (atLeastOne) {
				newPoly.pol.push_back(SimplePoly(sum, i));
			}
		}

		return newPoly;
	}

	Poly operator-(const Poly& rhs) {
		auto maxDegree = static_cast<size_t>(std::max(degree(), rhs.degree()));
		size_t maxSize = std::max(pol.size(), rhs.pol.size());

		bool isRHSPol = pol.size() > rhs.pol.size();

		auto newPoly = Poly();

		for (size_t i = 0; i <= maxDegree; i++)
		{
			bool atLeastOne = false;
			long long sum = 0;
			for (size_t j = 0; j < pol.size(); j++) {
				if (pol[j].getDegree() == i) {
					sum += pol[j].getCoeff();
					atLeastOne = true;
				}
			}
			for (size_t j = 0; j < rhs.pol.size(); j++) {
				if (rhs.pol[j].getDegree() == i) {
					sum -= rhs.pol[j].getCoeff();
					atLeastOne = true;
				}
			}
			if (atLeastOne) {
				newPoly.pol.push_back(SimplePoly(sum, i));
			}
		}


		return newPoly;
	}

	Poly operator-() {
		auto newPoly = Poly();

		for (const auto& poly : pol) {
			newPoly.pol.push_back(SimplePoly(-poly.getCoeff(), poly.getDegree()));
		}

		return newPoly;
	}

	Poly operator*(const Poly& rhs) {
		auto newPoly = Poly();
		for (size_t i = 0; i < pol.size(); i++)
		{
			for (size_t j = 0; j < rhs.pol.size(); j++)
			{
				newPoly.add(
					pol[i].getCoeff() * rhs.pol[j].getCoeff(),
					pol[i].getDegree() + rhs.pol[j].getDegree()
				);
			}
		}
		return newPoly;
	}

	bool operator==(const Poly& rhs) const {
		return pol == rhs.pol;
	}

	bool operator!=(const Poly& rhs) const {
		return pol != rhs.pol;
	}

	SimplePoly operator[](size_t index) {
		return pol[index];
	}

	long long calc(long long x) {
		long long result = 0;
		for (const auto& poly : pol) {
			result += poly.calc(x);
		}
		return result;
	}

	void add(long long coeff_, long long degree_) {
		auto el = std::find_if(pol.begin(), pol.end(),
			[degree_](const SimplePoly& a)
			{
				return a.getDegree() == degree_;
			}
		);

		if (el != pol.end()) {
			*(el) = SimplePoly((*el).getCoeff() + coeff_, degree_);
		}
		else {
			pol.push_back(SimplePoly(coeff_, degree_));
		}

		std::sort(pol.begin(), pol.end());
	}
};

