using TurboVision.Dialogs;

namespace Tests.TurboVision.Dialogs;

[TestClass]
public sealed unsafe class TSItemTests
{
    [TestMethod]
    public void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TSItem.PlacementSize];
        using var item = new TSItem(bytes, "Test Value", null);
        Assert.IsNotNull(item);
        Assert.IsFalse(item.IsDisposed);
        Assert.AreEqual("Test Value", item.Value);
        Assert.IsNull(item.Next);
    }

    [TestMethod]
    public void Test_New()
    {
        // Test basic constructor with value and no next
        using var item1 = new TSItem("Test Value", null);
        Assert.AreEqual("Test Value", item1.Value);
        Assert.IsNull(item1.Next);

        // Test constructor with value and next item
        using var item2 = new TSItem("Second Value", null);
        using var item3 = new TSItem("First Value", item2);
        Assert.AreEqual("First Value", item3.Value);
        Assert.IsNotNull(item3.Next);
        Assert.AreEqual("Second Value", item3.Next!.Value);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var item1 = new TSItem("Test", null);
        using var item2 = new TSItem("Test", null);
        using var item3 = new TSItem("Different", null);

        // Test equality with same content
        Assert.IsTrue(item1.Equals(item2));
        Assert.IsTrue(item2.Equals(item1));

        // Test inequality with different content
        Assert.IsFalse(item1.Equals(item3));
        Assert.IsFalse(item3.Equals(item1));

        // Test self-equality
        Assert.IsTrue(item1.Equals(item1));

        // Test null equality
        Assert.IsFalse(item1.Equals(null));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var item1 = new TSItem("Test", null);
        using var item2 = new TSItem("Test", null);
        using var item3 = new TSItem("Different", null);

        // Same values should produce same hash codes
        Assert.AreEqual(item1.GetHashCode(), item2.GetHashCode());

        // Different values should produce different hash codes (not guaranteed, but likely)
        Assert.AreNotEqual(item1.GetHashCode(), item3.GetHashCode());
    }

    [TestMethod]
    public void Test_ValueProperty()
    {
        using var item = new TSItem("Initial Value", null);
        Assert.AreEqual("Initial Value", item.Value);

        // Change the value
        item.Value = "New Value";
        Assert.AreEqual("New Value", item.Value);

        // Test empty string
        item.Value = "";
        Assert.AreEqual("", item.Value);
    }

    [TestMethod]
    public void Test_NextProperty()
    {
        using var item1 = new TSItem("First", null);
        using var item2 = new TSItem("Second", null);

        Assert.IsNull(item1.Next);

        // Set next
        item1.Next = item2;
        Assert.IsNotNull(item1.Next);
        Assert.AreEqual("Second", item1.Next!.Value);

        // Clear next
        item1.Next = null;
        Assert.IsNull(item1.Next);
    }

    [TestMethod]
    public void Test_LinkedList()
    {
        // Create a linked list of items
        using var item3 = new TSItem("Third", null);
        using var item2 = new TSItem("Second", item3);
        using var item1 = new TSItem("First", item2);

        // Verify the chain
        Assert.AreEqual("First", item1.Value);
        Assert.IsNotNull(item1.Next);
        Assert.AreEqual("Second", item1.Next!.Value);
        Assert.IsNotNull(item1.Next.Next);
        Assert.AreEqual("Third", item1.Next.Next!.Value);
        Assert.IsNull(item1.Next.Next.Next);
    }
}
