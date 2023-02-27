namespace lab5.Tests.MSTest;

[TestClass]
public class TestFraction
{
    [TestMethod]
    public void TestFractionConstructor()
    {
        Fraction Value = new Fraction(5, 3);

        var AssertValue1 = Value.GetNominator();
        var AssertValue2 = Value.GetDenominator();

        var ExpectedValue1 = 5;
        var ExpectedValue2 = 3;

        Assert.AreEqual(ExpectedValue1, AssertValue1);
        Assert.AreEqual(ExpectedValue2, AssertValue2);
    }


    [TestMethod]
    public void TestFractionFromString()
    {
        var Value = "2/3";
        Fraction FromStringToFraction = new Fraction(Value);

        var AssertValue1 = FromStringToFraction.GetNominator();
        var AssertValue2 = FromStringToFraction.GetDenominator();

        var ExpectedValue1 = 2;
        var ExpectedValue2 = 3;

        Assert.AreEqual(ExpectedValue1, AssertValue1);
        Assert.AreEqual(ExpectedValue2, AssertValue2);
    }

    [TestMethod]
    public void TestFractionFromStringWithGCD()
    {
        var Value = "12/8";
        Fraction FromStringToFraction = new Fraction(Value);

        var AssertValue1 = FromStringToFraction.GetNominator();
        var AssertValue2 = FromStringToFraction.GetDenominator();

        var ExpectedValue1 = 3;
        var ExpectedValue2 = 2;

        Assert.AreEqual(ExpectedValue1, AssertValue1);
        Assert.AreEqual(ExpectedValue2, AssertValue2);
    }

    [TestMethod]
    public void TestFractionReduce()
    {
        Fraction Value = new Fraction(12, 8);

        var AssertValue1 = Value.GetNominator();
        var AssertValue2 = Value.GetDenominator();

        var ExpectedValue1 = 3;
        var ExpectedValue2 = 2;

        Assert.AreEqual(ExpectedValue1, AssertValue1);
        Assert.AreEqual(ExpectedValue2, AssertValue2);
    }


    [TestMethod]
    public void TestFractionOperatorMinus()
    {
        Fraction FirstValue = new Fraction(2, 7);
        Fraction SecondValue = new Fraction(1, 7);

        Fraction AssertValue = FirstValue - SecondValue;

        Fraction ExpectedValue = new Fraction(1, 7);

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionOperatorPlus()
    {
        Fraction FirstValue = new Fraction(2, 7);
        Fraction SecondValue = new Fraction(1, 7);

        Fraction AssertValue = FirstValue + SecondValue;

        Fraction ExpectedValue = new Fraction(3, 7);

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionOperatorMultiply()
    {
        Fraction FirstValue = new Fraction(2, 7);
        Fraction SecondValue = new Fraction(1, 7);

        Fraction AssertValue = FirstValue * SecondValue;

        Fraction ExpectedValue = new Fraction(2, 49);

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionOperatorDivide()
    {
        Fraction FirstValue = new Fraction(2, 7);
        Fraction SecondValue = new Fraction(1, 4);

        Fraction AssertValue = FirstValue / SecondValue;

        Fraction ExpectedValue = new Fraction(8, 7);

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionPow()
    {
        Fraction Value = new Fraction(4, 5);

        Fraction AssertValue = Fraction.Pow(Value, 2);

        Fraction ExpectedValue = new Fraction(16, 25);

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionReverse()
    {
        Fraction Value = new Fraction(4, 5);

        Fraction AssertValue = Fraction.Reverse(Value);

        Fraction ExpectedValue = new Fraction(5, 4);

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionOperatorAssertTrue()
    {
        Fraction FirstValue = new Fraction(4, 5);
        Fraction SecondValue = new Fraction(4, 5);

        var isTrue = FirstValue == SecondValue ? true : false;

        Assert.IsTrue(isTrue);
    }

    [TestMethod]
    public void TestFractionOperatorAssertFalse()
    {
        var FirstValue = new Fraction(4, 5);
        var SecondValue = new Fraction(3, 5);

        var isTrue = FirstValue == SecondValue;

        Assert.IsTrue(!isTrue);
    }

    [TestMethod]
    public void TestFractionOperatorNotAssertTrue()
    {
        var FirstValue = new Fraction(4, 5);
        var SecondValue = new Fraction(3, 5);

        var isTrue = FirstValue != SecondValue;

        Assert.IsTrue(isTrue);
    }

    [TestMethod]
    public void TestFractionOperatorNotAssertFalse()
    {
        Fraction FirstValue = new Fraction(4, 5);
        Fraction SecondValue = new Fraction(3, 5);

        var isTrue = FirstValue == SecondValue;

        Assert.IsTrue(!isTrue);
    }

    [TestMethod]
    public void TestFractionGetNominator()
    {
        Fraction Value = new Fraction(2, 3);

        var AssertValue = Value.GetNominator();

        var ExpectedValue = 2;

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionGetDenominator()
    {
        Fraction Value = new Fraction(2, 3);

        var AssertValue = Value.GetDenominator();

        var ExpectedValue = 3;

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionGetNominatorString()
    {
        Fraction Value = new Fraction(2, 3);

        var AssertValue = Value.GetNominatorString();

        var ExpectedValue = "2";

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionGetDenominatorString()
    {
        Fraction Value = new Fraction(2, 3);

        var AssertValue = Value.GetDenominatorString();

        var ExpectedValue = "3";

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestFractionToString()
    {
        var Value = new Fraction(2, 3);

        var AssertValue = Value.ToFractionString();

        var ExpectedValue = "2/3";

        Assert.AreEqual(ExpectedValue, AssertValue);
    }
}