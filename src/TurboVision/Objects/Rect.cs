using System.Runtime.InteropServices;

namespace TurboVision.Objects;

public unsafe partial class Rect(void* ptr, bool owned, bool placement)
    : NativeObject<Rect>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_Rect_placementSize,
                NativeMethods.TV_Rect_placementNew,
                NativeMethods.TV_Rect_new
            ) { }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public Rect(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public Rect()
        : this(Factory.Instance.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
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
        public static partial Error TV_Rect_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_move(void* self, int aDX, int aDY);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_grow(void* self, int aDX, int aDY);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_intersect(void* self, void* r);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_Union(void* self, void* r);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_contains(
            void* self,
            void* p,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_isEmpty(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_get_a(void* self, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_set_a(void* self, void* src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_get_b(void* self, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Rect_set_b(void* self, void* src);
    }
}
