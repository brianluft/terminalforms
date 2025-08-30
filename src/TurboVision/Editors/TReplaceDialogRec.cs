using System.Runtime.InteropServices;

namespace TurboVision.Editors;

public unsafe partial class TReplaceDialogRec(void* ptr, bool owned, bool placement)
    : NativeObject<TReplaceDialogRec>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TReplaceDialogRec_placementSize) { }

        public unsafe void* PlacementNew2(byte* ptr, string str, string rep, ushort flgs)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(
                NativeMethods.TV_TReplaceDialogRec_placementNew2(ptr, str, rep, flgs)
            );
            return ptr;
        }

        public static unsafe void* New2(string str, string rep, ushort flgs)
        {
            TurboVisionException.Check(
                NativeMethods.TV_TReplaceDialogRec_new2(out var ptr, str, rep, flgs)
            );
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TReplaceDialogRec(byte* placement, string str, string rep, ushort flgs)
        : this(
            Factory.Instance.PlacementNew2(placement, str, rep, flgs),
            owned: true,
            placement: true
        ) { }

    public TReplaceDialogRec(string str, string rep, ushort flgs)
        : this(Factory.New2(str, rep, flgs), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TReplaceDialogRec_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TReplaceDialogRec_delete(Ptr));
    }

    protected override bool EqualsCore(TReplaceDialogRec other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TReplaceDialogRec_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TReplaceDialogRec_hash(Ptr, out var hash));
        return hash;
    }

    public string Find
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TReplaceDialogRec_get_find(Ptr, out var find)
            );
            return Marshal.PtrToStringUTF8((IntPtr)find)!;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TReplaceDialogRec_set_find(Ptr, value));
        }
    }

    public string Replace
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TReplaceDialogRec_get_replace(Ptr, out var replace)
            );
            return Marshal.PtrToStringUTF8((IntPtr)replace)!;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TReplaceDialogRec_set_replace(Ptr, value));
        }
    }

    public ushort Options
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TReplaceDialogRec_get_options(Ptr, out var options)
            );
            return options;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TReplaceDialogRec_set_options(Ptr, value));
        }
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TReplaceDialogRec_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TReplaceDialogRec_placementNew2(
            byte* self,
            string str,
            string rep,
            ushort flgs
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TReplaceDialogRec_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TReplaceDialogRec_new2(
            out void* @out,
            string str,
            string rep,
            ushort flgs
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TReplaceDialogRec_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TReplaceDialogRec_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TReplaceDialogRec_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TReplaceDialogRec_get_find(void* self, out byte* @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TReplaceDialogRec_set_find(void* self, string value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TReplaceDialogRec_get_replace(void* self, out byte* @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TReplaceDialogRec_set_replace(void* self, string value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TReplaceDialogRec_get_options(void* self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TReplaceDialogRec_set_options(void* self, ushort options);
    }
}
