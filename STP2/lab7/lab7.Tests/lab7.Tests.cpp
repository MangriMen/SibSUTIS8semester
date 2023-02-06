#include "pch.h"
#include "CppUnitTest.h"
#include "../lab7/PNumber.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab7Tests
{
	TEST_CLASS(lab7Tests)
	{
	public:
		
		TEST_METHOD(TestPNumber_1)
		{
			PNumber(1, 2, 3);
		}

		TEST_METHOD(TestPNumber_2)
		{
			auto triggerException = []() {PNumber(1, 0, 3); };
			Assert::ExpectException<std::invalid_argument>(triggerException);
		}

		TEST_METHOD(TestPNumber_3)
		{
			PNumber(-1, 2, 3);
		}

		TEST_METHOD(TestPNumber_4)
		{
			auto triggerException = []() {PNumber(-1, 18, 3); };
			Assert::ExpectException<std::invalid_argument>(triggerException);
		}

		TEST_METHOD(TestPNumberCopy) {
			auto a = PNumber(-1, 2, 3);
			auto b = a;

			Assert::IsTrue(a == b);
		}

		TEST_METHOD(TestPNumberAdd) {
			auto a = PNumber(-1, 2, 3);
			auto b = PNumber(1, 2, 3);

			auto expectedValue = PNumber(0, 2, 3);
			auto actualValue = a + b;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPNumberSub) {
			auto a = PNumber(0, 2, 3);
			auto b = PNumber(1, 2, 3);

			auto expectedValue = PNumber(-1, 2, 3);
			auto actualValue = a - b;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPNumberMul) {
			auto a = PNumber(-1, 2, 3);
			auto b = PNumber(1, 2, 3);

			auto expectedValue = PNumber(-1, 2, 3);
			auto actualValue = a * b;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPNumberDiv) {
			auto a = PNumber(-1, 2, 3);
			auto b = PNumber(1, 2, 3);

			auto expectedValue = PNumber(-1, 2, 3);
			auto actualValue = a / b;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestReciprocal) {
			auto a = PNumber(2, 8, 3);

			auto expectedValue = PNumber(1.0 / 2, 8, 3);
			auto actualValue = a.reciprocal();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPow) {
			auto a = PNumber(2, 8, 3);

			auto expectedValue = PNumber(4, 8, 3);
			auto actualValue = a.pow();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestNum) {
			auto a = PNumber(2, 8, 3);

			auto expectedValue = 2;
			auto actualValue = a.getNum();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestGetSetBase) {
			auto a = PNumber(2, 8, 3);

			auto expectedValue = 8;
			auto actualValue = a.getBase();

			Assert::IsTrue(expectedValue == actualValue);

			expectedValue = 10;
			a.setBase(expectedValue);
			actualValue = a.getBase();

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestGetSetAccuracy) {
			auto a = PNumber(2, 8, 3);

			auto expectedValue = 3;
			auto actualValue = a.getAccuracy();

			Assert::IsTrue(expectedValue == actualValue);

			expectedValue = 10;
			a.setAccuracy(expectedValue);
			actualValue = a.getAccuracy();

			Assert::IsTrue(expectedValue == actualValue);
		}
	};
}
