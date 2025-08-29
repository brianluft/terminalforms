using System.Runtime.InteropServices;

namespace TurboVision.Dialogs;

public unsafe partial class TDirEntry(void* ptr, bool owned, bool placement)
    : NativeObject<TDirEntry>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TDirEntry_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr, string text, string dir)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TDirEntry_placementNew2(ptr, text, dir));
            return ptr;
        }

        public static unsafe void* New(string text, string dir)
        {
            TurboVisionException.Check(NativeMethods.TV_TDirEntry_new2(out var ptr, text, dir));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TDirEntry(byte* placement, string text, string dir)
        : this(Factory.Instance.PlacementNew(placement, text, dir), owned: true, placement: true)
    { }

    public TDirEntry(string text, string dir)
        : this(Factory.New(text, dir), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TDirEntry_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TDirEntry_delete(Ptr));
    }

    protected override bool EqualsCore(TDirEntry other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TDirEntry_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TDirEntry_hash(Ptr, out var hash));
        return hash;
    }

    public string? Directory
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TDirEntry_dir(Ptr, out var dirPtr));
            return Marshal.PtrToStringUTF8((IntPtr)dirPtr);
        }
    }

    public string? Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TDirEntry_text(Ptr, out var textPtr));
            return Marshal.PtrToStringUTF8((IntPtr)textPtr);
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TDirEntry_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TDirEntry_placementNew2(byte* self, string text, string dir);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TDirEntry_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TDirEntry_new2(out void* @out, string text, string dir);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TDirEntry_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TDirEntry_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TDirEntry_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TDirEntry_dir(void* self, out byte* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TDirEntry_text(void* self, out byte* @out);
    }
}
