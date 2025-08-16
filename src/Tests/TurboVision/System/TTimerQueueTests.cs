using TurboVision.System;

namespace Tests.TurboVision.System;

[TestClass]
public sealed class TTimerQueueTests
{
    [TestMethod]
    public unsafe void Test_PlacementNew()
    {
        var bytes = stackalloc byte[TTimerQueue.PlacementSize];
        using var timerQueue = new TTimerQueue(bytes);
        Assert.IsNotNull(timerQueue);
        Assert.IsFalse(timerQueue.IsDisposed);
    }

    [TestMethod]
    public void Test_New()
    {
        using var timerQueue = new TTimerQueue();
        Assert.IsNotNull(timerQueue);
        Assert.IsFalse(timerQueue.IsDisposed);
    }

    [TestMethod]
    public void Test_Equals()
    {
        using var a = new TTimerQueue();
        using var b = new TTimerQueue();

        // Different instances should not be equal for stateful objects
        Assert.IsFalse(a.Equals(b));

        // Self-equality should work
        Assert.IsTrue(a.Equals(a));

        // Null comparison
        Assert.IsFalse(a.Equals(null));
        Assert.IsFalse(a.Equals(new object()));
    }

    [TestMethod]
    public void Test_GetHashCode()
    {
        using var a = new TTimerQueue();
        using var b = new TTimerQueue();

        // Different instances should have different hash codes for stateful objects
        Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());

        // Same instance should have consistent hash codes
        var hashA1 = a.GetHashCode();
        var hashA2 = a.GetHashCode();
        Assert.AreEqual(hashA1, hashA2);
    }
}
