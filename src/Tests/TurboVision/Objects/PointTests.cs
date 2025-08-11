using TurboVision.Objects;

namespace Tests.TurboVision.Objects;

[TestClass]
public sealed class PointTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[Point.PlacementSize];
        using var point = new Point(bytes);
        Assert.AreEqual(0, point.X);
        Assert.AreEqual(0, point.Y);
        point.X = 1;
        point.Y = 2;
        Assert.AreEqual(1, point.X);
        Assert.AreEqual(2, point.Y);
    }

    [TestMethod]
    public void Test_New()
    {
        using var point = new Point();
        Assert.AreEqual(0, point.X);
        Assert.AreEqual(0, point.Y);
        point.X = 1;
        point.Y = 2;
        Assert.AreEqual(1, point.X);
        Assert.AreEqual(2, point.Y);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new Point { X = 1, Y = 2 };
        using var b = new Point { X = 1, Y = 2 };
        using var c = new Point { X = 3, Y = 4 };
        Assert.IsTrue(a.Equals(b));
        Assert.IsFalse(a.Equals(c));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new Point { X = 1, Y = 2 };
        using var b = new Point { X = 1, Y = 2 };
        using var c = new Point { X = 3, Y = 4 };
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), c.GetHashCode());
    }

    [TestMethod]
    public void Test_Add()
    {
        using var a = new Point { X = 1, Y = 2 };
        using var b = new Point { X = 3, Y = 4 };
        a.Add(b);
        Assert.AreEqual(4, a.X);
        Assert.AreEqual(6, a.Y);
    }

    [TestMethod]
    public void Test_Subtract()
    {
        using var a = new Point { X = 3, Y = 4 };
        using var b = new Point { X = 2, Y = 1 };
        a.Subtract(b);
        Assert.AreEqual(1, a.X);
        Assert.AreEqual(3, a.Y);
    }

    [TestMethod]
    public void Test_Add_Static()
    {
        using var a = new Point { X = 1, Y = 2 };
        using var b = new Point { X = 3, Y = 4 };
        using var c = new Point();
        Point.Add(a, b, c);
        Assert.AreEqual(4, c.X);
        Assert.AreEqual(6, c.Y);
    }

    [TestMethod]
    public void Test_Subtract_Static()
    {
        using var a = new Point { X = 3, Y = 4 };
        using var b = new Point { X = 2, Y = 1 };
        using var c = new Point();
        Point.Subtract(a, b, c);
    }

    [TestMethod]
    public void Test_X()
    {
        using var point = new Point { X = 1, Y = 2 };
        Assert.AreEqual(1, point.X);
        point.X = 3;
        Assert.AreEqual(3, point.X);
    }

    [TestMethod]
    public void Test_Y()
    {
        using var point = new Point { X = 1, Y = 2 };
        Assert.AreEqual(2, point.Y);
        point.Y = 4;
        Assert.AreEqual(4, point.Y);
    }
}
