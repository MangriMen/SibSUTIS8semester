#include "pch.h"
#include "CppUnitTest.h"
#include "../lab11/Memory.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab11Tests
{
	TEST_CLASS(lab11Tests)
	{
	public:

		TEST_METHOD(TestMemory)
		{
			auto expectedValue = int();
			auto actualValue = Memory<int>().getNumber();

			Assert::AreEqual(expectedValue, actualValue);
		}

		TEST_METHOD(TestStore)
		{
			auto cell = Memory<int>();
			cell.store(2);

			auto expectedValue = int(2);
			auto actualValue = cell.getNumber();

			Assert::AreEqual(expectedValue, actualValue);

			auto expectedValue1 = std::string("_On");
			auto actualValue1 = cell.getStateStr();

			Assert::AreEqual(expectedValue1, actualValue1);
		}

		TEST_METHOD(TestRead)
		{
			auto cell = Memory<int>();
			cell.store(2);

			auto expectedValue = int(2);
			auto actualValue = cell.read();

			Assert::AreEqual(expectedValue, actualValue);

			auto expectedValue1 = std::string("_On");
			auto actualValue1 = cell.getStateStr();

			Assert::AreEqual(expectedValue1, actualValue1);
		}

		TEST_METHOD(TestAdd)
		{
			auto cell = Memory<int>();
			cell.store(2);
			cell.add(2);

			auto expectedValue = int(4);
			auto actualValue = cell.getNumber();

			Assert::AreEqual(expectedValue, actualValue);

			auto expectedValue1 = std::string("_On");
			auto actualValue1 = cell.getStateStr();

			Assert::AreEqual(expectedValue1, actualValue1);
		}

		TEST_METHOD(TestClear)
		{
			auto cell = Memory<int>();
			cell.store(2);
			cell.clear();

			auto expectedValue = int();
			auto actualValue = cell.getNumber();

			Assert::AreEqual(expectedValue, actualValue);
		}

		TEST_METHOD(TestGetStateStr)
		{
			auto cell = Memory<int>();

			auto expectedValue = std::string("_Off");
			auto actualValue = cell.getStateStr();

			Assert::AreEqual(expectedValue, actualValue);

			cell.read();

			expectedValue = std::string("_Off");
			actualValue = cell.getStateStr();

			Assert::AreEqual(expectedValue, actualValue);
		}

		TEST_METHOD(TestGetNumberStr)
		{
			auto cell = Memory<int>();
			cell.store(2);

			auto expectedValue = int(2);
			auto actualValue = cell.getNumber();

			Assert::AreEqual(expectedValue, actualValue);
		}
	};
}
