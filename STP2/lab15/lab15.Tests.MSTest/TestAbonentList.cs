using System.Diagnostics;
using lab15.Models;

namespace lab15.Tests.MSTest;

[TestClass]
public class TestAbonentList
{
    public TestContext? TestContext
    {
        get; set;
    }
    private static AbonentList? abonents;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        Debug.WriteLine("ClassInitialize");
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Debug.WriteLine("ClassCleanup");
    }

    [TestInitialize]
    public void TestInitialize()
    {
        abonents = new($"{TestContext?.TestName}.list");
        Debug.WriteLine("TestInitialize");
    }

    [TestCleanup]
    public void TestCleanup()
    {
        abonents?.Clear();
        abonents = null;
        Debug.WriteLine("TestCleanup");
    }

    [TestMethod]
    public void TestGetAbonents()
    {
        Assert.IsNotNull(abonents);

        abonents.Add("Kyle", 24812);
        abonents.Add("Ale", 15456);

        var abonentsList = abonents.Abonents;

        Assert.IsNotNull(abonentsList);
        Assert.AreEqual(2, abonentsList.Count);
        Assert.AreEqual(24812, abonentsList["Kyle"].Single()[0]);
        Assert.AreEqual(15456, abonentsList["Ale"].Single()[0]);
    }

    [TestMethod]
    public void TestClear()
    {
        Assert.IsNotNull(abonents);

        abonents.Add("Kyle", 24812);
        abonents.Add("Ale", 15456);

        abonents.Clear();

        var abonentsList = abonents.Abonents;

        Assert.IsNotNull(abonentsList);
        Assert.AreEqual(0, abonentsList.Count);
    }

    [TestMethod]
    public void TestFind()
    {
        Assert.IsNotNull(abonents);

        abonents.Add("Kyle", 24812);
        abonents.Add("Ale", 15456);

        var foundList = abonents.Find("Ale");

        Assert.IsNotNull(foundList);
        Assert.AreEqual(1, foundList.Count);
        Assert.AreEqual(15456, foundList["Ale"].Single()[0]);
    }
}
