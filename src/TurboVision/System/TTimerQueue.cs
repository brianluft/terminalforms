using System.Runtime.InteropServices;

namespace TurboVision.System;

public unsafe partial class TTimerQueue(void* ptr, bool owned, bool placement)
    : NativeObject<TTimerQueue>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TTimerQueue_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TTimerQueue_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TTimerQueue_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TTimerQueue(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TTimerQueue()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TTimerQueue_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TTimerQueue_delete(Ptr));
    }

    protected override bool EqualsCore(TTimerQueue other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TTimerQueue_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TTimerQueue_hash(Ptr, out var hash));
        return hash;
    }

    public unsafe void* SetTimer(uint timeoutMs, int periodMs)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TTimerQueue_setTimer(Ptr, timeoutMs, periodMs, out var id)
        );
        return id;
    }

    public unsafe void KillTimer(void* id)
    {
        TurboVisionException.Check(NativeMethods.TV_TTimerQueue_killTimer(Ptr, id));
    }

    public int TimeUntilNextTimeout()
    {
        TurboVisionException.Check(
            NativeMethods.TV_TTimerQueue_timeUntilNextTimeout(Ptr, out var time)
        );
        return time;
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_setTimer(
            void* self,
            uint timeoutMs,
            int periodMs,
            out void* @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_killTimer(void* self, void* id);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTimerQueue_timeUntilNextTimeout(void* self, out int @out);
    }
}
