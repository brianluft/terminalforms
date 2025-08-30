using System.Runtime.InteropServices;

namespace TurboVision.Resources;

public unsafe partial class TResourceItem(void* ptr, bool owned, bool placement)
    : NativeObject<TResourceItem>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TResourceItem_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TResourceItem_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TResourceItem_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TResourceItem(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TResourceItem()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TResourceItem_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TResourceItem_delete(Ptr));
    }

    protected override bool EqualsCore(TResourceItem other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TResourceItem_equals(Ptr, other.Ptr, out var result)
        );
        return result;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TResourceItem_hash(Ptr, out var hash));
        return hash;
    }

    public int Pos
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TResourceItem_get_pos(Ptr, out var pos));
            return pos;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TResourceItem_set_pos(Ptr, value));
        }
    }

    public int Size
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TResourceItem_get_size(Ptr, out var size));
            return size;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TResourceItem_set_size(Ptr, value));
        }
    }

    public string Key
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TResourceItem_get_key(Ptr, out var key));
            return Marshal.PtrToStringUTF8((nint)key) ?? string.Empty;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TResourceItem_set_key(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_get_pos(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_set_pos(void* self, int value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_get_size(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_set_size(void* self, int value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TResourceItem_get_key(void* self, out byte* @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TResourceItem_set_key(void* self, string value);
    }
}
