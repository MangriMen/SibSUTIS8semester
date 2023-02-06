#include "pch.h"
#include "CppUnitTest.h"
#include "../lab8/PNumberEditor.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab8Tests
{
	TEST_CLASS(lab8Tests)
	{
	public:

		TEST_METHOD(TestPNumberEditor)
		{
			auto editor = PNumberEditor();

			auto expectedValue = editor.getNumber();
			auto actualValue = ZERO;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestIsNull)
		{
			auto editor = PNumberEditor();

			auto expectedValue = editor.isNull();
			auto actualValue = true;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAppendNumber)
		{
			auto editor = PNumberEditor();
			editor.appendNumber(2);


			auto expectedValue = editor.getNumber();
			auto actualValue = "02";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAppendZero)
		{
			auto editor = PNumberEditor();
			editor.appendZero();

			auto expectedValue = editor.getNumber();
			auto actualValue = "00";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPopNumberBack)
		{
			auto editor = PNumberEditor();
			editor.appendNumber(2);
			editor.appendNumber(4);
			editor.popNumberBack();

			auto expectedValue = editor.getNumber();
			auto actualValue = "02";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestClear)
		{
			auto editor = PNumberEditor();
			editor.appendNumber(2);
			editor.appendNumber(4);
			editor.clear();

			auto expectedValue = editor.getNumber();
			auto actualValue = ZERO;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestGetNumber)
		{
			auto editor = PNumberEditor();

			auto expectedValue = editor.getNumber();
			auto actualValue = "0";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSetNumber_1)
		{
			auto editor = PNumberEditor();
			editor.setNumber("245");

			auto expectedValue = editor.getNumber();
			auto actualValue = "245";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSetNumber_2)
		{
			auto editor = PNumberEditor();

			auto triggerException = [&]() {editor.setNumber("test"); };

			Assert::ExpectException<std::invalid_argument>(triggerException);
		}
	};
}
