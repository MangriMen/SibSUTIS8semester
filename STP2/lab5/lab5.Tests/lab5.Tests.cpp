#include "pch.h"
#include "CppUnitTest.h"
#include "../lab5/Fraction.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace FractionTest
{
	TEST_CLASS(FractionTest)
	{
	public:

		TEST_METHOD(TestMethodForInitializeTrue)
		{
			Fraction a(3, 5);
		}

		TEST_METHOD(TestMethodForInitializeFalse)
		{
			auto triggerException = [] { Fraction a(3, 0); };

			Assert::ExpectException<FracException>(triggerException);
		}

		TEST_METHOD(TestMethodForSumPositive)
		{
			Fraction a(3, 5);
			Fraction b(1, 25);

			Fraction expected(16, 25);
			Fraction actual = a + b;

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForSumNegative)
		{
			Fraction a(2, 5);
			Fraction b(-5, 5);

			Fraction expected(-3, 5);
			Fraction actual = a + b;

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForMinusFirst)
		{
			Fraction a(4, 16);
			Fraction b(2, 16);

			Fraction expected(1, 8);
			Fraction actual = b - a;

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForMinusSecond)
		{
			Fraction a(4, 16);
			Fraction b(4, 16);

			Fraction expected(0, 16);
			Fraction actual = a - b;

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForMultiplyFirst)
		{
			Fraction a(2, 8);
			Fraction b(2, 8);

			Fraction expected(4, 64);
			Fraction actual = a * b;

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForMultiplySecond)
		{
			Fraction a(-5, 6);
			Fraction b(2, 6);

			Fraction expected(-5, 18);
			Fraction actual = a * b;

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForDividerFirst)
		{
			Fraction a(1, 8);
			Fraction b(2, 4);

			Fraction expected(1, 4);
			Fraction actual = a / b;

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForDividerSecond)
		{
			Fraction a(5, 5);
			Fraction b(5, 5);

			Fraction expected(1, 1);
			Fraction actual = a / b;

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForPowFirst)
		{
			Fraction a(5, 5);

			Fraction expected(1, 1);
			Fraction actual = Fraction::pow(a, 2);

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForPowSecond)
		{
			Fraction a(5, 3);

			Fraction expected(125, 27);
			Fraction actual = Fraction::pow(a, 3);

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForReciprocalFirst)
		{
			Fraction a(4, 5);

			Fraction expected(5, 4);
			Fraction actual = Fraction::reciprocal(a);

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForReciprocalSecond)
		{
			Fraction a(16, 8);

			Fraction expected(1, 2);
			Fraction actual = Fraction::reciprocal(a);

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForIsEqualFirst)
		{
			Fraction a(16, 8);
			Fraction b(8, 16);

			bool expected = false;
			bool actual = a.isEqual(b);

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForIsEqualSecond)
		{
			Fraction a(16, 16);
			Fraction b(16, 16);

			bool expected = true;
			bool actual = a.isEqual(b);

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForGetNominatorFirst)
		{
			Fraction a(5, 16);

			int expected = 5;
			long long actual = a.getNominator();

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForGetNominatorSecond)
		{
			Fraction a(-8, 16);

			int expected = -1;
			long long actual = a.getNominator();

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForGetDenominatorFirst)
		{
			Fraction a(5, 16);

			int expected = 16;
			long long actual = a.getDenominator();

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForGetDenominatorSecond)
		{
			Fraction a(-8, 16);

			int expected = 2;
			long long actual = a.getDenominator();

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForGetNominatorStringFirst)
		{
			Fraction a(5, 16);

			std::string expected = "5";
			std::string actual = a.getNominatorString();

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForGetNominatorStringSecond)
		{
			Fraction a(993, 1000);

			std::string expected = "993";
			std::string actual = a.getNominatorString();

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForGetDenominatorStringFirst)
		{
			Fraction a(-8, 16);

			std::string expected = "2";
			std::string actual = a.getDenominatorString();

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForGetDenominatorStringSecond)
		{
			Fraction a(-1, 1);

			std::string expected = "1";
			std::string actual = a.getDenominatorString();

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForFractionToStringFirst)
		{
			Fraction a(-1, 1);

			std::string expected = "-1";
			std::string actual = a.toString();

			Assert::IsTrue(expected == actual);
		}

		TEST_METHOD(TestMethodForFractionToStringSecond)
		{
			Fraction a(1000, -7);

			std::string expected = "1000/-7";
			std::string actual = a.toString();

			Assert::IsTrue(expected == actual);
		}

	};
}
