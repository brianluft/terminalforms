using System.Runtime.InteropServices;

namespace TurboVision.System;

public unsafe partial class TTimerQueue(void* ptr, bool owned, bool placement)
    : NativeObject<TTimerQueue>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_TTimerQueue_placementSize,
                NativeMethods.TV_TTimerQueue_placementNew,
                NativeMethods.TV_TTimerQueue_new
            ) { }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TTimerQueue(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TTimerQueue()
        : this(Factory.Instance.New(), owned: true, placement: false) { }

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
    }
}
