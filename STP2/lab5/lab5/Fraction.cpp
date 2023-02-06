#include "Fraction.h"

bool operator==(const Fraction& lhs, const Fraction& rhs) {
	return lhs.isEqual(rhs);
}

std::ostream& operator<<(std::ostream& os, const Fraction& fraction)
{
	os << fraction.toString();
	return os;
}