using TurboVision.Objects;

namespace Tests.TurboVision.Objects;

[TestClass]
public sealed class RectTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[Rect.PlacementSize];
        using var rect = new Rect(bytes);
    }

    [TestMethod]
    public void Test_New()
    {
        using var rect = new Rect();
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new Rect();
        using var b = new Rect();
        using var pointA = new Point { X = 1, Y = 2 };
        using var pointB = new Point { X = 3, Y = 4 };
        using var c = new Rect();

        a.SetA(pointA);
        a.SetB(pointB);
        c.SetA(pointA);
        c.SetB(pointB);

        Assert.IsTrue(a.Equals(c));
        Assert.IsFalse(a.Equals(b));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new Rect();
        using var b = new Rect();
        using var pointA = new Point { X = 1, Y = 2 };
        using var pointB = new Point { X = 3, Y = 4 };
        using var c = new Rect();

        a.SetA(pointA);
        a.SetB(pointB);
        c.SetA(pointA);
        c.SetB(pointB);

        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Test_Move()
    {
        using var rect = new Rect();
        using var pointA = new Point { X = 1, Y = 2 };
        using var pointB = new Point { X = 5, Y = 6 };
        using var resultA = new Point();
        using var resultB = new Point();

        rect.SetA(pointA);
        rect.SetB(pointB);
        rect.Move(2, 3);

        rect.GetA(resultA);
        rect.GetB(resultB);

        Assert.AreEqual(3, resultA.X); // 1 + 2
        Assert.AreEqual(5, resultA.Y); // 2 + 3
        Assert.AreEqual(7, resultB.X); // 5 + 2
        Assert.AreEqual(9, resultB.Y); // 6 + 3
    }

    [TestMethod]
    public void Test_Grow()
    {
        using var rect = new Rect();
        using var pointA = new Point { X = 2, Y = 3 };
        using var pointB = new Point { X = 6, Y = 7 };
        using var resultA = new Point();
        using var resultB = new Point();

        rect.SetA(pointA);
        rect.SetB(pointB);
        rect.Grow(1, 2);

        rect.GetA(resultA);
        rect.GetB(resultB);

        Assert.AreEqual(1, resultA.X); // 2 - 1
        Assert.AreEqual(1, resultA.Y); // 3 - 2
        Assert.AreEqual(7, resultB.X); // 6 + 1
        Assert.AreEqual(9, resultB.Y); // 7 + 2
    }

    [TestMethod]
    public void Test_Intersect()
    {
        using var rect1 = new Rect();
        using var rect2 = new Rect();
        using var pointA1 = new Point { X = 0, Y = 0 };
        using var pointB1 = new Point { X = 10, Y = 10 };
        using var pointA2 = new Point { X = 5, Y = 5 };
        using var pointB2 = new Point { X = 15, Y = 15 };
        using var resultA = new Point();
        using var resultB = new Point();

        rect1.SetA(pointA1);
        rect1.SetB(pointB1);
        rect2.SetA(pointA2);
        rect2.SetB(pointB2);

        rect1.Intersect(rect2);

        rect1.GetA(resultA);
        rect1.GetB(resultB);

        Assert.AreEqual(5, resultA.X);
        Assert.AreEqual(5, resultA.Y);
        Assert.AreEqual(10, resultB.X);
        Assert.AreEqual(10, resultB.Y);
    }

    [TestMethod]
    public void Test_Union()
    {
        using var rect1 = new Rect();
        using var rect2 = new Rect();
        using var pointA1 = new Point { X = 0, Y = 0 };
        using var pointB1 = new Point { X = 5, Y = 5 };
        using var pointA2 = new Point { X = 3, Y = 3 };
        using var pointB2 = new Point { X = 8, Y = 8 };
        using var resultA = new Point();
        using var resultB = new Point();

        rect1.SetA(pointA1);
        rect1.SetB(pointB1);
        rect2.SetA(pointA2);
        rect2.SetB(pointB2);

        rect1.Union(rect2);

        rect1.GetA(resultA);
        rect1.GetB(resultB);

        Assert.AreEqual(0, resultA.X);
        Assert.AreEqual(0, resultA.Y);
        Assert.AreEqual(8, resultB.X);
        Assert.AreEqual(8, resultB.Y);
    }

    [TestMethod]
    public void Test_Contains()
    {
        using var rect = new Rect();
        using var pointA = new Point { X = 1, Y = 2 };
        using var pointB = new Point { X = 5, Y = 6 };
        using var insidePoint = new Point { X = 3, Y = 4 };
        using var outsidePoint = new Point { X = 7, Y = 8 };

        rect.SetA(pointA);
        rect.SetB(pointB);

        Assert.IsTrue(rect.Contains(insidePoint));
        Assert.IsFalse(rect.Contains(outsidePoint));
    }

    [TestMethod]
    public void Test_IsEmpty()
    {
        using var emptyRect = new Rect();
        using var nonEmptyRect = new Rect();
        using var pointA = new Point { X = 1, Y = 2 };
        using var pointB = new Point { X = 5, Y = 6 };

        Assert.IsTrue(emptyRect.IsEmpty());

        nonEmptyRect.SetA(pointA);
        nonEmptyRect.SetB(pointB);
        Assert.IsFalse(nonEmptyRect.IsEmpty());
    }

    [TestMethod]
    public void Test_GetA_SetA()
    {
        using var rect = new Rect();
        using var point = new Point { X = 3, Y = 4 };
        using var result = new Point();

        rect.SetA(point);
        rect.GetA(result);

        Assert.AreEqual(3, result.X);
        Assert.AreEqual(4, result.Y);
    }

    [TestMethod]
    public void Test_GetB_SetB()
    {
        using var rect = new Rect();
        using var point = new Point { X = 7, Y = 8 };
        using var result = new Point();

        rect.SetB(point);
        rect.GetB(result);

        Assert.AreEqual(7, result.X);
        Assert.AreEqual(8, result.Y);
    }
}
