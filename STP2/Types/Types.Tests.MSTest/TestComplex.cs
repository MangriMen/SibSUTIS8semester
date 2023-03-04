namespace Types.Tests.MSTest;

[TestClass]
public class TestComplex
{
    [TestMethod]
    public void TestComplexConstructor()
    {
        Complex Value = new Complex(2.0, 3.0);

        var expectedReal = 2.0;
        var expectedImag = 3.0;

        var actualReal = Value.Real;
        var actualImag = Value.Image;

        Assert.AreEqual(actualReal, expectedReal);
        Assert.AreEqual(actualImag, expectedImag);
    }

    [TestMethod]
    public void TestCompexConstructorFromString()
    {
        var Value = new Complex("2+i*3");

        var expectedReal = 2.0;
        var expectedImag = 3.0;

        var actualReal = Value.Real;
        var actualImag = Value.Image;

        Assert.AreEqual(expectedReal, actualReal);
        Assert.AreEqual(expectedImag, actualImag);
    }

    [TestMethod]
    public void TestComplexOperatorPlus()
    {
        Complex FirstValue = new Complex(2, 3);
        Complex SecondValue = new Complex(2, 3);

        Complex AssertValue = FirstValue + SecondValue;

        Complex ExpectedValue = new Complex(4, 6);

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestComplexOperatorMinus()
    {
        var FirstValue = new Complex(2, 3);
        var SecondValue = new Complex(5, 5);

        var actualValue = FirstValue - SecondValue;

        var expectedValue = new Complex(-3, -2);

        Assert.AreEqual(expectedValue, actualValue);
    }

    [TestMethod]
    public void TestComplexOperatorMultiply()
    {
        Complex FirstValue = new Complex(2, 3);
        Complex SecondValue = new Complex(5, 5);

        Complex AssertValue = FirstValue * SecondValue;

        Complex ExpectedValue = new Complex(-5, 25);

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestComplexOperatorDivide()
    {
        var FirstValue = new Complex(2, 3);
        var SecondValue = new Complex(5, 5);

        var AssertValue = FirstValue / SecondValue;

        var ExpectedValue = new Complex(0.5, 0.1);

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestComplexOperatorEquals()
    {
        Complex FirstValue = new Complex(2, 3);
        Complex SecondValue = new Complex(2, 3);

        var AssertValue = FirstValue == SecondValue ? true : false;

        Assert.IsTrue(AssertValue);
    }

    [TestMethod]
    public void TestComplexOperatorNotEquals()
    {
        Complex FirstValue = new Complex(2, 3);
        Complex SecondValue = new Complex(3, 3);

        var AssertValue = FirstValue != SecondValue ? true : false;

        Assert.IsTrue(AssertValue);
    }

    [TestMethod]
    public void TestComplexPow()
    {
        var Value = new Complex(32, 16);

        var AssertValue = Value.Pow(2);

        var ExpectedValue = new Complex(768, 1024);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestComplexAbs()
    {
        var Value = new Complex(2, 2);

        var AssertValue = Complex.Abs(Value);

        var ExpectedValue = 2.8284271247461903;

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestComplexAngleRadians()
    {
        var Value = new Complex(2, 2);

        var AssertValue = Complex.AngleRadians(Value);

        var ExpectedValue = 0.78539816339744828;

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestComplexAngleRadiansWithUnderZeroOneVars()
    {
        var Value = new Complex(-2, 2);

        var AssertValue = Complex.AngleRadians(Value);

        var ExpectedValue = 2.356194490192345;

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestComplexReal()
    {
        var Value = new Complex(2, 2);

        var AssertValue = Value.Real;

        var ExpectedValue = 2;

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestComplexImage()
    {
        var Value = new Complex(2, 2);

        var AssertValue = Value.Image;

        var ExpectedValue = 2;

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestComplexToString()
    {
        var Value = new Complex(2, 2);

        var AssertValue = Value.ToString();

        var ExpectedValue = "2+i*2";

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestComplexRoot()
    {
        var Value = new Complex(3840, 2048);

        var AssertValue = Value.Root(2);

        var ExpectedValue = new Complex(64, 16);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }
}
