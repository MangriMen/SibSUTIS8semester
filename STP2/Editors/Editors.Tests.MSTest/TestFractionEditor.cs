namespace Editors.Tests.MSTest;

[TestClass]
public class TestFractionEditor
{
    [TestMethod]
    public void TestFractionEditorIsNull()
    {
        var Value = new FractionEditor();

        var AssertValue = Value.IsNull;

        var ExpectedValue = true;

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestPNumberAddDigit()
    {
        var Value = new FractionEditor();
        Value.AddDigit(5);

        var AssertValue = Value.Number;

        var ExpectedValue = "5";

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestPNumberBackspace()
    {
        var Value = new FractionEditor();
        Value.AddDigit(5);
        Value.AddDigit(8);
        Value.Backspace();

        var AssertValue = Value.Number;

        var ExpectedValue = "5";

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestPNumberClear()
    {
        var Value = new FractionEditor();
        Value.AddDigit(5);
        Value.AddDigit(8);
        Value.Clear();

        var AssertValue = Value.Number;

        var ExpectedValue = string.Empty;

        Assert.AreEqual(ExpectedValue, AssertValue);
    }

    [TestMethod]
    public void TestPNumberToggleNegative()
    {
        var Value = new FractionEditor();
        Value.AddDigit(12);
        Value.ToggleNegative();

        var AssertValue = Value.Number;

        var ExpectedValue = "-12";

        Assert.AreEqual(ExpectedValue, AssertValue);
    }
}
