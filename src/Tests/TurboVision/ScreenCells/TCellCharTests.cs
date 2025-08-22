using TurboVision.ScreenCells;

namespace Tests.TurboVision.ScreenCells;

[TestClass]
public sealed class TCellCharTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var size = TCellChar.PlacementSize;
        Assert.IsTrue(size > 0);

        byte* buffer = stackalloc byte[size];
        using var cellChar = new TCellChar(buffer);

        Assert.IsFalse(cellChar.IsDisposed);
    }

    [TestMethod]
    public void Test_New()
    {
        using var cellChar = new TCellChar();

        Assert.IsFalse(cellChar.IsDisposed);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var cellChar1 = new TCellChar();
        using var cellChar2 = new TCellChar();
        using var cellChar3 = new TCellChar();

        // Initially all should be equal (both zero-initialized)
        Assert.IsTrue(cellChar1.Equals(cellChar2));
        Assert.IsTrue(cellChar1.Equals(cellChar3));

        // Modify one and check they're no longer equal
        cellChar1.MoveChar('A');
        Assert.IsFalse(cellChar1.Equals(cellChar2));

        // Modify another to the same value and check they're equal again
        cellChar2.MoveChar('A');
        Assert.IsTrue(cellChar1.Equals(cellChar2));
        Assert.IsFalse(cellChar1.Equals(cellChar3));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var cellChar1 = new TCellChar();
        using var cellChar2 = new TCellChar();

        // Initially both should have the same hash (both zero-initialized)
        var hash1 = cellChar1.GetHashCode();
        var hash2 = cellChar2.GetHashCode();
        Assert.AreEqual(hash1, hash2);

        // After modification, hashes should likely be different
        cellChar1.MoveChar('A');
        var hash1Modified = cellChar1.GetHashCode();
        Assert.AreNotEqual(hash1Modified, hash2);

        // Same modifications should produce same hash
        cellChar2.MoveChar('A');
        var hash2Modified = cellChar2.GetHashCode();
        Assert.AreEqual(hash1Modified, hash2Modified);
    }

    [TestMethod]
    public void Test_Constants()
    {
        var fWide = TCellChar.FWide;
        var fTrail = TCellChar.FTrail;

        Assert.AreEqual(0x1, fWide);
        Assert.AreEqual(0x2, fTrail);
    }

    [TestMethod]
    public void Test_MoveChar()
    {
        using var cellChar = new TCellChar();

        cellChar.MoveChar('A');

        // Check that the character was set
        Assert.AreEqual("A", cellChar.Text);
        Assert.AreEqual(1, cellChar.Size);
        Assert.AreEqual(1, cellChar.TextLength);
        Assert.AreEqual('A', cellChar[0]);
    }

    [TestMethod]
    public void Test_Properties()
    {
        using var cellChar = new TCellChar();

        // Initial state
        Assert.IsTrue(cellChar.Size >= 1); // size() returns max(_textLength, 1)
        Assert.IsFalse(cellChar.IsWide);
        Assert.IsFalse(cellChar.IsWideCharTrail);

        // Set some flags
        cellChar.Flags = TCellChar.FWide;
        Assert.IsTrue(cellChar.IsWide);
        Assert.IsFalse(cellChar.IsWideCharTrail);

        cellChar.Flags = TCellChar.FTrail;
        Assert.IsFalse(cellChar.IsWide);
        Assert.IsTrue(cellChar.IsWideCharTrail);
    }

    [TestMethod]
    public void Test_WideCharTrail()
    {
        using var cellChar = new TCellChar();

        cellChar.MoveWideCharTrail();

        Assert.IsTrue(cellChar.IsWideCharTrail);
        Assert.IsFalse(cellChar.IsWide);
    }

    [TestMethod]
    public void Test_ArrayIndexer()
    {
        using var cellChar = new TCellChar();

        // Test setting and getting individual bytes
        cellChar[0] = 'H';
        cellChar[1] = 'i';

        Assert.AreEqual('H', cellChar[0]);
        Assert.AreEqual('i', cellChar[1]);
    }

    [TestMethod]
    public void Test_TextLength()
    {
        using var cellChar = new TCellChar();

        // Initially text length should be 0 or 1
        var initialLength = cellChar.TextLength;
        Assert.IsTrue(initialLength <= 1);

        // After setting a character, length should be 1
        cellChar.MoveChar('X');
        Assert.AreEqual(1, cellChar.TextLength);

        // Test setting text length directly
        cellChar.TextLength = 3;
        Assert.AreEqual(3, cellChar.TextLength);
    }

    [TestMethod]
    public void Test_EmptyText()
    {
        using var cellChar = new TCellChar();

        // Initially, text might be empty or contain a null character
        var text = cellChar.Text;
        Assert.IsNotNull(text);

        // Size should be at least 1 even for empty text (as per C++ comment)
        Assert.IsTrue(cellChar.Size >= 1);
    }
}
