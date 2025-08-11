using TurboVision.Views;

namespace Tests.TurboVision.Views;

[TestClass]
public sealed class WriteArgsTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[WriteArgs.PlacementSize];
        using var writeArgs = new WriteArgs(bytes);

        // Test that the object was created successfully
        Assert.IsNotNull(writeArgs);
        Assert.IsFalse(writeArgs.IsDisposed);
    }

    [TestMethod]
    public void Test_New()
    {
        using var writeArgs = new WriteArgs();

        // Test that the object was created successfully
        Assert.IsNotNull(writeArgs);
        Assert.IsFalse(writeArgs.IsDisposed);
    }

    [TestMethod]
    public unsafe void Test_Equals()
    {
        using var a = new WriteArgs();
        using var b = new WriteArgs();
        using var c = new WriteArgs();

        // Set same values for a and c
        var selfPtr = (void*)0x1000;
        var targetPtr = (void*)0x2000;
        var bufPtr = (void*)0x3000;
        ushort offset = 42;

        a.Self = selfPtr;
        a.Target = targetPtr;
        a.Buf = bufPtr;
        a.Offset = offset;

        c.Self = selfPtr;
        c.Target = targetPtr;
        c.Buf = bufPtr;
        c.Offset = offset;

        // Test equality
        Assert.IsTrue(a.Equals(c));
        Assert.IsFalse(a.Equals(b));
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public unsafe void Test_GetHashCode()
    {
        using var a = new WriteArgs();
        using var b = new WriteArgs();
        using var c = new WriteArgs();

        // Set same values for a and c
        var selfPtr = (void*)0x1000;
        var targetPtr = (void*)0x2000;
        var bufPtr = (void*)0x3000;
        ushort offset = 42;

        a.Self = selfPtr;
        a.Target = targetPtr;
        a.Buf = bufPtr;
        a.Offset = offset;

        c.Self = selfPtr;
        c.Target = targetPtr;
        c.Buf = bufPtr;
        c.Offset = offset;

        // Test hash codes
        Assert.AreEqual(a.GetHashCode(), c.GetHashCode());
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public unsafe void Test_Self_Property()
    {
        using var writeArgs = new WriteArgs();
        var testPtr = (void*)0x12345678;

        writeArgs.Self = testPtr;
        Assert.AreEqual((nint)testPtr, (nint)writeArgs.Self);
    }

    [TestMethod]
    public unsafe void Test_Target_Property()
    {
        using var writeArgs = new WriteArgs();
        var testPtr = (void*)0x87654321;

        writeArgs.Target = testPtr;
        Assert.AreEqual((nint)testPtr, (nint)writeArgs.Target);
    }

    [TestMethod]
    public unsafe void Test_Buf_Property()
    {
        using var writeArgs = new WriteArgs();
        var testPtr = (void*)0xABCDEF01;

        writeArgs.Buf = testPtr;
        Assert.AreEqual((nint)testPtr, (nint)writeArgs.Buf);
    }

    [TestMethod]
    public void Test_Offset_Property()
    {
        using var writeArgs = new WriteArgs();
        ushort testOffset = 1234;

        writeArgs.Offset = testOffset;
        Assert.AreEqual(testOffset, writeArgs.Offset);
    }

    [TestMethod]
    public unsafe void Test_All_Properties_Together()
    {
        using var writeArgs = new WriteArgs();

        var selfPtr = (void*)0x1111;
        var targetPtr = (void*)0x2222;
        var bufPtr = (void*)0x3333;
        ushort offset = 9999;

        // Set all properties
        writeArgs.Self = selfPtr;
        writeArgs.Target = targetPtr;
        writeArgs.Buf = bufPtr;
        writeArgs.Offset = offset;

        // Verify all properties
        Assert.AreEqual((nint)selfPtr, (nint)writeArgs.Self);
        Assert.AreEqual((nint)targetPtr, (nint)writeArgs.Target);
        Assert.AreEqual((nint)bufPtr, (nint)writeArgs.Buf);
        Assert.AreEqual(offset, writeArgs.Offset);
    }
}
