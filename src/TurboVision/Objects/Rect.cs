using System.Runtime.InteropServices;

namespace TurboVision.Objects;

public partial class Rect : IDisposable, IEquatable<Rect>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public Rect()
    {
        TurboVisionException.Check(NativeMethods.TV_Rect_new(out var ptr));
        Ptr = ptr;
    }

    public Rect(int ax, int ay, int bx, int by)
    {
        TurboVisionException.Check(NativeMethods.TV_Rect_new2(ax, ay, bx, by, out var ptr));
        Ptr = ptr;
    }

    public Rect(Point p1, Point p2)
    {
        TurboVisionException.Check(NativeMethods.TV_Rect_new3(p1.Ptr, p2.Ptr, out var ptr));
        Ptr = ptr;
    }

    public Rect(IntPtr ptr)
    {
        Ptr = ptr;
    }

    ~Rect()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            TurboVisionException.Check(NativeMethods.TV_Rect_delete(Ptr));
            IsDisposed = true;
        }
    }

    public void Move(int aDX, int aDY)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_Rect_move(Ptr, aDX, aDY));
    }

    public void Grow(int aDX, int aDY)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_Rect_grow(Ptr, aDX, aDY));
    }

    public void Intersect(Rect r)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(r.IsDisposed, r);
        TurboVisionException.Check(NativeMethods.TV_Rect_intersect(Ptr, r.Ptr));
    }

    public void Union(Rect r)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(r.IsDisposed, r);
        TurboVisionException.Check(NativeMethods.TV_Rect_Union(Ptr, r.Ptr));
    }

    public bool Contains(Point p)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(p.IsDisposed, p);
        TurboVisionException.Check(NativeMethods.TV_Rect_contains(Ptr, p.Ptr, out var contains));
        return contains;
    }

    public bool IsEmpty()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_Rect_isEmpty(Ptr, out var isEmpty));
        return isEmpty;
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as Rect);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_Rect_hash(Ptr, out var hash));
        return hash;
    }

    public bool Equals(Rect? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);
        TurboVisionException.Check(NativeMethods.TV_Rect_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    public Point A
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Rect_get_a(Ptr, out var a));
            return new Point(a);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ObjectDisposedException.ThrowIf(value.IsDisposed, value);
            TurboVisionException.Check(NativeMethods.TV_Rect_set_a(Ptr, value.Ptr));
        }
    }

    public Point B
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Rect_get_b(Ptr, out var b));
            return new Point(b);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ObjectDisposedException.ThrowIf(value.IsDisposed, value);
            TurboVisionException.Check(NativeMethods.TV_Rect_set_b(Ptr, value.Ptr));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_new(out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_new2(int ax, int ay, int bx, int by, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_new3(IntPtr p1, IntPtr p2, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_delete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_equals(
            IntPtr self,
            IntPtr other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_hash(IntPtr self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_move(IntPtr self, int aDX, int aDY);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_grow(IntPtr self, int aDX, int aDY);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_intersect(IntPtr self, IntPtr r);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_Union(IntPtr self, IntPtr r);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_contains(
            IntPtr self,
            IntPtr p,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_isEmpty(
            IntPtr self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_get_a(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_get_b(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_set_a(IntPtr self, IntPtr p);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_set_b(IntPtr self, IntPtr p);
    }
}
