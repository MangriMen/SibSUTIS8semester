#include "pch.h"
#include "CppUnitTest.h"
#include "../lab10/ComplexEditor.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab10Tests
{
	TEST_CLASS(lab10Tests)
	{
	public:
		
		TEST_METHOD(TestComplexEditor)
		{
			auto editor = ComplexEditor();

			auto expectedValue = editor.getNumber();
			auto actualValue = ComplexEditor::ZERO;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestIsNull)
		{
			auto editor = ComplexEditor();

			auto expectedValue = editor.isNull();
			auto actualValue = true;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAppendNumber)
		{
			auto editor = ComplexEditor();
			editor.appendNumber(2);


			auto expectedValue = editor.getNumber();
			auto actualValue = "0+i*02";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestAppendZero)
		{
			auto editor = ComplexEditor();
			editor.appendZero();

			auto expectedValue = editor.getNumber();
			auto actualValue = "0+i*00";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPopNumberBack)
		{
			auto editor = ComplexEditor();
			editor.appendNumber(2);
			editor.appendNumber(4);
			editor.popNumberBack();

			auto expectedValue = editor.getNumber();
			auto actualValue = "0+i*02";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestClear)
		{
			auto editor = ComplexEditor();
			editor.appendNumber(2);
			editor.appendNumber(4);
			editor.clear();

			auto expectedValue = editor.getNumber();
			auto actualValue = ComplexEditor::ZERO;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestGetNumber)
		{
			auto editor = ComplexEditor();

			auto expectedValue = editor.getNumber();
			auto actualValue = "0+i*0";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSetNumber_1)
		{
			auto editor = ComplexEditor();
			editor.setNumber("1+i*2.0");

			auto expectedValue = editor.getNumber();
			auto actualValue = "1+i*2.0";

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSetNumber_2)
		{
			auto editor = ComplexEditor();

			auto triggerException = [&]() {editor.setNumber("test"); };

			Assert::ExpectException<std::invalid_argument>(triggerException);
		}
	};
}
