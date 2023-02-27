using lab5;

namespace lab12.Tests.MSTest;

[TestClass]
public class TestProcessor
{
    [TestMethod]
    public void TestConstructor()
    {
        var processor = new Processor<Fraction>();

        var actualLeftOperand = processor.LeftOperand;
        var actualRightOperand = processor.RightOperand;
        var actualOperation = processor.LastOperation;

        var expectedLeftOperand = new Fraction();
        var expectedRightOperand = new Fraction();
        var expectedOperation = Processor<Fraction>.Operation.None;

        Assert.AreEqual(expectedLeftOperand, actualLeftOperand);
        Assert.AreEqual(expectedRightOperand, actualRightOperand);
        Assert.AreEqual(expectedOperation, actualOperation);
    }

    [TestMethod]
    public void TestPerformOperationPlus()
    {
        var processor = new Processor<Fraction>();

        var leftOperand = new Fraction("2");
        var rightOperand = new Fraction("4");

        processor.LeftOperand = leftOperand;
        processor.RightOperand = rightOperand;
        processor.LastOperation = Processor<Fraction>.Operation.Plus;

        processor.PerformOperation();

        var expected = new Fraction("6");
        var actual = processor.LeftOperand;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestPerformOperationMinus()
    {
        var processor = new Processor<Fraction>();

        var leftOperand = new Fraction("2");
        var rightOperand = new Fraction("4");

        processor.LeftOperand = leftOperand;
        processor.RightOperand = rightOperand;
        processor.LastOperation = Processor<Fraction>.Operation.Minus;

        processor.PerformOperation();

        var expected = new Fraction("-2");
        var actual = processor.LeftOperand;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestPerformOperationMultiply()
    {
        var processor = new Processor<Fraction>();

        var leftOperand = new Fraction("2");
        var rightOperand = new Fraction("4");

        processor.LeftOperand = leftOperand;
        processor.RightOperand = rightOperand;
        processor.LastOperation = Processor<Fraction>.Operation.Multiply;

        processor.PerformOperation();

        var expected = new Fraction("8");
        var actual = processor.LeftOperand;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestPerformOperationDivide()
    {
        var processor = new Processor<Fraction>();

        var leftOperand = new Fraction("2");
        var rightOperand = new Fraction("4");

        processor.LeftOperand = leftOperand;
        processor.RightOperand = rightOperand;
        processor.LastOperation = Processor<Fraction>.Operation.Divide;

        processor.PerformOperation();

        var expected = new Fraction("1/2");
        var actual = processor.LeftOperand;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestPerfomFunctionModule()
    {
        var processor = new Processor<Fraction>();

        var leftOperand = new Fraction("4");
        var rightOperand = new Fraction("0");

        processor.LeftOperand = leftOperand;
        processor.RightOperand = rightOperand;

        processor.PerformFunction(Processor<Fraction>.Function.Module);

        var expected = new Fraction("1/25");
        var actual = processor.LeftOperand;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestPerfomFunctionReciprocal()
    {
        var processor = new Processor<Fraction>();

        var leftOperand = new Fraction("4");
        var rightOperand = new Fraction("0");

        processor.LeftOperand = leftOperand;
        processor.RightOperand = rightOperand;

        processor.PerformFunction(Processor<Fraction>.Function.Reciprocal);

        var expected = new Fraction("1/4");
        var actual = processor.LeftOperand;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestPerfomFunctionSqr()
    {
        var processor = new Processor<Fraction>();

        var leftOperand = new Fraction("4");
        var rightOperand = new Fraction("0");

        processor.LeftOperand = leftOperand;
        processor.RightOperand = rightOperand;

        processor.PerformFunction(Processor<Fraction>.Function.Sqr);

        var expected = new Fraction("16");
        var actual = processor.LeftOperand;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestPerfomFunctionSqrt()
    {
        var processor = new Processor<Fraction>();

        var leftOperand = new Fraction("4");
        var rightOperand = new Fraction("0");

        processor.LeftOperand = leftOperand;
        processor.RightOperand = rightOperand;

        processor.PerformFunction(Processor<Fraction>.Function.Sqrt);

        var expected = new Fraction("2");
        var actual = processor.LeftOperand;

        Assert.AreEqual(expected, actual);
    }
}