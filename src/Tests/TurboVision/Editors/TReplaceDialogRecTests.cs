using TurboVision.Editors;

namespace Tests.TurboVision.Editors;

[TestClass]
public sealed unsafe class TReplaceDialogRecTests
{
    [TestMethod]
    public void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TReplaceDialogRec.PlacementSize];
        using var replaceDialogRec = new TReplaceDialogRec(
            bytes,
            "test search",
            "test replace",
            42
        );
        Assert.IsNotNull(replaceDialogRec);
        Assert.IsFalse(replaceDialogRec.IsDisposed);
        Assert.AreEqual("test search", replaceDialogRec.Find);
        Assert.AreEqual("test replace", replaceDialogRec.Replace);
        Assert.AreEqual((ushort)42, replaceDialogRec.Options);
    }

    [TestMethod]
    public void Test_New()
    {
        using var replaceDialogRec = new TReplaceDialogRec("test search", "test replace", 42);
        Assert.IsNotNull(replaceDialogRec);
        Assert.IsFalse(replaceDialogRec.IsDisposed);
        Assert.AreEqual("test search", replaceDialogRec.Find);
        Assert.AreEqual("test replace", replaceDialogRec.Replace);
        Assert.AreEqual((ushort)42, replaceDialogRec.Options);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var replaceDialogRec1 = new TReplaceDialogRec("search text", "replace text", 100);
        using var replaceDialogRec2 = new TReplaceDialogRec("search text", "replace text", 100);
        using var replaceDialogRec3 = new TReplaceDialogRec(
            "different search",
            "replace text",
            100
        );
        using var replaceDialogRec4 = new TReplaceDialogRec(
            "search text",
            "different replace",
            100
        );
        using var replaceDialogRec5 = new TReplaceDialogRec("search text", "replace text", 200);

        // Same data should be equal
        Assert.IsTrue(replaceDialogRec1.Equals(replaceDialogRec2));
        Assert.IsTrue(replaceDialogRec2.Equals(replaceDialogRec1));

        // Different find text should not be equal
        Assert.IsFalse(replaceDialogRec1.Equals(replaceDialogRec3));
        Assert.IsFalse(replaceDialogRec3.Equals(replaceDialogRec1));

        // Different replace text should not be equal
        Assert.IsFalse(replaceDialogRec1.Equals(replaceDialogRec4));
        Assert.IsFalse(replaceDialogRec4.Equals(replaceDialogRec1));

        // Different options should not be equal
        Assert.IsFalse(replaceDialogRec1.Equals(replaceDialogRec5));
        Assert.IsFalse(replaceDialogRec5.Equals(replaceDialogRec1));

        // Self equality
        Assert.IsTrue(replaceDialogRec1.Equals(replaceDialogRec1));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var replaceDialogRec1 = new TReplaceDialogRec("search text", "replace text", 100);
        using var replaceDialogRec2 = new TReplaceDialogRec("search text", "replace text", 100);
        using var replaceDialogRec3 = new TReplaceDialogRec(
            "different search",
            "replace text",
            100
        );
        using var replaceDialogRec4 = new TReplaceDialogRec(
            "search text",
            "different replace",
            100
        );

        // Equal objects should have equal hash codes
        Assert.AreEqual(replaceDialogRec1.GetHashCode(), replaceDialogRec2.GetHashCode());

        // Different objects usually have different hash codes (not guaranteed, but likely)
        Assert.AreNotEqual(replaceDialogRec1.GetHashCode(), replaceDialogRec3.GetHashCode());
        Assert.AreNotEqual(replaceDialogRec1.GetHashCode(), replaceDialogRec4.GetHashCode());

        // Hash code should be consistent
        var hash1 = replaceDialogRec1.GetHashCode();
        var hash2 = replaceDialogRec1.GetHashCode();
        Assert.AreEqual(hash1, hash2);
    }

    [TestMethod]
    public void Test_FindProperty()
    {
        using var replaceDialogRec = new TReplaceDialogRec("initial", "replace", 0);

        // Test initial value
        Assert.AreEqual("initial", replaceDialogRec.Find);

        // Test setting new value
        replaceDialogRec.Find = "new search text";
        Assert.AreEqual("new search text", replaceDialogRec.Find);

        // Test setting empty string
        replaceDialogRec.Find = "";
        Assert.AreEqual("", replaceDialogRec.Find);

        // Test setting longer string
        var longString = new string('a', 70); // Should be less than maxFindStrLen (80)
        replaceDialogRec.Find = longString;
        Assert.AreEqual(longString, replaceDialogRec.Find);
    }

    [TestMethod]
    public void Test_ReplaceProperty()
    {
        using var replaceDialogRec = new TReplaceDialogRec("search", "initial", 0);

        // Test initial value
        Assert.AreEqual("initial", replaceDialogRec.Replace);

        // Test setting new value
        replaceDialogRec.Replace = "new replace text";
        Assert.AreEqual("new replace text", replaceDialogRec.Replace);

        // Test setting empty string
        replaceDialogRec.Replace = "";
        Assert.AreEqual("", replaceDialogRec.Replace);

        // Test setting longer string
        var longString = new string('b', 70); // Should be less than maxReplaceStrLen (80)
        replaceDialogRec.Replace = longString;
        Assert.AreEqual(longString, replaceDialogRec.Replace);
    }

    [TestMethod]
    public void Test_OptionsProperty()
    {
        using var replaceDialogRec = new TReplaceDialogRec("test", "replace", 0);

        // Test initial value
        Assert.AreEqual((ushort)0, replaceDialogRec.Options);

        // Test setting new values
        replaceDialogRec.Options = 42;
        Assert.AreEqual((ushort)42, replaceDialogRec.Options);

        replaceDialogRec.Options = 65535; // Max ushort value
        Assert.AreEqual((ushort)65535, replaceDialogRec.Options);

        replaceDialogRec.Options = 0;
        Assert.AreEqual((ushort)0, replaceDialogRec.Options);
    }

    [TestMethod]
    public void Test_StringTruncation()
    {
        // Test that very long strings are properly truncated to maxFindStrLen/maxReplaceStrLen (80)
        var veryLongFindString = new string('x', 100); // Longer than maxFindStrLen
        var veryLongReplaceString = new string('y', 100); // Longer than maxReplaceStrLen
        using var replaceDialogRec = new TReplaceDialogRec(
            veryLongFindString,
            veryLongReplaceString,
            0
        );

        // Should be truncated to 79 characters (leaving room for null terminator)
        var retrievedFind = replaceDialogRec.Find;
        var retrievedReplace = replaceDialogRec.Replace;
        Assert.IsTrue(retrievedFind.Length <= 79);
        Assert.IsTrue(retrievedReplace.Length <= 79);
        Assert.AreEqual(new string('x', 79), retrievedFind);
        Assert.AreEqual(new string('y', 79), retrievedReplace);
    }
}
