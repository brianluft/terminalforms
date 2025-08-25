using System.Runtime.InteropServices;

namespace TurboVision.Objects;

public unsafe partial class TPoint(void* ptr, bool owned, bool placement)
    : NativeObject<TPoint>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TPoint_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TPoint_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TPoint_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TPoint(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TPoint()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TPoint_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TPoint_delete(Ptr));
    }

    protected override bool EqualsCore(TPoint other)
    {
        TurboVisionException.Check(NativeMethods.TV_TPoint_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TPoint_hash(Ptr, out var hash));
        return hash;
    }

    public void Add(TPoint adder)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(adder.IsDisposed, adder);
        TurboVisionException.Check(NativeMethods.TV_TPoint_operator_add_in_place(Ptr, adder.Ptr));
    }

    public void Subtract(TPoint subber)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(subber.IsDisposed, subber);
        TurboVisionException.Check(
            NativeMethods.TV_TPoint_operator_subtract_in_place(Ptr, subber.Ptr)
        );
    }

    public static void Add(TPoint one, TPoint two, TPoint dst)
    {
        ObjectDisposedException.ThrowIf(one.IsDisposed, one);
        ObjectDisposedException.ThrowIf(two.IsDisposed, two);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_TPoint_operator_add(one.Ptr, two.Ptr, dst.Ptr));
    }

    public static void Subtract(TPoint one, TPoint two, TPoint dst)
    {
        ObjectDisposedException.ThrowIf(one.IsDisposed, one);
        ObjectDisposedException.ThrowIf(two.IsDisposed, two);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(
            NativeMethods.TV_TPoint_operator_subtract(one.Ptr, two.Ptr, dst.Ptr)
        );
    }

    public int X
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPoint_get_x(Ptr, out var x));
            return x;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPoint_set_x(Ptr, value));
        }
    }

    public int Y
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPoint_get_y(Ptr, out var y));
            return y;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TPoint_set_y(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_operator_add_in_place(void* self, void* adder);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_operator_subtract_in_place(void* self, void* subber);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_operator_add(void* one, void* two, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_operator_subtract(void* one, void* two, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_get_x(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_set_x(void* self, int x);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_get_y(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TPoint_set_y(void* self, int y);
    }
}
