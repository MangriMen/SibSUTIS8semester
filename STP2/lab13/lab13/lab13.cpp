#include <iostream>
#include <vector>
#include <memory>
#include "MySet.h"
//#include "MySet.h"

using namespace std;

int main()
{
	MySet<int> a;
	a.push_back(3);
	a.push_back(4);
	a.push_back(5);

	MySet<int> b;
	b.push_back(1);
	b.push_back(2);
	b.push_back(3);

	MySet<int> c = a.getUnion(b);

	cout << "C: " << c.size() << "\n";

	for (size_t i = 0; i < c.size(); i++)
	{
		cout << c[i] << " ";
	}

	//auto te = 2;
}
