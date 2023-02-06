#include "pch.h"
#include "CppUnitTest.h"
#include "../lab9/FractionEditor.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab9Tests
{
	TEST_CLASS(lab9Tests)
	{
	public:
		
		TEST_METHOD(TestFractionEditor)
		{
			auto editor = FractionEditor();

			auto expectedValue = editor.getNumber();
			auto actualValue = FractionEditor::ZERO;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestIsNull)
		{
			auto editor = FractionEditor();

			auto expectedValue = editor.isNull();
			auto actualValue = true;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAppendNumber)
		{
			auto editor = FractionEditor();
			editor.appendNumber(2);


			auto expectedValue = editor.getNumber();
			auto actualValue = "0/12";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAppendZero)
		{
			auto editor = FractionEditor();
			editor.appendZero();

			auto expectedValue = editor.getNumber();
			auto actualValue = "0/10";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPopNumberBack)
		{
			auto editor = FractionEditor();
			editor.appendNumber(2);
			editor.appendNumber(4);
			editor.popNumberBack();

			auto expectedValue = editor.getNumber();
			auto actualValue = "0/12";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestClear)
		{
			auto editor = FractionEditor();
			editor.appendNumber(2);
			editor.appendNumber(4);
			editor.clear();

			auto expectedValue = editor.getNumber();
			auto actualValue = FractionEditor::ZERO;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestGetNumber)
		{
			auto editor = FractionEditor();

			auto expectedValue = editor.getNumber();
			auto actualValue = "0/1";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSetNumber_1)
		{
			auto editor = FractionEditor();
			editor.setNumber("2/245");

			auto expectedValue = editor.getNumber();
			auto actualValue = "2/245";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSetNumber_2)
		{
			auto editor = FractionEditor();

			auto triggerException = [&]() {editor.setNumber("test"); };

			Assert::ExpectException<std::invalid_argument>(triggerException);
		}
	};
}
