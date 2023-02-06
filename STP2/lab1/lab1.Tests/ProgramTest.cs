using lab1;
using System.Diagnostics;

namespace lab1Test
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void TestGetEvenProduct()
        {
            var sequence = new float[] { 1.5f, 2.5f, 4.2f, 6.3f };

            var expectedValue = 6.3f;
            var actualValue = Modules.GetEvenProduct(sequence);
            var accuracy = 0.000001f;

            Assert.AreEqual(expectedValue, actualValue, accuracy);
        }

        [TestMethod]
        public void TestShiftSequenceRight()
        {
            var sequence = new float[] { 1, 2, 3, 4, 5 };

            var expectedValue = new float[] { 4, 5, 1, 2, 3 };
            var actualValue = Modules.ShiftSequence(sequence, 2);

            CollectionAssert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestShiftSequenceLeft()
        {
            var sequence = new float[] { 1, 2, 3, 4, 5 };

            var expectedValue = new float[] { 3, 4, 5, 1, 2 };
            var actualValue = Modules.ShiftSequence(sequence, -2);

            CollectionAssert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestGetMaxEvenAndIndex()
        {
            var sequence = new int[] { 1, 2, 3, 4, 5 };

            var expectedValue = (5, 4);
            var actualValue = Modules.GetMaxEvenAndIndex(sequence);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}