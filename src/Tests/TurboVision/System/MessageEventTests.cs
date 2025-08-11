using TurboVision.System;

namespace Tests.TurboVision.System;

[TestClass]
public sealed class MessageEventTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[MessageEvent.PlacementSize];
        using var messageEvent = new MessageEvent(bytes);
        Assert.AreEqual((ushort)0, messageEvent.Command);
        Assert.AreEqual(IntPtr.Zero, (IntPtr)messageEvent.InfoPtr);
    }

    [TestMethod]
    public unsafe void Test_New()
    {
        using var messageEvent = new MessageEvent();
        Assert.AreEqual((ushort)0, messageEvent.Command);
        Assert.AreEqual(IntPtr.Zero, (IntPtr)messageEvent.InfoPtr);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new MessageEvent();
        using var b = new MessageEvent();
        using var c = new MessageEvent();

        a.Command = 123;
        c.Command = 123;

        Assert.IsTrue(a.Equals(c));
        Assert.IsFalse(a.Equals(b));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new MessageEvent();
        using var b = new MessageEvent();
        using var c = new MessageEvent();

        a.Command = 456;
        c.Command = 456;

        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Test_Command_GetSet()
    {
        using var messageEvent = new MessageEvent();

        // Test initial value
        Assert.AreEqual((ushort)0, messageEvent.Command);

        // Test setting and getting various values
        messageEvent.Command = 100;
        Assert.AreEqual((ushort)100, messageEvent.Command);

        messageEvent.Command = 65535; // Max ushort value
        Assert.AreEqual((ushort)65535, messageEvent.Command);

        messageEvent.Command = 0;
        Assert.AreEqual((ushort)0, messageEvent.Command);
    }

    [TestMethod]
    public unsafe void Test_InfoPtr_GetSet()
    {
        using var messageEvent = new MessageEvent();

        // Test initial value
        Assert.AreEqual(IntPtr.Zero, (IntPtr)messageEvent.InfoPtr);

        // Test setting to a non-null pointer
        var testValue = 42;
        var testPtr = &testValue;
        messageEvent.InfoPtr = testPtr;
        Assert.AreEqual((IntPtr)testPtr, (IntPtr)messageEvent.InfoPtr);

        // Test setting back to null
        messageEvent.InfoPtr = null;
        Assert.AreEqual(IntPtr.Zero, (IntPtr)messageEvent.InfoPtr);
    }

    [TestMethod]
    public unsafe void Test_Multiple_Properties()
    {
        using var messageEvent = new MessageEvent();

        var testValue = 123;
        var testPtr = &testValue;

        messageEvent.Command = 789;
        messageEvent.InfoPtr = testPtr;

        Assert.AreEqual((ushort)789, messageEvent.Command);
        Assert.AreEqual((IntPtr)testPtr, (IntPtr)messageEvent.InfoPtr);

        // Verify changing one property doesn't affect the other
        messageEvent.Command = 456;
        Assert.AreEqual((ushort)456, messageEvent.Command);
        Assert.AreEqual((IntPtr)testPtr, (IntPtr)messageEvent.InfoPtr);

        messageEvent.InfoPtr = null;
        Assert.AreEqual((ushort)456, messageEvent.Command);
        Assert.AreEqual(IntPtr.Zero, (IntPtr)messageEvent.InfoPtr);
    }
}
