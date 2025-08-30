using System.Runtime.InteropServices;

namespace TurboVision.Dialogs;

public unsafe partial class TSearchRec(void* ptr, bool owned, bool placement)
    : NativeObject<TSearchRec>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TSearchRec_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TSearchRec(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TSearchRec()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TSearchRec_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TSearchRec_delete(Ptr));
    }

    protected override bool EqualsCore(TSearchRec other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TSearchRec_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TSearchRec_hash(Ptr, out var hash));
        return hash;
    }

    public byte Attr
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_get_attr(Ptr, out var attr));
            return attr;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_set_attr(Ptr, value));
        }
    }

    public int Time
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_get_time(Ptr, out var time));
            return time;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_set_time(Ptr, value));
        }
    }

    public int Size
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_get_size(Ptr, out var size));
            return size;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_set_size(Ptr, value));
        }
    }

    public string Name
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_get_name(Ptr, out var name));
            return Marshal.PtrToStringUTF8((IntPtr)name)!;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TSearchRec_set_name(Ptr, value));
        }
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_get_attr(void* self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_set_attr(void* self, byte attr);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_get_time(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_set_time(void* self, int time);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_get_size(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_set_size(void* self, int size);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TSearchRec_get_name(void* self, out byte* @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TSearchRec_set_name(void* self, string value);
    }
}
