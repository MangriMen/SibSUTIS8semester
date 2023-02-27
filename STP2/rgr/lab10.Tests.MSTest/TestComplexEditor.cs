namespace lab10.Tests.MSTest;

[TestClass]
public class TestComplexEditor
{
    [TestMethod]
    public void TestComplexEditorIsNull()
    {
        var Value = new ComplexEditor();

        var AssertValue = Value.IsNull();

        var ExpectedValue = true;

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestPNumberAppendNumber()
    {
        var Value = new ComplexEditor();
        Value.AppendNumber("5");

        var AssertValue = Value.CurrentNumber;

        var ExpectedValue = "5";

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberPopNumberBack()
    {
        var Value = new ComplexEditor();
        Value.AppendNumber("5");
        Value.AppendNumber("8");
        Value.PopNumber();

        var AssertValue = Value.CurrentNumber;

        var ExpectedValue = "5";

        Assert.IsTrue(ExpectedValue == AssertValue);
    }

    [TestMethod]
    public void TestPNumberClear()
    {
        var Value = new ComplexEditor();
        Value.AppendNumber("5");
        Value.AppendNumber("8");
        Value.Clear();

        var AssertValue = Value.CurrentNumber;

        var ExpectedValue = "0+i*0";

        Assert.IsTrue(ExpectedValue == AssertValue);
    }


    [TestMethod]
    public void TestPNumberToggleNegative()
    {
        var Value = new ComplexEditor();
        Value.AppendNumber("12");
        Value.ToggleNegative();

        var AssertValue = Value.CurrentNumber;

        var ExpectedValue = "-12";

        Assert.IsTrue(ExpectedValue == AssertValue);
    }
}