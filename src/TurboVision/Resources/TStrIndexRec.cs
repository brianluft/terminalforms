using System.Runtime.InteropServices;

namespace TurboVision.Resources;

public unsafe partial class TStrIndexRec(void* ptr, bool owned, bool placement)
    : NativeObject<TStrIndexRec>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TStrIndexRec_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TStrIndexRec(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TStrIndexRec()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_delete(Ptr));
    }

    protected override bool EqualsCore(TStrIndexRec other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TStrIndexRec_equals(Ptr, other.Ptr, out var result)
        );
        return result;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_hash(Ptr, out var hash));
        return hash;
    }

    public ushort Key
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_get_key(Ptr, out var key));
            return key;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_set_key(Ptr, value));
        }
    }

    public ushort Count
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_get_count(Ptr, out var count));
            return count;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_set_count(Ptr, value));
        }
    }

    public ushort Offset
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TStrIndexRec_get_offset(Ptr, out var offset)
            );
            return offset;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TStrIndexRec_set_offset(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_get_key(void* self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_set_key(void* self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_get_count(void* self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_set_count(void* self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_get_offset(void* self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TStrIndexRec_set_offset(void* self, ushort value);
    }
}
