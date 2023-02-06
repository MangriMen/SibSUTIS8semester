#include "pch.h"
#include "CppUnitTest.h"
#include "../lab3/Class1.h"
#include <vector>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab3Tests
{
	TEST_CLASS(lab3Tests)
	{
	public:
		TEST_METHOD(TestGetNumberRankCount)
		{
			auto expected_value = 6;
			auto actual_value = Class1::GetNumberRankCount(123456);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestGetNumberRankCountNumberZero)
		{
			auto expected_value = 1;
			auto actual_value = Class1::GetNumberRankCount(0);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestShiftLeft)
		{
			auto expected_value = 345612;
			auto actual_value = Class1::ShiftLeft(123456, 2);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestShiftLeftNoShift)
		{
			auto expected_value = 123456;
			auto actual_value = Class1::ShiftLeft(123456, 0);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestShiftRight)
		{
			auto expected_value = 561234;
			auto actual_value = Class1::ShiftRight(123456, 2);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestShiftRightNoShift)
		{
			auto expected_value = 123456;
			auto actual_value = Class1::ShiftRight(123456, 0);

			Assert::AreEqual(expected_value, actual_value);
		}

		// Tasks

		TEST_METHOD(TestShiftInt)
		{
			auto expected_value = 345612;
			auto actual_value = Class1::ShiftInt(123456, 2, Class1::ShiftDirection::left);

			Assert::AreEqual(expected_value, actual_value);

			expected_value = 561234;
			actual_value = Class1::ShiftInt(123456, 2, Class1::ShiftDirection::right);

			Assert::AreEqual(expected_value, actual_value);

			auto triggerException = [] { Class1::ShiftInt(123456, 2, Class1::ShiftDirection(20)); };

			Assert::ExpectException<std::invalid_argument>(triggerException);
		}

		TEST_METHOD(TestFibNumber)
		{
			auto expected_value = 3;
			auto actual_value = Class1::FibNumber(4);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestFibNumberZero)
		{
			auto expected_value = 0;
			auto actual_value = Class1::FibNumber(0);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestDelDecimalInt)
		{
			auto expected_value = 1256;
			auto actual_value = Class1::DelDecimalInt(123456, 2, 2);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestGetEvenSumTopAndSecondaryDiagonalMatrix)
		{
			std::vector<std::vector<int>> matrix{ {1, 2, 3, 32, 3}, { 1,2,3,32,3 }, { 1,2,3,32,3 }, { 1,2,3,32,3 }, { 1,2,3,32,3 } };
			Assert::AreEqual(5, Class1::GetEvenSumTopAndSecondaryDiagonalMatrix(matrix));
		}

		TEST_METHOD(TestGetEvenSumTopAndSecondaryDiagonalMatrixEmptyMatrix)
		{
			std::vector<std::vector<int>> matrix;

			auto expected_value = 0;
			auto actual_value = Class1::GetEvenSumTopAndSecondaryDiagonalMatrix(matrix);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestGetEvenSumTopAndSecondaryDiagonalMatrixEmptyRows)
		{
			std::vector<std::vector<int>> matrix{ { }, { }, { }, { }, { } };

			auto expected_value = 0;
			auto actual_value = Class1::GetEvenSumTopAndSecondaryDiagonalMatrix(matrix);

			Assert::AreEqual(expected_value, actual_value);
		}

		TEST_METHOD(TestGetEvenSumTopAndSecondaryDiagonalMatrixWithoutEvenIndexes)
		{
			std::vector<std::vector<int>> matrix{ { 0, 1 }, { 2, 3 } };

			auto expected_value = 0;
			auto actual_value = Class1::GetEvenSumTopAndSecondaryDiagonalMatrix(matrix);

			Assert::AreEqual(expected_value, actual_value);
		}
	};
}
