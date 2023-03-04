using Types;

namespace Calculator.Tests.MSTest;

[TestClass]
public class TestMemory
{
    [TestMethod]
    public void TestConstructor()
    {
        var memory = new Memory<PNumber>();

        var expected = 0;
        var actual = memory.Number;

        var expectedState = false;
        var actualState = memory.IsOn;

        Assert.AreEqual(expected, actual);
        Assert.AreEqual(expectedState, actualState);
    }

    [TestMethod]
    public void TestGetNumber()
    {
        var memory = new Memory<PNumber>();
        memory.Store(2);

        var expected = 2;
        var actual = memory.Number;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestStore()
    {
        var memory = new Memory<PNumber>();
        memory.Store(2);

        var expected = 2;
        var actual = memory.Number;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestRead()
    {
        var memory = new Memory<PNumber>();
        memory.Store(2);

        var expected = 2;
        var actual = memory.Read();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestAdd()
    {
        var memory = new Memory<PNumber>();
        memory.Store(2);
        memory.Add(2);

        var expected = 4;
        var actual = memory.Number;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestSubtract()
    {
        var memory = new Memory<PNumber>();
        memory.Store(2);
        memory.Subtract(2);

        var expected = 0;
        var actual = memory.Number;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestClear()
    {
        var memory = new Memory<PNumber>();
        memory.Store(2);
        memory.Clear();

        var expected = 0;
        var actual = memory.Number;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestGetStateOn()
    {
        var memory = new Memory<PNumber>();
        memory.Store(2);

        var expected = true;
        var actual = memory.IsOn;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestGetStateOff()
    {
        var memory = new Memory<PNumber>();
        memory.Store(2);
        memory.Clear();

        var expected = false;
        var actual = memory.IsOn;

        Assert.AreEqual(expected, actual);
    }
}
