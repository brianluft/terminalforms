using TurboVision.Dialogs;

namespace Tests.TurboVision.Dialogs;

[TestClass]
public sealed class TDirEntryTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TDirEntry.PlacementSize];
        using var entry = new TDirEntry(bytes, "DisplayText", "/home/user");
        Assert.AreEqual("DisplayText", entry.Text);
        Assert.AreEqual("/home/user", entry.Directory);
    }

    [TestMethod]
    public void Test_New()
    {
        using var entry = new TDirEntry("MyFile.txt", "/var/data");
        Assert.AreEqual("MyFile.txt", entry.Text);
        Assert.AreEqual("/var/data", entry.Directory);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new TDirEntry("File1.txt", "/path1");
        using var b = new TDirEntry("File1.txt", "/path1");
        using var c = new TDirEntry("File2.txt", "/path2");
        Assert.IsTrue(a.Equals(b));
        Assert.IsFalse(a.Equals(c));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new TDirEntry("File1.txt", "/path1");
        using var b = new TDirEntry("File1.txt", "/path1");
        using var c = new TDirEntry("File2.txt", "/path2");
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), c.GetHashCode());
    }

    [TestMethod]
    public void Test_Directory_Property()
    {
        using var entry = new TDirEntry("Test.dat", "/some/path");
        Assert.AreEqual("/some/path", entry.Directory);
    }

    [TestMethod]
    public void Test_Text_Property()
    {
        using var entry = new TDirEntry("DisplayName", "/some/dir");
        Assert.AreEqual("DisplayName", entry.Text);
    }

    [TestMethod]
    public void Test_UTF8_Support()
    {
        using var entry = new TDirEntry("Файл.txt", "/путь/к/файлу");
        Assert.AreEqual("Файл.txt", entry.Text);
        Assert.AreEqual("/путь/к/файлу", entry.Directory);
    }

    [TestMethod]
    public void Test_Empty_Strings()
    {
        using var entry = new TDirEntry("", "");
        Assert.AreEqual("", entry.Text);
        Assert.AreEqual("", entry.Directory);
    }
}
