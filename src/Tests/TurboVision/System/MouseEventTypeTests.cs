using TurboVision.Objects;
using TurboVision.System;

namespace Tests.TurboVision.System;

[TestClass]
public sealed class MouseEventTypeTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[MouseEventType.PlacementSize];
        using var mouseEvent = new MouseEventType(bytes);
        Assert.AreEqual(0, mouseEvent.EventFlags);
        Assert.AreEqual(0, mouseEvent.ControlKeyState);
        Assert.AreEqual(0, mouseEvent.Buttons);
        Assert.AreEqual(0, mouseEvent.Wheel);
    }

    [TestMethod]
    public void Test_New()
    {
        using var mouseEvent = new MouseEventType();
        Assert.AreEqual(0, mouseEvent.EventFlags);
        Assert.AreEqual(0, mouseEvent.ControlKeyState);
        Assert.AreEqual(0, mouseEvent.Buttons);
        Assert.AreEqual(0, mouseEvent.Wheel);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new MouseEventType();
        using var b = new MouseEventType();
        using var c = new MouseEventType();
        using var TPoint = new TPoint { X = 10, Y = 20 };

        // Initially all should be equal
        Assert.IsTrue(a.Equals(b));
        Assert.IsTrue(a.Equals(c));

        // Modify a and c to have the same values
        a.EventFlags = 123;
        a.ControlKeyState = 456;
        a.Buttons = 7;
        a.Wheel = 8;
        a.SetWhere(TPoint);

        c.EventFlags = 123;
        c.ControlKeyState = 456;
        c.Buttons = 7;
        c.Wheel = 8;
        c.SetWhere(TPoint);

        Assert.IsTrue(a.Equals(c));
        Assert.IsFalse(a.Equals(b));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new MouseEventType();
        using var b = new MouseEventType();
        using var c = new MouseEventType();
        using var TPoint = new TPoint { X = 10, Y = 20 };

        // Initially all should have same hash
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());

        // Modify a and c to have the same values
        a.EventFlags = 123;
        a.ControlKeyState = 456;
        a.Buttons = 7;
        a.Wheel = 8;
        a.SetWhere(TPoint);

        c.EventFlags = 123;
        c.ControlKeyState = 456;
        c.Buttons = 7;
        c.Wheel = 8;
        c.SetWhere(TPoint);

        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Test_GetWhere_SetWhere()
    {
        using var mouseEvent = new MouseEventType();
        using var inputPoint = new TPoint { X = 15, Y = 25 };
        using var outputPoint = new TPoint();

        mouseEvent.SetWhere(inputPoint);
        mouseEvent.GetWhere(outputPoint);

        Assert.AreEqual(15, outputPoint.X);
        Assert.AreEqual(25, outputPoint.Y);
    }

    [TestMethod]
    public void Test_EventFlags()
    {
        using var mouseEvent = new MouseEventType();

        // Test initial value
        Assert.AreEqual(0, mouseEvent.EventFlags);

        // Test setter and getter
        mouseEvent.EventFlags = 0x1234;
        Assert.AreEqual(0x1234, mouseEvent.EventFlags);

        // Test with max value
        mouseEvent.EventFlags = ushort.MaxValue;
        Assert.AreEqual(ushort.MaxValue, mouseEvent.EventFlags);

        // Test with zero
        mouseEvent.EventFlags = 0;
        Assert.AreEqual(0, mouseEvent.EventFlags);
    }

    [TestMethod]
    public void Test_ControlKeyState()
    {
        using var mouseEvent = new MouseEventType();

        // Test initial value
        Assert.AreEqual(0, mouseEvent.ControlKeyState);

        // Test setter and getter
        mouseEvent.ControlKeyState = 0x5678;
        Assert.AreEqual(0x5678, mouseEvent.ControlKeyState);

        // Test with max value
        mouseEvent.ControlKeyState = ushort.MaxValue;
        Assert.AreEqual(ushort.MaxValue, mouseEvent.ControlKeyState);

        // Test with zero
        mouseEvent.ControlKeyState = 0;
        Assert.AreEqual(0, mouseEvent.ControlKeyState);
    }

    [TestMethod]
    public void Test_Buttons()
    {
        using var mouseEvent = new MouseEventType();

        // Test initial value
        Assert.AreEqual(0, mouseEvent.Buttons);

        // Test setter and getter
        mouseEvent.Buttons = 0xAB;
        Assert.AreEqual(0xAB, mouseEvent.Buttons);

        // Test with max value
        mouseEvent.Buttons = byte.MaxValue;
        Assert.AreEqual(byte.MaxValue, mouseEvent.Buttons);

        // Test with zero
        mouseEvent.Buttons = 0;
        Assert.AreEqual(0, mouseEvent.Buttons);
    }

    [TestMethod]
    public void Test_Wheel()
    {
        using var mouseEvent = new MouseEventType();

        // Test initial value
        Assert.AreEqual(0, mouseEvent.Wheel);

        // Test setter and getter
        mouseEvent.Wheel = 0xCD;
        Assert.AreEqual(0xCD, mouseEvent.Wheel);

        // Test with max value
        mouseEvent.Wheel = byte.MaxValue;
        Assert.AreEqual(byte.MaxValue, mouseEvent.Wheel);

        // Test with zero
        mouseEvent.Wheel = 0;
        Assert.AreEqual(0, mouseEvent.Wheel);
    }

    [TestMethod]
    public void Test_AllPropertiesTogether()
    {
        using var mouseEvent = new MouseEventType();
        using var TPoint = new TPoint { X = 100, Y = 200 };
        using var resultPoint = new TPoint();

        // Set all properties
        mouseEvent.EventFlags = 0x1111;
        mouseEvent.ControlKeyState = 0x2222;
        mouseEvent.Buttons = 0x33;
        mouseEvent.Wheel = 0x44;
        mouseEvent.SetWhere(TPoint);

        // Verify all properties
        Assert.AreEqual(0x1111, mouseEvent.EventFlags);
        Assert.AreEqual(0x2222, mouseEvent.ControlKeyState);
        Assert.AreEqual(0x33, mouseEvent.Buttons);
        Assert.AreEqual(0x44, mouseEvent.Wheel);

        mouseEvent.GetWhere(resultPoint);
        Assert.AreEqual(100, resultPoint.X);
        Assert.AreEqual(200, resultPoint.Y);
    }
}
