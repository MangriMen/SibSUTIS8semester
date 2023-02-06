#pragma once
#include <iostream>

template <typename T>
class Memory
{
	T _number;
	bool _is_on;
public:
	Memory() {
		clear();
	}

	void store(T object) {
		_number = object;
		_is_on = true;
	}

	T read() {
		return _number;
		_is_on = true;
	}

	void add(T object) {
		_number += object;
		_is_on = true;
	}

	void clear() {
		_number = T();
		_is_on = false;
	}

	std::string getStateStr() {
		return _is_on ? "_On" : "_Off";
	}

	T getNumber() {
		return _number;
	}
};

