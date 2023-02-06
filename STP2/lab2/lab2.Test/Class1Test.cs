namespace lab2.Test
{
    [TestClass]
    public class Class1Test
    {
        [TestMethod]
        public void TestMax()
        {
            var expectedValue = (50, 38);
            var actualValue = Class1.SortDecrement(38, 50);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestGetEvenProductMatrix()
        {
            var array = new float[,] { { 1, 8 }, { 3, 4 }, { 5, 6 } };

            var expectedValue = 192;
            var actualValue = Class1.GetEvenProductMatrix(array);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestGetEvenProductMatrixEmptyMatrix()
        {
            var array = new float[,] { };

            var expectedValue = 0;
            var actualValue = Class1.GetEvenProductMatrix(array);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestGetEvenSumLeftTopTriangleMatrix()
        {
            var array = new float[,] { { 1, 28, 3 }, { 5, 4, 6 }, { 7, 8, 9 } };

            var expectedValue = 32;
            var actualValue = Class1.GetEvenSumLeftTopTriangleMatrix(array);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestGetEvenSumLeftTopTriangleMatrixEmptyMatrix()
        {
            var array = new float[,] { };

            var expectedValue = 0;
            var actualValue = Class1.GetEvenSumLeftTopTriangleMatrix(array);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}