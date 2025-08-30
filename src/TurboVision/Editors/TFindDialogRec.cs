using System.Runtime.InteropServices;

namespace TurboVision.Editors;

public unsafe partial class TFindDialogRec(void* ptr, bool owned, bool placement)
    : NativeObject<TFindDialogRec>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TFindDialogRec_placementSize) { }

        public unsafe void* PlacementNew2(byte* ptr, string str, ushort flgs)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(
                NativeMethods.TV_TFindDialogRec_placementNew2(ptr, str, flgs)
            );
            return ptr;
        }

        public static unsafe void* New2(string str, ushort flgs)
        {
            TurboVisionException.Check(
                NativeMethods.TV_TFindDialogRec_new2(out var ptr, str, flgs)
            );
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TFindDialogRec(byte* placement, string str, ushort flgs)
        : this(Factory.Instance.PlacementNew2(placement, str, flgs), owned: true, placement: true)
    { }

    public TFindDialogRec(string str, ushort flgs)
        : this(Factory.New2(str, flgs), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TFindDialogRec_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TFindDialogRec_delete(Ptr));
    }

    protected override bool EqualsCore(TFindDialogRec other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TFindDialogRec_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TFindDialogRec_hash(Ptr, out var hash));
        return hash;
    }

    public string Find
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TFindDialogRec_get_find(Ptr, out var find));
            return Marshal.PtrToStringUTF8((IntPtr)find)!;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TFindDialogRec_set_find(Ptr, value));
        }
    }

    public ushort Options
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TFindDialogRec_get_options(Ptr, out var options)
            );
            return options;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TFindDialogRec_set_options(Ptr, value));
        }
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TFindDialogRec_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TFindDialogRec_placementNew2(
            byte* self,
            string str,
            ushort flgs
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TFindDialogRec_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TFindDialogRec_new2(out void* @out, string str, ushort flgs);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TFindDialogRec_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TFindDialogRec_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TFindDialogRec_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TFindDialogRec_get_find(void* self, out byte* @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TFindDialogRec_set_find(void* self, string value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TFindDialogRec_get_options(void* self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TFindDialogRec_set_options(void* self, ushort options);
    }
}
