#include "pch.h"
#include "CppUnitTest.h"
#include "../lab12/Processor.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab12Tests
{
	TEST_CLASS(lab12Tests)
	{
	public:

		TEST_METHOD(TestProcessor)
		{
			auto proc = Processor<int>();

			Assert::IsTrue(0 == proc.getLeftOperand());
			Assert::IsTrue(0 == proc.getRightOperand());
			Assert::IsTrue(Processor<int>::Operation::None == proc.getState());
		}

		TEST_METHOD(TestReset)
		{
			auto proc = Processor<int>();

			proc.setLeftOperand(2);
			proc.setLeftOperand(2);
			proc.setState(Processor<int>::Operation::Add);

			proc.resetOperation();

			Assert::IsTrue(Processor<int>::Operation::None == proc.getState());

			proc.setState(Processor<int>::Operation::Add);

			proc.reset();

			Assert::IsTrue(int() == proc.getLeftOperand());
			Assert::IsTrue(int() == proc.getRightOperand());
			Assert::IsTrue(Processor<int>::Operation::None == proc.getState());
		}

		TEST_METHOD(TestPerformOperation_Add) {
			auto proc = Processor<int>();
			proc.setLeftOperand(2);
			proc.setRightOperand(3);

			proc.setState(Processor<int>::Operation::Add);

			proc.performOperation();

			auto expectedValue = proc.getLeftOperand();
			auto actualValue = 5;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPerformOperation_Sub) {
			auto proc = Processor<int>();
			proc.setLeftOperand(3);
			proc.setRightOperand(2);

			proc.setState(Processor<int>::Operation::Sub);

			proc.performOperation();

			auto expectedValue = proc.getLeftOperand();
			auto actualValue = -1;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPerformOperation_Mul) {
			auto proc = Processor<int>();
			proc.setLeftOperand(2);
			proc.setRightOperand(3);

			proc.setState(Processor<int>::Operation::Mul);

			proc.performOperation();

			auto expectedValue = proc.getLeftOperand();
			auto actualValue = 6;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPerformOperation_Dvd) {
			auto proc = Processor<int>();
			proc.setLeftOperand(5);
			proc.setRightOperand(10);

			proc.setState(Processor<int>::Operation::Dvd);

			proc.performOperation();

			auto expectedValue = proc.getLeftOperand();
			auto actualValue = 2;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestGetSetLeftOperand)
		{
			auto proc = Processor<int>();

			proc.setLeftOperand(10);
			auto operand = proc.getLeftOperand();

			Assert::IsTrue(10 == operand);
		}

		TEST_METHOD(TestGetSetRightOperand)
		{
			auto proc = Processor<int>();

			proc.setRightOperand(10);
			auto operand = proc.getRightOperand();

			Assert::IsTrue(10 == operand);
		}

		TEST_METHOD(TestGetSetState)
		{
			auto proc = Processor<int>();

			proc.setState(Processor<int>::Operation::Add);
			auto operand = proc.getState();

			Assert::IsTrue(Processor<int>::Operation::Add == operand);
		}
	};
}
