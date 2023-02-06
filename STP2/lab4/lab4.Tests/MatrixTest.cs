namespace lab4.Tests
{
    [TestClass]
    public class MatrixTest
    {
        [TestMethod]
        [ExpectedException(typeof(MyException))]
        public void TestConstructorZeroSize()
        {
            _ = new Matrix(0, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(MyException))]
        public void TestConstructorNegativeSize()
        {
            _ = new Matrix(2, -1);
        }

        [TestMethod]
        public void TestConstructorDefault()
        {
            _ = new Matrix(2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(MyException))]
        public void TestIndexOperatorGetBigI()
        {
            Matrix testMatrix = new(2, 2);
            int _ = testMatrix[3, 1];
        }

        [TestMethod]
        [ExpectedException(typeof(MyException))]
        public void TestIndexOperatorSetBigI()
        {
            Matrix testMatrix = new(2, 2);
            testMatrix[3, 1] = 2;
        }

        [TestMethod]
        [ExpectedException(typeof(MyException))]
        public void TestIndexOperatorGetBigJ()
        {
            Matrix testMatrix = new(2, 2);
            int _ = testMatrix[1, 3];
        }

        [TestMethod]
        [ExpectedException(typeof(MyException))]
        public void TestIndexOperatorSetBigJ()
        {
            Matrix testMatrix = new(2, 2);
            testMatrix[1, 3] = 2;
        }

        [TestMethod]
        public void TestIndexOperatorGet()
        {
            Matrix testMatrix = new(2, 2);
            int _ = testMatrix[1, 1];
        }

        [TestMethod]
        public void TestIndexOperatorSet()
        {
            Matrix testMatrix = new(2, 2);
            testMatrix[1, 1] = 2;
        }

        [TestMethod]
        public void TestSumOperator()
        {
            Matrix firstMatrix = new(new int[,] { { 1, 1 }, { 1, 1 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 2, 2 }, { 2, 2 } }, 2, 2);

            Matrix expectedValue = new(new int[,] { { 3, 3 }, { 3, 3 } }, 2, 2);
            Matrix actualValue = firstMatrix + secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestSumOperatorNegativeTerms()
        {
            Matrix firstMatrix = new(new int[,] { { -7, 3 }, { -6, -1 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { -2, 2 }, { -2, -2 } }, 2, 2);

            Matrix expectedValue = new(new int[,] { { -9, 5 }, { -8, -3 } }, 2, 2);
            Matrix actualValue = firstMatrix + secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestSumOperatorZeroes()
        {
            Matrix firstMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);

            Matrix expectedValue = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);
            Matrix actualValue = firstMatrix + secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestMinusOperator()
        {
            Matrix firstMatrix = new(new int[,] { { 2, 2 }, { 2, 2 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 1, 1 }, { 1, 1 } }, 2, 2);

            Matrix expectedValue = new(new int[,] { { 1, 1 }, { 1, 1 } }, 2, 2);
            Matrix actualValue = firstMatrix - secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestMinusOperatorNegativeResult()
        {
            Matrix firstMatrix = new((new int[,] { { 0, 0 }, { 0, 0 } }), 2, 2);
            Matrix secondMatrix = new((new int[,] { { 1, 1 }, { 1, 1 } }), 2, 2);

            Matrix expectedValue = new((new int[,] { { -1, -1 }, { -1, -1 } }), 2, 2);
            Matrix actualValue = firstMatrix - secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestMinusOperatorZeroes()
        {
            Matrix firstMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);

            Matrix expectedValue = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);
            Matrix actualValue = firstMatrix - secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestMultiplyOperator()
        {
            Matrix firstMatrix = new(new int[,] { { 3, 3 }, { 3, 3 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 2, 2 }, { 2, 2 } }, 2, 2);

            Matrix expectedValue = new(new int[,] { { 12, 12 }, { 12, 12 } }, 2, 2);
            Matrix actualValue = firstMatrix * secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestMultiplyOperatorNegative()
        {
            Matrix firstMatrix = new(new int[,] { { 3, 3 }, { 3, -3 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { -2, 2 }, { 2, 2 } }, 2, 2);

            Matrix expectedValue = new(new int[,] { { 0, 12 }, { -12, 0 } }, 2, 2);
            Matrix actualValue = firstMatrix * secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestMultiplyOperatorZeroes()
        {
            Matrix firstMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);

            Matrix expectedValue = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);
            Matrix actualValue = firstMatrix * secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestEqualityOperator()
        {
            Matrix firstMatrix = new(new int[,] { { 3, 3 }, { 3, 3 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 3, 3 }, { 3, 3 } }, 2, 2);

            bool expectedValue = true;
            bool actualValue = firstMatrix == secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestEqualityOperatorNotEqual()
        {
            Matrix firstMatrix = new(new int[,] { { 3, 3 }, { 3, 3 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 2, 2 }, { 2, 2 } }, 2, 2);

            bool expectedValue = true;
            bool actualValue = firstMatrix == secondMatrix;

            Assert.AreNotEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestEqualityOperatorZeroes()
        {
            Matrix firstMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);

            bool expectedValue = true;
            bool actualValue = firstMatrix == secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }


        [TestMethod]
        public void TestNotEqualOperator()
        {
            Matrix firstMatrix = new(new int[,] { { 3, 3 }, { 3, 3 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 3, 3 }, { 3, 3 } }, 2, 2);

            bool expectedValue = true;
            bool actualValue = firstMatrix != secondMatrix;

            Assert.AreNotEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestNotEqualOperatorNotEqual()
        {
            Matrix firstMatrix = new(new int[,] { { 3, 3 }, { 3, 3 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 2, 2 }, { 2, 2 } }, 2, 2);

            bool expectedValue = true;
            bool actualValue = firstMatrix != secondMatrix;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestNotEqualOperatorZeroes()
        {
            Matrix firstMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);
            Matrix secondMatrix = new(new int[,] { { 0, 0 }, { 0, 0 } }, 2, 2);

            bool expectedValue = true;
            bool actualValue = firstMatrix != secondMatrix;

            Assert.AreNotEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestTranspose2x2()
        {
            Matrix testMatrix = new(new int[,] { { 1, 2 }, { 3, 4 } }, 2, 2);

            Matrix expectedValue = new(new int[,] { { 1, 3 }, { 2, 4 } }, 2, 2);
            Matrix actualValue = testMatrix.Tranpose();

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestTranspose3x3()
        {
            Matrix testMatrix = new(new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }, 3, 3);

            Matrix expectedValue = new(new int[,] { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } }, 3, 3);
            Matrix actualValue = testMatrix.Tranpose();

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestMin()
        {
            Matrix testMatrix = new(new int[,] { { 1, 2 }, { 3, 4 } }, 2, 2);

            int expectedValue = 1;
            int actualValue = testMatrix.Min();

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestMinZero()
        {
            Matrix testMatrix = new(new int[,] { { 1, 2 }, { 0, 4 } }, 2, 2);

            int expectedValue = 0;
            int actualValue = testMatrix.Min();

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestMinNegative()
        {
            Matrix testMatrix = new(new int[,] { { -651, 2 }, { 3, 4 } }, 2, 2);

            int expectedValue = -651;
            int actualValue = testMatrix.Min();

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestToString() {
            string expectedValue = "{{1,2},{3,4}}";
            Matrix actualValue = new(new int[,] { { 1, 2 }, { 3, 4 } }, 2, 2);

            Assert.AreEqual(expectedValue, actualValue.ToString());
            Assert.IsInstanceOfType(actualValue.ToString(), typeof(string));
        }
    }
}
