#include "pch.h"
#include "CppUnitTest.h"
#include "../lab6/Complex.h"
#include <sstream>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab6Tests
{
	TEST_CLASS(lab6Tests)
	{
	public:
		
		TEST_METHOD(TestConstructor)
		{
			auto _ = Complex(1, 5);
		}
		TEST_METHOD(TestConstructorZeroes)
		{
			auto _ = Complex(0, 0);
		}
		TEST_METHOD(TestConstructorNegative)
		{
			auto _ = Complex(-1, -5);
		}

		TEST_METHOD(TestPlus)
		{
			auto firstValue = Complex(2, 5);
			auto secondValue = Complex(1, 5);

			auto expectedValue = Complex(3, 10);
			auto actualValue = firstValue + secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestPlusOneNegative)
		{
			auto firstValue = Complex(2, 5);
			auto secondValue = Complex(-1, -5);

			auto expectedValue = Complex(1, 0);
			auto actualValue = firstValue + secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestPlusBothNegative)
		{
			auto firstValue = Complex(-2, -5);
			auto secondValue = Complex(-1, -5);

			auto expectedValue = Complex(-3, -10);
			auto actualValue = firstValue + secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestPlusZeroAnswer)
		{
			auto firstValue = Complex(-2, 0);
			auto secondValue = Complex(2, 0);

			auto expectedValue = Complex(0, 0);
			auto actualValue = firstValue + secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestMinus)
		{
			auto firstValue = Complex(2, 5);
			auto secondValue = Complex(1, 5);

			auto expectedValue = Complex(1, 0);
			auto actualValue = firstValue - secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestMinusSecondNegative)
		{
			auto firstValue = Complex(2, 5);
			auto secondValue = Complex(-1, -5);

			auto expectedValue = Complex(3, 10);
			auto actualValue = firstValue - secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestMinusBothNegative)
		{
			auto firstValue = Complex(-2, -5);
			auto secondValue = Complex(-1, -5);

			auto expectedValue = Complex(-1, 0);
			auto actualValue = firstValue - secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestMinusZeroAnswer)
		{
			auto firstValue = Complex(2, 5);
			auto secondValue = Complex(2, 5);

			auto expectedValue = Complex(0, 0);
			auto actualValue = firstValue - secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestMinusUnary)
		{
			auto testValue = Complex(2, 5);

			auto expectedValue = Complex(-2, -5);
			auto actualValue = -testValue;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestMul)
		{
			auto firstValue = Complex(2, 5);
			auto secondValue = Complex(4, 5);

			auto expectedValue = Complex(-17, 30);
			auto actualValue = firstValue * secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestMulDouble)
		{
			auto firstValue = Complex(0.68, 4.4);
			auto secondValue = Complex(25.3, 0.97);

			auto expectedValue = Complex(12.936, 111.9796);
			auto actualValue = firstValue * secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestMulNegative)
		{
			auto firstValue = Complex(2, -5);
			auto secondValue = Complex(-4, 5);

			auto expectedValue = Complex(17, 30);
			auto actualValue = firstValue * secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestDiv)
		{
			auto firstValue = Complex(64, 16);
			auto secondValue = Complex(2, 26);

			auto expectedValue = Complex(0.8, -2.4);
			auto actualValue = firstValue / secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestDivNegative)
		{
			auto firstValue = Complex(-64, 16);
			auto secondValue = Complex(-2, 26);

			auto expectedValue = Complex(0.8, 2.4);
			auto actualValue = firstValue / secondValue;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestEqual)
		{
			auto firstValue = Complex(2, 5);
			auto secondValue = Complex(2, 5);

			auto expectedValue = firstValue == secondValue;
			auto actualValue = true;

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestUnequal)
		{
			auto firstValue = Complex(2, 5);
			auto secondValue = Complex(23, 125);

			auto expectedValue = firstValue != secondValue;
			auto actualValue = true;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPow)
		{
			auto testValue = Complex(64, 16);

			auto expectedValue = Complex(212992, 192512);
			auto actualValue = testValue.pow(3);

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestPowNegative)
		{
			auto testValue = Complex(-64, 16);

			auto expectedValue = Complex(-212992, 192512);
			auto actualValue = testValue.pow(3);

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestRoot)
		{
			auto testValue = Complex(3840, 2048);

			auto expectedValue = Complex(64, 16);
			auto actualValue = testValue.root(2, 0);

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAbs)
		{
			auto testValue = Complex(2, 2);

			auto expectedValue = 2.82842712474619009l;
			auto actualValue = testValue.abs();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAngleRad_I)
		{
			auto testValue = Complex(2, 2);

			auto expectedValue = 0.78539816339744828;
			auto actualValue = testValue.angleRad();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAngleRad_II)
		{
			auto testValue = Complex(-2, 2);

			auto expectedValue = 2.356194490192345;
			auto actualValue = testValue.angleRad();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAngleRad_III)
		{
			auto testValue = Complex(-2, -2);

			auto expectedValue = 3.9269908169872414;
			auto actualValue = testValue.angleRad();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAngleRad_IV)
		{
			auto testValue = Complex(2, -2);

			auto expectedValue = -0.78539816339744828;
			auto actualValue = testValue.angleRad();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAngleRad_X_Y)
		{
			auto testValue = Complex(0, 2);

			auto expectedValue = PI / 2;
			auto actualValue = testValue.angleRad();

			Assert::IsTrue(expectedValue == actualValue);

			testValue = Complex(0, -2);

			expectedValue = -PI / 2;
			actualValue = testValue.angleRad();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAngleDeg)
		{
			auto testValue = Complex(2, -2);

			auto expectedValue = -45;
			auto actualValue = testValue.angleDeg();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestGetReal)
		{
			auto testValue = Complex(2, -2);

			auto expectedValue = 2.0l;
			auto actualValue = testValue.real();

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestGetImagine)
		{
			auto testValue = Complex(2, -2);

			auto expectedValue = -2.0l;
			auto actualValue = testValue.img();

			Assert::IsTrue(expectedValue == actualValue);
		}
		TEST_METHOD(TestToString)
		{
			auto testValue = Complex(2, -2);

			auto expectedValue = "2.000000+i*(-2.000000)";
			
			std::stringstream ss;
			ss << testValue;
			auto actualValue = ss.str();

			Assert::IsTrue(expectedValue == actualValue);
		}
	};
}
