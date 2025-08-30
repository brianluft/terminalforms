using TurboVision.Resources;

namespace Tests.TurboVision.Resources;

[TestClass]
public sealed class TResourceItemTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TResourceItem.PlacementSize];
        using var item = new TResourceItem(bytes);
        Assert.AreEqual(0, item.Pos);
        Assert.AreEqual(0, item.Size);
        Assert.AreEqual(string.Empty, item.Key);
    }

    [TestMethod]
    public void Test_New()
    {
        using var item = new TResourceItem();
        Assert.AreEqual(0, item.Pos);
        Assert.AreEqual(0, item.Size);
        Assert.AreEqual(string.Empty, item.Key);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new TResourceItem();
        using var b = new TResourceItem();
        using var c = new TResourceItem();

        // Initially all should be equal
        Assert.IsTrue(a.Equals(b));
        Assert.IsTrue(a.Equals(c));

        // Modify a and c to have the same values
        a.Pos = 123;
        a.Size = 456;
        a.Key = "test_key";

        c.Pos = 123;
        c.Size = 456;
        c.Key = "test_key";

        Assert.IsTrue(a.Equals(c));
        Assert.IsFalse(a.Equals(b));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new TResourceItem();
        using var b = new TResourceItem();
        using var c = new TResourceItem();

        // Initially all should have same hash
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());

        // Modify a and c to have the same values
        a.Pos = 123;
        a.Size = 456;
        a.Key = "test_key";

        c.Pos = 123;
        c.Size = 456;
        c.Key = "test_key";

        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Test_Pos()
    {
        using var item = new TResourceItem();

        // Test initial value
        Assert.AreEqual(0, item.Pos);

        // Test setter and getter
        item.Pos = 12345;
        Assert.AreEqual(12345, item.Pos);

        // Test with negative value
        item.Pos = -999;
        Assert.AreEqual(-999, item.Pos);

        // Test with max value
        item.Pos = int.MaxValue;
        Assert.AreEqual(int.MaxValue, item.Pos);

        // Test with min value
        item.Pos = int.MinValue;
        Assert.AreEqual(int.MinValue, item.Pos);

        // Test with zero
        item.Pos = 0;
        Assert.AreEqual(0, item.Pos);
    }

    [TestMethod]
    public void Test_Size()
    {
        using var item = new TResourceItem();

        // Test initial value
        Assert.AreEqual(0, item.Size);

        // Test setter and getter
        item.Size = 67890;
        Assert.AreEqual(67890, item.Size);

        // Test with negative value
        item.Size = -555;
        Assert.AreEqual(-555, item.Size);

        // Test with max value
        item.Size = int.MaxValue;
        Assert.AreEqual(int.MaxValue, item.Size);

        // Test with min value
        item.Size = int.MinValue;
        Assert.AreEqual(int.MinValue, item.Size);

        // Test with zero
        item.Size = 0;
        Assert.AreEqual(0, item.Size);
    }

    [TestMethod]
    public void Test_Key()
    {
        using var item = new TResourceItem();

        // Test initial value
        Assert.AreEqual(string.Empty, item.Key);

        // Test setter and getter
        item.Key = "test_resource_key";
        Assert.AreEqual("test_resource_key", item.Key);

        // Test with empty string
        item.Key = "";
        Assert.AreEqual("", item.Key);

        // Test with special characters
        item.Key = "key_with-special.chars123!@#";
        Assert.AreEqual("key_with-special.chars123!@#", item.Key);

        // Test with unicode characters
        item.Key = "key_with_unicode_Æ∞±≤≥";
        Assert.AreEqual("key_with_unicode_Æ∞±≤≥", item.Key);

        // Test with long string
        item.Key = new string('a', 1000);
        Assert.AreEqual(new string('a', 1000), item.Key);
    }

    [TestMethod]
    public void Test_AllPropertiesTogether()
    {
        using var item = new TResourceItem();

        // Set all properties
        item.Pos = 111111;
        item.Size = 222222;
        item.Key = "combined_test_key";

        // Verify all properties
        Assert.AreEqual(111111, item.Pos);
        Assert.AreEqual(222222, item.Size);
        Assert.AreEqual("combined_test_key", item.Key);
    }
}
