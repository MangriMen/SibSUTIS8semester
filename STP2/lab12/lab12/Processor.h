#pragma once
#include <iostream>

template <typename T>
class Processor
{
public:
	enum class Operation {
		None,
		Add,
		Sub,
		Mul,
		Dvd
	};

	enum class Func {
		Rev,
		Sqr
	};

private:
	T _l_op_res;
	T _r_op;
	Operation _operation;

public:
	Processor() {
		reset();
	}

	void reset() {
		_l_op_res = T();
		_r_op = T();
		_operation = Operation::None;
	}

	void resetOperation() {
		_operation = Operation::None;
	}

	void performOperation() {
		switch (_operation)
		{
		case Operation::None:
			break;
		case Operation::Add:
			_l_op_res = _r_op + _l_op_res;
			break;
		case Operation::Sub:
			_l_op_res = _r_op - _l_op_res;
			break;
		case Operation::Mul:
			_l_op_res = _r_op * _l_op_res;
			break;
		case Operation::Dvd:
			_l_op_res = _r_op / _l_op_res;
			break;
		default:
			throw std::invalid_argument("No such operation exists");
		}
	}

	void performFunction(Func function) {
		switch (function)
		{
		case Func::Rev:
			_r_op = _r_op.reciprocal();
			break;
		case Func::Sqr:
			_r_op = _r_op.pow(2);
			break;
		default:
			throw std::invalid_argument("No such operation exists");
		}
	}

	T getLeftOperand() {
		return _l_op_res;
	}

	void setLeftOperand(T op) {
		_l_op_res = op;
	}

	T getRightOperand() {
		return _r_op;
	}

	void setRightOperand(T operand) {
		_r_op = operand;
	}

	Operation getState() {
		return _operation;
	}

	void setState(Operation state) {
		_operation = state;
	}
};

