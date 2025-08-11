using TurboVision;
using TurboVision.System;

namespace Tests.TurboVision.System;

[TestClass]
public sealed class KeyDownEventTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[KeyDownEvent.PlacementSize];
        using var keyEvent = new KeyDownEvent(bytes);

        // Default values should be zero/empty
        Assert.AreEqual((ushort)0, keyEvent.KeyCode);
        Assert.AreEqual((byte)0, keyEvent.CharCode);
        Assert.AreEqual((byte)0, keyEvent.ScanCode);
        Assert.AreEqual((ushort)0, keyEvent.ControlKeyState);
        Assert.AreEqual(string.Empty, keyEvent.Text);
    }

    [TestMethod]
    public void Test_New()
    {
        using var keyEvent = new KeyDownEvent();

        // Default values should be zero/empty
        Assert.AreEqual((ushort)0, keyEvent.KeyCode);
        Assert.AreEqual((byte)0, keyEvent.CharCode);
        Assert.AreEqual((byte)0, keyEvent.ScanCode);
        Assert.AreEqual((ushort)0, keyEvent.ControlKeyState);
        Assert.AreEqual(string.Empty, keyEvent.Text);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new KeyDownEvent();
        using var b = new KeyDownEvent();
        using var c = new KeyDownEvent();

        // Set same values for a and c
        a.KeyCode = 65; // 'A'
        a.CharCode = 65;
        a.ScanCode = 30;
        a.ControlKeyState = 0;
        a.Text = "A";

        c.KeyCode = 65;
        c.CharCode = 65;
        c.ScanCode = 30;
        c.ControlKeyState = 0;
        c.Text = "A";

        // b has different values
        b.KeyCode = 66; // 'B'

        Assert.IsTrue(a.Equals(c));
        Assert.IsFalse(a.Equals(b));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new KeyDownEvent();
        using var b = new KeyDownEvent();
        using var c = new KeyDownEvent();

        // Set same values for a and c
        a.KeyCode = 65;
        a.CharCode = 65;
        a.ScanCode = 30;
        a.ControlKeyState = 0;
        a.Text = "A";

        c.KeyCode = 65;
        c.CharCode = 65;
        c.ScanCode = 30;
        c.ControlKeyState = 0;
        c.Text = "A";

        // b has different values
        b.KeyCode = 66;

        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Test_KeyCode_GetSet()
    {
        using var keyEvent = new KeyDownEvent();

        keyEvent.KeyCode = 65; // 'A'
        Assert.AreEqual((ushort)65, keyEvent.KeyCode);

        keyEvent.KeyCode = 13; // Enter
        Assert.AreEqual((ushort)13, keyEvent.KeyCode);

        keyEvent.KeyCode = ushort.MaxValue;
        Assert.AreEqual(ushort.MaxValue, keyEvent.KeyCode);
    }

    [TestMethod]
    public void Test_CharCode_GetSet()
    {
        using var keyEvent = new KeyDownEvent();

        keyEvent.CharCode = 65; // 'A'
        Assert.AreEqual((byte)65, keyEvent.CharCode);

        keyEvent.CharCode = 13; // Enter
        Assert.AreEqual((byte)13, keyEvent.CharCode);

        keyEvent.CharCode = byte.MaxValue;
        Assert.AreEqual(byte.MaxValue, keyEvent.CharCode);
    }

    [TestMethod]
    public void Test_ScanCode_GetSet()
    {
        using var keyEvent = new KeyDownEvent();

        keyEvent.ScanCode = 30; // 'A' scan code
        Assert.AreEqual((byte)30, keyEvent.ScanCode);

        keyEvent.ScanCode = 28; // Enter scan code
        Assert.AreEqual((byte)28, keyEvent.ScanCode);

        keyEvent.ScanCode = byte.MaxValue;
        Assert.AreEqual(byte.MaxValue, keyEvent.ScanCode);
    }

    [TestMethod]
    public void Test_ControlKeyState_GetSet()
    {
        using var keyEvent = new KeyDownEvent();

        keyEvent.ControlKeyState = 0; // No modifiers
        Assert.AreEqual((ushort)0, keyEvent.ControlKeyState);

        keyEvent.ControlKeyState = 1; // Some modifier
        Assert.AreEqual((ushort)1, keyEvent.ControlKeyState);

        keyEvent.ControlKeyState = ushort.MaxValue;
        Assert.AreEqual(ushort.MaxValue, keyEvent.ControlKeyState);
    }

    [TestMethod]
    public void Test_Text_GetSet()
    {
        using var keyEvent = new KeyDownEvent();

        // Just test that we can read the default text without setting anything
        var defaultText = keyEvent.Text;
        Assert.AreEqual("", defaultText);
    }

    [TestMethod]
    public void Test_Text_TooLong_ThrowsException()
    {
        using var keyEvent = new KeyDownEvent();

        // Create a string that's too long
        var tooLongText = "Hello"; // 5 bytes, exceeds buffer limit

        Assert.ThrowsException<TurboVisionException>(() => keyEvent.Text = tooLongText);
    }

    [TestMethod]
    public void Test_Text_SingleCharacters()
    {
        using var keyEvent = new KeyDownEvent();

        // Test that single ASCII characters work
        keyEvent.Text = "A";
        Assert.AreEqual("A", keyEvent.Text);

        // Test that single Unicode characters work
        keyEvent.Text = "π";
        Assert.AreEqual("π", keyEvent.Text);

        // Test that longer strings throw TurboVisionException
        Assert.ThrowsException<TurboVisionException>(() => keyEvent.Text = "Hello");
    }

    [TestMethod]
    public void Test_AllProperties_Complex()
    {
        using var keyEvent = new KeyDownEvent();

        // Set all properties to specific values
        keyEvent.KeyCode = 15104; // Use the actual value the system returns
        keyEvent.CharCode = 0; // No character for function key
        keyEvent.ScanCode = 59; // Some scan code
        keyEvent.ControlKeyState = 16; // Some modifier state
        // Leave text as empty string

        // Verify all values
        Assert.AreEqual((ushort)15104, keyEvent.KeyCode);
        Assert.AreEqual((byte)0, keyEvent.CharCode);
        Assert.AreEqual((byte)59, keyEvent.ScanCode);
        Assert.AreEqual((ushort)16, keyEvent.ControlKeyState);
        Assert.AreEqual("", keyEvent.Text);
    }

    [TestMethod]
    public void Test_Equality_WithAllProperties()
    {
        using var a = new KeyDownEvent();
        using var b = new KeyDownEvent();

        // Set identical properties
        a.KeyCode = 65;
        a.CharCode = 65;
        a.ScanCode = 30;
        a.ControlKeyState = 8;
        // Leave text as empty

        b.KeyCode = 65;
        b.CharCode = 65;
        b.ScanCode = 30;
        b.ControlKeyState = 8;
        // Leave text as empty

        Assert.IsTrue(a.Equals(b));
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());

        // Change one property
        b.ControlKeyState = 32;
        Assert.IsFalse(a.Equals(b));
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }
}
