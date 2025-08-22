using TurboVision.Text;

namespace Tests.TurboVision.Text;

[TestClass]
public sealed class TTextMetricsTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TTextMetrics.PlacementSize];
        using var textMetrics = new TTextMetrics(bytes);
        Assert.AreEqual(0u, textMetrics.Width);
        Assert.AreEqual(0u, textMetrics.CharacterCount);
        Assert.AreEqual(0u, textMetrics.GraphemeCount);
        textMetrics.Width = 10u;
        textMetrics.CharacterCount = 5u;
        textMetrics.GraphemeCount = 3u;
        Assert.AreEqual(10u, textMetrics.Width);
        Assert.AreEqual(5u, textMetrics.CharacterCount);
        Assert.AreEqual(3u, textMetrics.GraphemeCount);
    }

    [TestMethod]
    public void Test_New()
    {
        using var textMetrics = new TTextMetrics();
        Assert.AreEqual(0u, textMetrics.Width);
        Assert.AreEqual(0u, textMetrics.CharacterCount);
        Assert.AreEqual(0u, textMetrics.GraphemeCount);
        textMetrics.Width = 15u;
        textMetrics.CharacterCount = 8u;
        textMetrics.GraphemeCount = 6u;
        Assert.AreEqual(15u, textMetrics.Width);
        Assert.AreEqual(8u, textMetrics.CharacterCount);
        Assert.AreEqual(6u, textMetrics.GraphemeCount);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new TTextMetrics
        {
            Width = 10u,
            CharacterCount = 5u,
            GraphemeCount = 3u,
        };
        using var b = new TTextMetrics
        {
            Width = 10u,
            CharacterCount = 5u,
            GraphemeCount = 3u,
        };
        using var c = new TTextMetrics
        {
            Width = 20u,
            CharacterCount = 8u,
            GraphemeCount = 6u,
        };
        Assert.IsTrue(a.Equals(b));
        Assert.IsFalse(a.Equals(c));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new TTextMetrics
        {
            Width = 10u,
            CharacterCount = 5u,
            GraphemeCount = 3u,
        };
        using var b = new TTextMetrics
        {
            Width = 10u,
            CharacterCount = 5u,
            GraphemeCount = 3u,
        };
        using var c = new TTextMetrics
        {
            Width = 20u,
            CharacterCount = 8u,
            GraphemeCount = 6u,
        };
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), c.GetHashCode());
    }

    [TestMethod]
    public void Test_Width()
    {
        using var textMetrics = new TTextMetrics
        {
            Width = 15u,
            CharacterCount = 8u,
            GraphemeCount = 6u,
        };
        Assert.AreEqual(15u, textMetrics.Width);
        textMetrics.Width = 25u;
        Assert.AreEqual(25u, textMetrics.Width);
    }

    [TestMethod]
    public void Test_CharacterCount()
    {
        using var textMetrics = new TTextMetrics
        {
            Width = 15u,
            CharacterCount = 8u,
            GraphemeCount = 6u,
        };
        Assert.AreEqual(8u, textMetrics.CharacterCount);
        textMetrics.CharacterCount = 12u;
        Assert.AreEqual(12u, textMetrics.CharacterCount);
    }

    [TestMethod]
    public void Test_GraphemeCount()
    {
        using var textMetrics = new TTextMetrics
        {
            Width = 15u,
            CharacterCount = 8u,
            GraphemeCount = 6u,
        };
        Assert.AreEqual(6u, textMetrics.GraphemeCount);
        textMetrics.GraphemeCount = 10u;
        Assert.AreEqual(10u, textMetrics.GraphemeCount);
    }
}
