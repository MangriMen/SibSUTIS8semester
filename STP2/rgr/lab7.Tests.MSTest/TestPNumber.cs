namespace lab7.Tests.MSTest;

[TestClass]
public class TestPNumber
{
    [TestMethod]
    public void TestPNumberConstructor()
    {
        var _ = new PNumber(1, 2, 3);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void TestPNumberSecond()
    {
        var _ = new PNumber(1, 0, 3);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void TestPNumberThird()
    {
        var _ = new PNumber("1, 29, 1");
    }

    [TestMethod]
    public void TestPNumberAdd()
    {
        PNumber FirstValue = new PNumber(1, 2, 3);
        PNumber SecondValue = new PNumber(5, 2, 3);

        PNumber AssertValue = FirstValue + SecondValue;

        PNumber ExpectedValue = new PNumber(6, 2, 3);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void TestPNumberAddSecond()
    {
        PNumber FirstValue = new PNumber(1, 1, 3);
        PNumber SecondValue = new PNumber(5, 2, 3);

        PNumber AssertValue = FirstValue + SecondValue;

        PNumber ExpectedValue = new PNumber(6, 2, 3);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberSub()
    {
        PNumber FirstValue = new PNumber(0, 2, 3);
        PNumber SecondValue = new PNumber(1, 2, 3);

        PNumber AssertValue = FirstValue - SecondValue;

        PNumber ExpectedValue = new PNumber(-1, 2, 3);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberMul()
    {
        PNumber FirstValue = new PNumber(2, 2, 3);
        PNumber SecondValue = new PNumber(1, 2, 3);

        PNumber AssertValue = FirstValue * SecondValue;

        PNumber ExpectedValue = new PNumber(2, 2, 3);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberDiv()
    {
        PNumber FirstValue = new PNumber(2, 2, 3);
        PNumber SecondValue = new PNumber(1, 2, 3);

        PNumber AssertValue = FirstValue / SecondValue;

        PNumber ExpectedValue = new PNumber(2, 2, 3);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberPow()
    {
        PNumber Value = new PNumber(2, 2, 3);

        PNumber AssertValue = PNumber.Pow(Value, 2);

        PNumber ExpectedValue = new PNumber(4, 2, 3);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberRevers()
    {
        PNumber Value = new PNumber(2, 2, 3);

        PNumber AssertValue = PNumber.Revers(Value);

        PNumber ExpectedValue = new PNumber(1.0 / 2, 2, 3);

        Assert.IsTrue(AssertValue == ExpectedValue);
    }

    [TestMethod]
    public void TestPNumberGetNum()
    {
        PNumber Value = new PNumber(2, 2, 3);

        var AssertValue = Value.GetNumber();

        var ExpectedValue = 2;

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberGetString()
    {
        var Value = new PNumber(2, 2, 3);

        var AssertValue = Value.ToString();

        var ExpectedValue = "2, 2, 3";

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberGetBase()
    {
        PNumber Value = new PNumber(2, 2, 3);

        var AssertValue = Value.GetBase();

        var ExpectedValue = 2;

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberGetAccuracy()
    {
        PNumber Value = new PNumber(2, 2, 3);

        var AssertValue = Value.GetAccuracy();

        var ExpectedValue = 3;

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberSetBase()
    {
        PNumber AssertValue = new PNumber(2, 2, 3);

        AssertValue.SetBase(5);
        var ExpectedValue = new PNumber(2, 5, 3);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberSetAccuracy()
    {
        PNumber AssertValue = new PNumber(2, 2, 3);

        AssertValue.SetAccuracy(5);

        var ExpectedValue = new PNumber(2, 2, 5);

        Assert.IsTrue(ExpectedValue == AssertValue);
    }
}
