#include <iostream>
#include <vector>
#include "Class1.h"

using namespace std;

int main()
{
	Class1 myClass;
	vector<vector<int>> matr{ {1,2,3,32,3},{1,2,3,32,3},{1,2,3,32,3},{1,2,3,32,3},{1,2,3,32,3} };
	int a = myClass.GetEvenSumTopAndSecondaryDiagonalMatrix(matr);
	cout << a;
}
