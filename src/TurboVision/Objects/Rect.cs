using System.Runtime.InteropServices;

namespace TurboVision.Objects;

public partial class Rect(IntPtr ptr, bool owned, bool placement) : NativeObject<Rect>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_Rect_placementSize,
                NativeMethods.TV_Rect_placementNew,
                NativeMethods.TV_Rect_new
            )
        {
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public Rect(IntPtr placement) : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public Rect() : this(Factory.Instance.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(IntPtr ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_Rect_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_Rect_delete(Ptr));
    }

    protected override bool EqualsCore(Rect other)
    {
        TurboVisionException.Check(NativeMethods.TV_Rect_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_Rect_hash(Ptr, out var hash));
        return hash;
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

    public void GetA(Point dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_Rect_get_a(Ptr, dst.Ptr));
    }

    public void SetA(Point src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_Rect_set_a(Ptr, src.Ptr));
    }

    public void GetB(Point dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_Rect_get_b(Ptr, dst.Ptr));
    }

    public void SetB(Point src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_Rect_set_b(Ptr, src.Ptr));
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_placementNew(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_placementDelete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_new(out IntPtr @out);

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
        public static partial Error TV_Rect_get_a(IntPtr self, IntPtr dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_set_a(IntPtr self, IntPtr src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_get_b(IntPtr self, IntPtr dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_set_b(IntPtr self, IntPtr src);
    }
}
