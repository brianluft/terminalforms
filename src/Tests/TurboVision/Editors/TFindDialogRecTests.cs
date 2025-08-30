using TurboVision.Editors;

namespace Tests.TurboVision.Editors;

[TestClass]
public sealed unsafe class TFindDialogRecTests
{
    [TestMethod]
    public void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TFindDialogRec.PlacementSize];
        using var findDialogRec = new TFindDialogRec(bytes, "test search", 42);
        Assert.IsNotNull(findDialogRec);
        Assert.IsFalse(findDialogRec.IsDisposed);
        Assert.AreEqual("test search", findDialogRec.Find);
        Assert.AreEqual((ushort)42, findDialogRec.Options);
    }

    [TestMethod]
    public void Test_New()
    {
        using var findDialogRec = new TFindDialogRec("test search", 42);
        Assert.IsNotNull(findDialogRec);
        Assert.IsFalse(findDialogRec.IsDisposed);
        Assert.AreEqual("test search", findDialogRec.Find);
        Assert.AreEqual((ushort)42, findDialogRec.Options);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var findDialogRec1 = new TFindDialogRec("search text", 100);
        using var findDialogRec2 = new TFindDialogRec("search text", 100);
        using var findDialogRec3 = new TFindDialogRec("different text", 100);
        using var findDialogRec4 = new TFindDialogRec("search text", 200);

        // Same data should be equal
        Assert.IsTrue(findDialogRec1.Equals(findDialogRec2));
        Assert.IsTrue(findDialogRec2.Equals(findDialogRec1));

        // Different find text should not be equal
        Assert.IsFalse(findDialogRec1.Equals(findDialogRec3));
        Assert.IsFalse(findDialogRec3.Equals(findDialogRec1));

        // Different options should not be equal
        Assert.IsFalse(findDialogRec1.Equals(findDialogRec4));
        Assert.IsFalse(findDialogRec4.Equals(findDialogRec1));

        // Self equality
        Assert.IsTrue(findDialogRec1.Equals(findDialogRec1));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var findDialogRec1 = new TFindDialogRec("search text", 100);
        using var findDialogRec2 = new TFindDialogRec("search text", 100);
        using var findDialogRec3 = new TFindDialogRec("different text", 100);

        // Equal objects should have equal hash codes
        Assert.AreEqual(findDialogRec1.GetHashCode(), findDialogRec2.GetHashCode());

        // Different objects usually have different hash codes (not guaranteed, but likely)
        Assert.AreNotEqual(findDialogRec1.GetHashCode(), findDialogRec3.GetHashCode());

        // Hash code should be consistent
        var hash1 = findDialogRec1.GetHashCode();
        var hash2 = findDialogRec1.GetHashCode();
        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void Test_FindProperty()
    {
        using var findDialogRec = new TFindDialogRec("initial", 0);

        // Test initial value
        Assert.AreEqual("initial", findDialogRec.Find);

        // Test setting new value
        findDialogRec.Find = "new search text";
        Assert.AreEqual("new search text", findDialogRec.Find);

        // Test setting empty string
        findDialogRec.Find = "";
        Assert.AreEqual("", findDialogRec.Find);

        // Test setting longer string
        var longString = new string('a', 70); // Should be less than maxFindStrLen (80)
        findDialogRec.Find = longString;
        Assert.AreEqual(longString, findDialogRec.Find);
    }

    [TestMethod]
    public void Test_OptionsProperty()
    {
        using var findDialogRec = new TFindDialogRec("test", 0);

        // Test initial value
        Assert.AreEqual((ushort)0, findDialogRec.Options);

        // Test setting new values
        findDialogRec.Options = 42;
        Assert.AreEqual((ushort)42, findDialogRec.Options);

        findDialogRec.Options = 65535; // Max ushort value
        Assert.AreEqual((ushort)65535, findDialogRec.Options);

        findDialogRec.Options = 0;
        Assert.AreEqual((ushort)0, findDialogRec.Options);
    }

    [TestMethod]
    public void Test_StringTruncation()
    {
        // Test that very long strings are properly truncated to maxFindStrLen (80)
        var veryLongString = new string('x', 100); // Longer than maxFindStrLen
        using var findDialogRec = new TFindDialogRec(veryLongString, 0);

        // Should be truncated to 79 characters (leaving room for null terminator)
        var retrievedFind = findDialogRec.Find;
        Assert.IsTrue(retrievedFind.Length <= 79);
        Assert.AreEqual(new string('x', 79), retrievedFind);
    }
}
