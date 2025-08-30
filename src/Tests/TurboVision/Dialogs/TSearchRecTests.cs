using TurboVision.Dialogs;

namespace Tests.TurboVision.Dialogs;

[TestClass]
public sealed unsafe class TSearchRecTests
{
    [TestMethod]
    public void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TSearchRec.PlacementSize];
        using var searchRec = new TSearchRec(bytes);
        Assert.IsNotNull(searchRec);
        Assert.IsFalse(searchRec.IsDisposed);
        Assert.AreEqual((byte)0, searchRec.Attr);
        Assert.AreEqual(0, searchRec.Time);
        Assert.AreEqual(0, searchRec.Size);
        Assert.AreEqual(string.Empty, searchRec.Name);
    }

    [TestMethod]
    public void Test_New()
    {
        using var searchRec = new TSearchRec();
        Assert.IsNotNull(searchRec);
        Assert.IsFalse(searchRec.IsDisposed);
        Assert.AreEqual((byte)0, searchRec.Attr);
        Assert.AreEqual(0, searchRec.Time);
        Assert.AreEqual(0, searchRec.Size);
        Assert.AreEqual(string.Empty, searchRec.Name);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var searchRec1 = new TSearchRec();
        using var searchRec2 = new TSearchRec();
        using var searchRec3 = new TSearchRec();

        // Set up identical data
        searchRec1.Attr = 1;
        searchRec1.Time = 12345;
        searchRec1.Size = 1024;
        searchRec1.Name = "test.txt";

        searchRec2.Attr = 1;
        searchRec2.Time = 12345;
        searchRec2.Size = 1024;
        searchRec2.Name = "test.txt";

        // Set up different data
        searchRec3.Attr = 2;
        searchRec3.Time = 54321;
        searchRec3.Size = 2048;
        searchRec3.Name = "different.txt";

        // Test equality with same content
        Assert.IsTrue(searchRec1.Equals(searchRec2));
        Assert.IsTrue(searchRec2.Equals(searchRec1));

        // Test inequality with different content
        Assert.IsFalse(searchRec1.Equals(searchRec3));
        Assert.IsFalse(searchRec3.Equals(searchRec1));

        // Test self-equality
        Assert.IsTrue(searchRec1.Equals(searchRec1));

        // Test null equality
        Assert.IsFalse(searchRec1.Equals(null));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var searchRec1 = new TSearchRec();
        using var searchRec2 = new TSearchRec();
        using var searchRec3 = new TSearchRec();

        // Set up identical data
        searchRec1.Attr = 1;
        searchRec1.Time = 12345;
        searchRec1.Size = 1024;
        searchRec1.Name = "test.txt";

        searchRec2.Attr = 1;
        searchRec2.Time = 12345;
        searchRec2.Size = 1024;
        searchRec2.Name = "test.txt";

        // Set up different data
        searchRec3.Attr = 2;
        searchRec3.Time = 54321;
        searchRec3.Size = 2048;
        searchRec3.Name = "different.txt";

        // Same values should produce same hash codes
        Assert.AreEqual(searchRec1.GetHashCode(), searchRec2.GetHashCode());

        // Different values should produce different hash codes (not guaranteed, but likely)
        Assert.AreNotEqual(searchRec1.GetHashCode(), searchRec3.GetHashCode());
    }

    [TestMethod]
    public void Test_AttrProperty()
    {
        using var searchRec = new TSearchRec();

        // Initial value should be 0
        Assert.AreEqual((byte)0, searchRec.Attr);

        // Set and get various values
        searchRec.Attr = 1;
        Assert.AreEqual((byte)1, searchRec.Attr);

        searchRec.Attr = 255;
        Assert.AreEqual((byte)255, searchRec.Attr);

        searchRec.Attr = 0;
        Assert.AreEqual((byte)0, searchRec.Attr);
    }

    [TestMethod]
    public void Test_TimeProperty()
    {
        using var searchRec = new TSearchRec();

        // Initial value should be 0
        Assert.AreEqual(0, searchRec.Time);

        // Set and get various values
        searchRec.Time = 12345;
        Assert.AreEqual(12345, searchRec.Time);

        searchRec.Time = -1;
        Assert.AreEqual(-1, searchRec.Time);

        searchRec.Time = int.MaxValue;
        Assert.AreEqual(int.MaxValue, searchRec.Time);
    }

    [TestMethod]
    public void Test_SizeProperty()
    {
        using var searchRec = new TSearchRec();

        // Initial value should be 0
        Assert.AreEqual(0, searchRec.Size);

        // Set and get various values
        searchRec.Size = 1024;
        Assert.AreEqual(1024, searchRec.Size);

        searchRec.Size = 0;
        Assert.AreEqual(0, searchRec.Size);

        searchRec.Size = int.MaxValue;
        Assert.AreEqual(int.MaxValue, searchRec.Size);
    }

    [TestMethod]
    public void Test_NameProperty()
    {
        using var searchRec = new TSearchRec();

        // Initial value should be empty string
        Assert.AreEqual(string.Empty, searchRec.Name);

        // Set and get various values
        searchRec.Name = "test.txt";
        Assert.AreEqual("test.txt", searchRec.Name);

        searchRec.Name = "long_filename_with_many_characters.extension";
        Assert.AreEqual("long_filename_with_many_characters.extension", searchRec.Name);

        // Test empty string
        searchRec.Name = "";
        Assert.AreEqual("", searchRec.Name);

        // Test maximum length string (510 characters)
        var longName = new string('a', 510);
        searchRec.Name = longName;
        Assert.AreEqual(longName, searchRec.Name);
    }
}
