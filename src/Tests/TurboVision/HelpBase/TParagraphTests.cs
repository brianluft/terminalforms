using TurboVision.HelpBase;

namespace Tests.TurboVision.HelpBase;

[TestClass]
public sealed class TParagraphTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TParagraph.PlacementSize];
        using var paragraph = new TParagraph(bytes);
        Assert.IsNull(paragraph.Next);
        Assert.IsFalse(paragraph.Wrap);
        Assert.AreEqual((ushort)0, paragraph.Size);
        Assert.AreEqual(string.Empty, paragraph.Text);
    }

    [TestMethod]
    public void Test_New()
    {
        using var paragraph = new TParagraph();
        Assert.IsNull(paragraph.Next);
        Assert.IsFalse(paragraph.Wrap);
        Assert.AreEqual((ushort)0, paragraph.Size);
        Assert.AreEqual(string.Empty, paragraph.Text);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new TParagraph
        {
            Text = "Hello",
            Wrap = true,
            Size = 5,
        };
        using var b = new TParagraph
        {
            Text = "Hello",
            Wrap = true,
            Size = 5,
        };
        using var c = new TParagraph
        {
            Text = "World",
            Wrap = false,
            Size = 10,
        };
        Assert.IsTrue(a.Equals(b));
        Assert.IsFalse(a.Equals(c));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new TParagraph
        {
            Text = "Hello",
            Wrap = true,
            Size = 5,
        };
        using var b = new TParagraph
        {
            Text = "Hello",
            Wrap = true,
            Size = 5,
        };
        using var c = new TParagraph
        {
            Text = "World",
            Wrap = false,
            Size = 10,
        };
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), c.GetHashCode());
    }

    [TestMethod]
    public void Test_Text()
    {
        using var paragraph = new TParagraph();
        Assert.AreEqual(string.Empty, paragraph.Text);

        paragraph.Text = "Hello, World!";
        Assert.AreEqual("Hello, World!", paragraph.Text);

        paragraph.Text = "";
        Assert.AreEqual("", paragraph.Text);
    }

    [TestMethod]
    public void Test_Wrap()
    {
        using var paragraph = new TParagraph();
        Assert.IsFalse(paragraph.Wrap);

        paragraph.Wrap = true;
        Assert.IsTrue(paragraph.Wrap);

        paragraph.Wrap = false;
        Assert.IsFalse(paragraph.Wrap);
    }

    [TestMethod]
    public void Test_Size()
    {
        using var paragraph = new TParagraph();
        Assert.AreEqual((ushort)0, paragraph.Size);

        paragraph.Size = 100;
        Assert.AreEqual((ushort)100, paragraph.Size);

        paragraph.Size = ushort.MaxValue;
        Assert.AreEqual(ushort.MaxValue, paragraph.Size);
    }

    [TestMethod]
    public void Test_Next()
    {
        using var paragraph1 = new TParagraph { Text = "First" };
        using var paragraph2 = new TParagraph { Text = "Second" };

        Assert.IsNull(paragraph1.Next);

        paragraph1.Next = paragraph2;
        Assert.IsNotNull(paragraph1.Next);
        Assert.AreEqual("Second", paragraph1.Next.Text);

        paragraph1.Next = null;
        Assert.IsNull(paragraph1.Next);
    }
}
