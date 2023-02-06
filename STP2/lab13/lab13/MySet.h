#pragma once
#include <iostream>
#include <memory>
#include <vector>
#include <algorithm>

template <typename T>
class Tree {
public:
	class Node {
	public:
		typedef std::shared_ptr<T> T_ptr;

		T data;
		T_ptr left;
		T_ptr right;

		Node(const T& data, T_ptr left = nullptr, T_ptr right = nullptr) {
			this->data = data;
			this->left = left;
			this->right = right;
		}
	};
private:
	typedef std::shared_ptr<Node> Node_ptr;

	Node_ptr _root;
	size_t _size;

	void add_rec(Node_ptr root, const T& data) {
		if (root == nullptr) {
			root = std::make_shared<Node>(data);
			return;
		}

		//add(root->)
	}

	void clear_rec(Node_ptr root) {
		if (root == nullptr) {
			return;
		}

		clear_rec(root->left);
		clear_rec(root->right);

		root.reset();
	}

public:
	Tree() {

	}

	Tree(const T& root_data) {
		_root = std::make_shared<Node>(root_data);
	}

	void add(const T& data) {
		add_rec(_root, data);
	}

	typename Node_ptr getRoot() {
		return _root;
	}

	void clear() {
		clear_rec(_root);
	}

	Node_ptr find(const T& item) {
		return _root;
	}

	size_t size() {
		return _size;
	}
};

template <typename T>
class MySet
{
public:
	std::vector<T> _data;

	MySet() = default;
	~MySet() = default;

	void clear() {
		_data.clear();
	}

	void push_back(const T& item) {
		if (!contains(item)) {
			_data.push_back(item);
			std::sort(_data.begin(), _data.end());
		}
	}

	void remove(const T& item) {
		auto el = std::find(_data.begin(), _data.end(), item);
		if (el != _data.end()) {
			_data.erase(el);
		}
	}

	bool empty() {
		return _data.size() == 0;
	}

	bool contains(const T& item) const {
		return std::find(_data.begin(), _data.end(), item) != _data.end() ? true : false;
	}

	MySet getUnion(const MySet& rhs) {
		MySet<T> newSet;
		for (size_t i = 0; i < _data.size(); i++)
		{
			if (rhs.contains(_data[i])) {
				newSet.push_back(_data[i]);
			}
		}
		return newSet;
	}

	MySet getConflux(const MySet& rhs) {
		MySet<T> newSet;
		for (const auto& item : _data) {
			newSet.push_back(item);
		}
		for (const auto& item : rhs._data) {
			newSet.push_back(item);
		}
		return newSet;
	}

	MySet getSub(const MySet& rhs) {
		MySet<T> newSet;
		for (size_t i = 0; i < _data.size(); i++)
		{
			if (!rhs.contains(_data[i])) {
				newSet.push_back(_data[i]);
			}
		}
		return newSet;
	}

	size_t size() {
		return _data.size();
	}

	T operator[](size_t index) const {
		return _data[index];
	}
};
