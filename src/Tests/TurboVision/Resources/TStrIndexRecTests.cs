using TurboVision.Resources;

namespace Tests.TurboVision.Resources;

[TestClass]
public sealed class TStrIndexRecTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TStrIndexRec.PlacementSize];
        using var strIndex = new TStrIndexRec(bytes);
        Assert.AreEqual(0, strIndex.Key);
        Assert.AreEqual(0, strIndex.Count);
        Assert.AreEqual(0, strIndex.Offset);
    }

    [TestMethod]
    public void Test_New()
    {
        using var strIndex = new TStrIndexRec();
        Assert.AreEqual(0, strIndex.Key);
        Assert.AreEqual(0, strIndex.Count);
        Assert.AreEqual(0, strIndex.Offset);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new TStrIndexRec();
        using var b = new TStrIndexRec();
        using var c = new TStrIndexRec();

        // Initially all should be equal
        Assert.IsTrue(a.Equals(b));
        Assert.IsTrue(a.Equals(c));

        // Modify a and c to have the same values
        a.Key = 123;
        a.Count = 456;
        a.Offset = 789;

        c.Key = 123;
        c.Count = 456;
        c.Offset = 789;

        Assert.IsTrue(a.Equals(c));
        Assert.IsFalse(a.Equals(b));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new TStrIndexRec();
        using var b = new TStrIndexRec();
        using var c = new TStrIndexRec();

        // Initially all should have same hash
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());

        // Modify a and c to have the same values
        a.Key = 123;
        a.Count = 456;
        a.Offset = 789;

        c.Key = 123;
        c.Count = 456;
        c.Offset = 789;

        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Test_Key()
    {
        using var strIndex = new TStrIndexRec();

        // Test initial value
        Assert.AreEqual(0, strIndex.Key);

        // Test setter and getter
        strIndex.Key = 0x1234;
        Assert.AreEqual(0x1234, strIndex.Key);

        // Test with max value
        strIndex.Key = ushort.MaxValue;
        Assert.AreEqual(ushort.MaxValue, strIndex.Key);

        // Test with zero
        strIndex.Key = 0;
        Assert.AreEqual(0, strIndex.Key);
    }

    [TestMethod]
    public void Test_Count()
    {
        using var strIndex = new TStrIndexRec();

        // Test initial value
        Assert.AreEqual(0, strIndex.Count);

        // Test setter and getter
        strIndex.Count = 0x5678;
        Assert.AreEqual(0x5678, strIndex.Count);

        // Test with max value
        strIndex.Count = ushort.MaxValue;
        Assert.AreEqual(ushort.MaxValue, strIndex.Count);

        // Test with zero
        strIndex.Count = 0;
        Assert.AreEqual(0, strIndex.Count);
    }

    [TestMethod]
    public void Test_Offset()
    {
        using var strIndex = new TStrIndexRec();

        // Test initial value
        Assert.AreEqual(0, strIndex.Offset);

        // Test setter and getter
        strIndex.Offset = 0x9ABC;
        Assert.AreEqual(0x9ABC, strIndex.Offset);

        // Test with max value
        strIndex.Offset = ushort.MaxValue;
        Assert.AreEqual(ushort.MaxValue, strIndex.Offset);

        // Test with zero
        strIndex.Offset = 0;
        Assert.AreEqual(0, strIndex.Offset);
    }

    [TestMethod]
    public void Test_AllPropertiesTogether()
    {
        using var strIndex = new TStrIndexRec();

        // Set all properties
        strIndex.Key = 0x1111;
        strIndex.Count = 0x2222;
        strIndex.Offset = 0x3333;

        // Verify all properties
        Assert.AreEqual(0x1111, strIndex.Key);
        Assert.AreEqual(0x2222, strIndex.Count);
        Assert.AreEqual(0x3333, strIndex.Offset);
    }
}
