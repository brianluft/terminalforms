using System.Runtime.InteropServices;

namespace TurboVision.Objects;

public partial class Point : NativeObject<Point>
{
    public Point()
        : base(New(), owned: true) { }

    private static IntPtr New()
    {
        TurboVisionException.Check(NativeMethods.TV_Point_new(out var ptr));
        return ptr;
    }

    internal Point(IntPtr ptr, bool owned)
        : base(ptr, owned) { }

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

    public static Point operator +(Point one, Point two)
    {
        ObjectDisposedException.ThrowIf(one.IsDisposed, one);
        ObjectDisposedException.ThrowIf(two.IsDisposed, two);
        TurboVisionException.Check(
            NativeMethods.TV_Point_operator_add(one.Ptr, two.Ptr, out var ptr)
        );
        return new Point(ptr, owned: true);
    }

    public static Point operator -(Point one, Point two)
    {
        ObjectDisposedException.ThrowIf(one.IsDisposed, one);
        ObjectDisposedException.ThrowIf(two.IsDisposed, two);
        TurboVisionException.Check(
            NativeMethods.TV_Point_operator_subtract(one.Ptr, two.Ptr, out var ptr)
        );
        return new Point(ptr, owned: true);
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
        public static partial Error TV_Point_new(out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_delete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_equals(
            IntPtr self,
            IntPtr other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_hash(IntPtr self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_operator_add_in_place(IntPtr self, IntPtr adder);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_operator_subtract_in_place(IntPtr self, IntPtr subber);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_operator_add(IntPtr one, IntPtr two, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_operator_subtract(
            IntPtr one,
            IntPtr two,
            out IntPtr @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_get_x(IntPtr self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_set_x(IntPtr self, int x);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_get_y(IntPtr self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Point_set_y(IntPtr self, int y);
    }
}
