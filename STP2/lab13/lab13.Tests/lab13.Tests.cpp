#include "pch.h"
#include "CppUnitTest.h"
#include "../lab13/MySet.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace lab13Tests
{
	TEST_CLASS(lab13Tests)
	{
	public:

		TEST_METHOD(TestMySet)
		{
			MySet<int> a;
		}

		TEST_METHOD(TestClear)
		{
			auto mySet = MySet<int>();

			mySet.push_back(1);
			mySet.push_back(2);
			mySet.push_back(3);

			mySet.clear();

			auto actualValue = mySet.empty();
			auto expectedValue = true;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestPushBack)
		{
			auto mySet = MySet<int>();

			mySet.push_back(1);

			auto actualValue = mySet[0];
			auto expectedValue = 1;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestRemove)
		{
			auto mySet = MySet<int>();

			mySet.push_back(1);
			mySet.push_back(2);

			mySet.remove(1);

			auto actualValue = mySet.size();
			auto expectedValue = 1;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestContains)
		{
			auto mySet = MySet<int>();

			mySet.push_back(1);

			auto actualValue = mySet.contains(1);
			auto expectedValue = true;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestUnion)
		{
			auto a = MySet<int>();
			auto b = MySet<int>();

			a.push_back(1);
			a.push_back(2);
			a.push_back(3);

			b.push_back(3);
			b.push_back(4);
			b.push_back(5);

			auto uni = a.getUnion(b);

			auto actualValue = uni[0];
			auto expectedValue = 3;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestConflux)
		{
			auto a = MySet<int>();
			auto b = MySet<int>();

			a.push_back(1);
			a.push_back(2);
			a.push_back(3);

			b.push_back(3);
			b.push_back(4);
			b.push_back(5);

			auto uni = a.getConflux(b);

			auto actualValue = uni.size();
			auto expectedValue = 5;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSub)
		{
			auto a = MySet<int>();
			auto b = MySet<int>();

			a.push_back(1);
			a.push_back(2);
			a.push_back(3);

			b.push_back(3);
			b.push_back(4);
			b.push_back(5);

			auto uni = b.getSub(a);

			auto actualValue = uni.size();
			auto expectedValue = 2;

			Assert::IsTrue(expectedValue == actualValue);
		}

		TEST_METHOD(TestSize)
		{
			auto mySet = MySet<int>();

			mySet.push_back(1);
			mySet.push_back(2);
			mySet.push_back(3);

			auto actualValue = mySet.size();
			auto expectedValue = 3;

			Assert::IsTrue(expectedValue == actualValue);
		}
	};
}
