using System.Runtime.InteropServices;

namespace TurboVision.Objects;

public unsafe partial class Point(void* ptr, bool owned, bool placement)
    : NativeObject<Point>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_Point_placementSize,
                NativeMethods.TV_Point_placementNew,
                NativeMethods.TV_Point_new
            ) { }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public Point(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public Point()
        : this(Factory.Instance.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_Point_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_Point_delete(Ptr));
    }

    protected override bool EqualsCore(Point other)
    {
        TurboVisionException.Check(NativeMethods.TV_Point_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_Point_hash(Ptr, out var hash));
        return hash;
    }

    public void Add(Point adder)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(adder.IsDisposed, adder);
        TurboVisionException.Check(NativeMethods.TV_Point_operator_add_in_place(Ptr, adder.Ptr));
    }

    public void Subtract(Point subber)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(subber.IsDisposed, subber);
        TurboVisionException.Check(
            NativeMethods.TV_Point_operator_subtract_in_place(Ptr, subber.Ptr)
        );
    }

    public static void Add(Point one, Point two, Point dst)
    {
        ObjectDisposedException.ThrowIf(one.IsDisposed, one);
        ObjectDisposedException.ThrowIf(two.IsDisposed, two);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_Point_operator_add(one.Ptr, two.Ptr, dst.Ptr));
    }

    public static void Subtract(Point one, Point two, Point dst)
    {
        ObjectDisposedException.ThrowIf(one.IsDisposed, one);
        ObjectDisposedException.ThrowIf(two.IsDisposed, two);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(
            NativeMethods.TV_Point_operator_subtract(one.Ptr, two.Ptr, dst.Ptr)
        );
    }

    public int X
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Point_get_x(Ptr, out var x));
            return x;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Point_set_x(Ptr, value));
        }
    }

    public int Y
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Point_get_y(Ptr, out var y));
            return y;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Point_set_y(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_operator_add_in_place(void* self, void* adder);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_operator_subtract_in_place(void* self, void* subber);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_operator_add(void* one, void* two, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_operator_subtract(void* one, void* two, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_get_x(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_set_x(void* self, int x);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_get_y(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_set_y(void* self, int y);
    }
}
