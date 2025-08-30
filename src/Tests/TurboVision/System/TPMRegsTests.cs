using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurboVision.System;

namespace Tests.TurboVision.System;

[TestClass]
public unsafe class TPMRegsTests
{
    [TestMethod]
    public void Test_PlacementNew()
    {
        var size = TPMRegs.PlacementSize;
        Assert.IsTrue(size > 0);

        var buffer = stackalloc byte[size];
        using var regs = new TPMRegs(buffer);

        Assert.IsFalse(regs.IsDisposed);
    }

    [TestMethod]
    public void Test_New()
    {
        using var regs = new TPMRegs();

        Assert.IsFalse(regs.IsDisposed);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var regs1 = new TPMRegs();
        using var regs2 = new TPMRegs();

        // Initially both should be equal (all fields default to 0)
        Assert.IsTrue(regs1.Equals(regs2));

        // Modify one field and they should no longer be equal
        regs1.Di = 42;
        Assert.IsFalse(regs1.Equals(regs2));

        // Set the same value in both and they should be equal again
        regs2.Di = 42;
        Assert.IsTrue(regs1.Equals(regs2));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var regs1 = new TPMRegs();
        using var regs2 = new TPMRegs();

        // Initially both should have the same hash code
        Assert.AreEqual(regs1.GetHashCode(), regs2.GetHashCode());

        // Modify one field and hash codes should likely be different
        regs1.Di = 42;
        Assert.AreNotEqual(regs1.GetHashCode(), regs2.GetHashCode());

        // Set the same value in both and hash codes should be equal again
        regs2.Di = 42;
        Assert.AreEqual(regs1.GetHashCode(), regs2.GetHashCode());
    }

    [TestMethod]
    public void Test_Properties()
    {
        using var regs = new TPMRegs();

        // Test all unsigned long properties
        regs.Di = 0x12345678;
        Assert.AreEqual(0x12345678u, regs.Di);

        regs.Si = 0x87654321;
        Assert.AreEqual(0x87654321u, regs.Si);

        regs.Bp = 0xABCDEF00;
        Assert.AreEqual(0xABCDEF00u, regs.Bp);

        regs.Dummy = 0x11111111;
        Assert.AreEqual(0x11111111u, regs.Dummy);

        regs.Bx = 0x22222222;
        Assert.AreEqual(0x22222222u, regs.Bx);

        regs.Dx = 0x33333333;
        Assert.AreEqual(0x33333333u, regs.Dx);

        regs.Cx = 0x44444444;
        Assert.AreEqual(0x44444444u, regs.Cx);

        regs.Ax = 0x55555555;
        Assert.AreEqual(0x55555555u, regs.Ax);

        // Test all unsigned properties
        regs.Flags = 0x66666666;
        Assert.AreEqual(0x66666666u, regs.Flags);

        regs.Es = 0x77777777;
        Assert.AreEqual(0x77777777u, regs.Es);

        regs.Ds = 0x88888888;
        Assert.AreEqual(0x88888888u, regs.Ds);

        regs.Fs = 0x99999999;
        Assert.AreEqual(0x99999999u, regs.Fs);

        regs.Gs = 0xAAAAAAAA;
        Assert.AreEqual(0xAAAAAAAAu, regs.Gs);

        regs.Ip = 0xBBBBBBBB;
        Assert.AreEqual(0xBBBBBBBBu, regs.Ip);

        regs.Cs = 0xCCCCCCCC;
        Assert.AreEqual(0xCCCCCCCCu, regs.Cs);

        regs.Sp = 0xDDDDDDDD;
        Assert.AreEqual(0xDDDDDDDDu, regs.Sp);

        regs.Ss = 0xEEEEEEEE;
        Assert.AreEqual(0xEEEEEEEEu, regs.Ss);
    }
}
