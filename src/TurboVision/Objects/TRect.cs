using System.Runtime.InteropServices;

namespace TurboVision.Objects;

public unsafe partial class TRect(void* ptr, bool owned, bool placement)
    : NativeObject<TRect>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TRect_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TRect_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TRect_new(out var ptr));
            return ptr;
        }

        public unsafe void* PlacementNew2(byte* ptr, int ax, int ay, int bx, int by)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TRect_placementNew2(ptr, ax, ay, bx, by));
            return ptr;
        }

        public static unsafe void* New2(int ax, int ay, int bx, int by)
        {
            TurboVisionException.Check(NativeMethods.TV_TRect_new2(out var ptr, ax, ay, bx, by));
            return ptr;
        }

        public unsafe void* PlacementNew3(byte* ptr, void* p1, void* p2)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TRect_placementNew3(ptr, p1, p2));
            return ptr;
        }

        public static unsafe void* New3(void* p1, void* p2)
        {
            TurboVisionException.Check(NativeMethods.TV_TRect_new3(out var ptr, p1, p2));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TRect(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TRect()
        : this(Factory.New(), owned: true, placement: false) { }

    public TRect(byte* placement, int ax, int ay, int bx, int by)
        : this(
            Factory.Instance.PlacementNew2(placement, ax, ay, bx, by),
            owned: true,
            placement: true
        ) { }

    public TRect(int ax, int ay, int bx, int by)
        : this(Factory.New2(ax, ay, bx, by), owned: true, placement: false) { }

    public TRect(byte* placement, TPoint p1, TPoint p2)
        : this(
            Factory.Instance.PlacementNew3(placement, p1.Ptr, p2.Ptr),
            owned: true,
            placement: true
        ) { }

    public TRect(TPoint p1, TPoint p2)
        : this(Factory.New3(p1.Ptr, p2.Ptr), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TRect_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TRect_delete(Ptr));
    }

    protected override bool EqualsCore(TRect other)
    {
        TurboVisionException.Check(NativeMethods.TV_TRect_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TRect_hash(Ptr, out var hash));
        return hash;
    }

    public void Move(int aDX, int aDY)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_TRect_move(Ptr, aDX, aDY));
    }

    public void Grow(int aDX, int aDY)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_TRect_grow(Ptr, aDX, aDY));
    }

    public void Intersect(TRect r)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(r.IsDisposed, r);
        TurboVisionException.Check(NativeMethods.TV_TRect_intersect(Ptr, r.Ptr));
    }

    public void Union(TRect r)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(r.IsDisposed, r);
        TurboVisionException.Check(NativeMethods.TV_TRect_Union(Ptr, r.Ptr));
    }

    public bool Contains(TPoint p)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(p.IsDisposed, p);
        TurboVisionException.Check(NativeMethods.TV_TRect_contains(Ptr, p.Ptr, out var contains));
        return contains;
    }

    public bool IsEmpty()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_TRect_isEmpty(Ptr, out var isEmpty));
        return isEmpty;
    }

    public void GetA(TPoint dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_TRect_get_a(Ptr, dst.Ptr));
    }

    public void SetA(TPoint src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_TRect_set_a(Ptr, src.Ptr));
    }

    public void GetB(TPoint dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_TRect_get_b(Ptr, dst.Ptr));
    }

    public void SetB(TPoint src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_TRect_set_b(Ptr, src.Ptr));
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_placementNew2(
            byte* self,
            int ax,
            int ay,
            int bx,
            int by
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_new2(out void* @out, int ax, int ay, int bx, int by);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_placementNew3(byte* self, void* p1, void* p2);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_new3(out void* @out, void* p1, void* p2);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_move(void* self, int aDX, int aDY);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_grow(void* self, int aDX, int aDY);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_intersect(void* self, void* r);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_Union(void* self, void* r);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_contains(
            void* self,
            void* p,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_isEmpty(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_get_a(void* self, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_set_a(void* self, void* src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_get_b(void* self, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TRect_set_b(void* self, void* src);
    }
}
