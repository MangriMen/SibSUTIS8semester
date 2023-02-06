#include "pch.h"
#include "CppUnitTest.h"
#include "../lab14/Poly.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab14Tests
{
	TEST_CLASS(lab14Tests)
	{
	public:

		TEST_METHOD(TestPoly)
		{
			auto poly = Poly();

			auto expectedValue = poly.degree();
			auto actualValue = 0;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestDegree)
		{
			auto poly = Poly(3, 20);

			auto expectedValue = poly.degree();
			auto actualValue = 20;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestCoeff_1) {
			auto poly = Poly(3, 15);
			poly.add(4, 20);

			auto expectedValue = poly.coeff(15);
			auto actualValue = 3;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestCoeff_2) {
			auto poly = Poly(3, 15);
			poly.add(4, 20);

			auto expectedValue = poly.coeff(1);
			auto actualValue = 0;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestClear) {
			auto poly = Poly(3, 15);
			poly.add(4, 20);

			auto expectedValue = poly.degree();
			auto actualValue = 20;

			Assert::IsTrue(expectedValue == actualValue);

			poly.clear();

			expectedValue = poly.degree();
			actualValue = 0;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestEqual) {
			auto polyA = Poly(3, 15);
			polyA.add(4, 20);

			auto polyB = Poly(4, 20);
			polyB.add(3, 15);

			Assert::IsTrue(polyA == polyB);
		}

		TEST_METHOD(TestIndex) {
			auto poly = Poly(3, 15);
			poly.add(4, 20);

			auto expectedValue = poly[1];
			auto actualValue = SimplePoly(4, 20);

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestCalc) {
			auto poly = Poly(2, 1);
			poly.add(4, 1);

			auto expectedValue = poly.calc(1);
			auto actualValue = 6;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSum) {
			auto polyA = Poly(2, 2);
			auto polyB = Poly(4, 2);
			auto polyC = polyA + polyB;

			auto expectedValue = polyC.coeff(2);
			auto actualValue = 6;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSub) {
			auto polyA = Poly(2, 2);
			auto polyB = Poly(4, 2);
			auto polyC = polyA - polyB;

			auto expectedValue = polyC.coeff(2);
			auto actualValue = -2;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestMul) {
			auto polyA = Poly(2, 2);
			polyA.add(1, 1);
			auto polyB = Poly(2, 2);
			polyB.add(1, 1);
			auto polyC = polyA * polyB;

			auto expectedValue = polyC.calc(2);
			auto actualValue = 100;

			Assert::IsTrue(expectedValue == actualValue);
		}
	};
}
