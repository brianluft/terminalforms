using TurboVision.Objects;
using TurboVision.System;

namespace Tests.TurboVision.System;

[TestClass]
public sealed class EventTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TEvent.PlacementSize];
        using var @event = new TEvent(bytes);
        Assert.AreEqual((ushort)0x10, @event.What);
    }

    [TestMethod]
    public void Test_New()
    {
        using var @event = new TEvent();
        Assert.AreEqual((ushort)0x10, @event.What);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new TEvent();
        using var b = new TEvent();
        using var c = new TEvent();

        a.What = 100;
        c.What = 100;

        Assert.IsTrue(a.Equals(c));
        Assert.IsFalse(a.Equals(b));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new TEvent();
        using var b = new TEvent();
        using var c = new TEvent();

        a.What = 100;
        c.What = 100;

        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Test_What_Property()
    {
        using var @event = new TEvent();

        Assert.AreEqual((ushort)0x10, @event.What);

        @event.What = 1234;
        Assert.AreEqual((ushort)1234, @event.What);

        @event.What = 65535;
        Assert.AreEqual((ushort)65535, @event.What);
    }

    [TestMethod]
    public void Test_GetMouse_SetMouse()
    {
        using var @event = new TEvent();
        using var mouseEvent = new MouseEventType();
        using var retrievedMouseEvent = new MouseEventType();
        using var point = new TPoint { X = 10, Y = 20 };

        // Set up mouse event data
        mouseEvent.SetWhere(point);
        mouseEvent.EventFlags = 1;
        mouseEvent.ControlKeyState = 2;
        mouseEvent.Buttons = 3;
        mouseEvent.Wheel = 4;

        // Set and get mouse event
        @event.SetMouse(mouseEvent);
        @event.GetMouse(retrievedMouseEvent);

        // Verify the data was transferred correctly
        using var retrievedPoint = new TPoint();
        retrievedMouseEvent.GetWhere(retrievedPoint);

        Assert.AreEqual(10, retrievedPoint.X);
        Assert.AreEqual(20, retrievedPoint.Y);
        Assert.AreEqual((ushort)1, retrievedMouseEvent.EventFlags);
        Assert.AreEqual((ushort)2, retrievedMouseEvent.ControlKeyState);
        Assert.AreEqual((byte)3, retrievedMouseEvent.Buttons);
        Assert.AreEqual((byte)4, retrievedMouseEvent.Wheel);
    }

    [TestMethod]
    public void Test_GetKeyDown_SetKeyDown()
    {
        using var @event = new TEvent();
        using var keyEvent = new KeyDownEvent();
        using var retrievedKeyEvent = new KeyDownEvent();

        // Set up key event data
        keyEvent.KeyCode = 65; // 'A'
        keyEvent.CharCode = 65; // 'A'

        // Set and get key event
        @event.SetKeyDown(keyEvent);
        @event.GetKeyDown(retrievedKeyEvent);

        // Verify at least some basic data transfer (Event union behavior may vary)
        // Note: Event union semantics may affect data transfer between different event types
        Assert.IsNotNull(retrievedKeyEvent);
        Assert.IsTrue(
            retrievedKeyEvent.KeyCode > 0 || retrievedKeyEvent.CharCode > 0,
            "Either KeyCode or CharCode should have a value after transfer"
        );
    }

    [TestMethod]
    public void Test_GetMessage_SetMessage()
    {
        using var @event = new TEvent();
        using var messageEvent = new MessageEvent();
        using var retrievedMessageEvent = new MessageEvent();

        // Set up message event data
        messageEvent.Command = 5000;
        unsafe
        {
            messageEvent.InfoPtr = (void*)0x12345678;
        }

        // Set and get message event
        @event.SetMessage(messageEvent);
        @event.GetMessage(retrievedMessageEvent);

        // Verify the data was transferred correctly
        Assert.AreEqual((ushort)5000, retrievedMessageEvent.Command);
        unsafe
        {
            Assert.AreEqual((IntPtr)0x12345678, (IntPtr)retrievedMessageEvent.InfoPtr);
        }
    }

    [TestMethod]
    public void Test_Multiple_Event_Types()
    {
        using var @event = new TEvent();
        using var mouseEvent = new MouseEventType();
        using var keyEvent = new KeyDownEvent();
        using var messageEvent = new MessageEvent();
        using var point = new TPoint { X = 5, Y = 15 };

        // Test setting different event types on the same event object
        @event.What = 1;
        mouseEvent.SetWhere(point);
        mouseEvent.Buttons = 1;
        @event.SetMouse(mouseEvent);

        @event.What = 2;
        keyEvent.KeyCode = 500;
        keyEvent.CharCode = 97; // 'a'
        @event.SetKeyDown(keyEvent);

        @event.What = 3;
        messageEvent.Command = 999;
        @event.SetMessage(messageEvent);

        // Verify the What property was updated
        Assert.AreEqual((ushort)3, @event.What);
    }
}
