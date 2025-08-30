using System.Runtime.InteropServices;

namespace TurboVision.HelpBase;

public unsafe partial class TParagraph(void* ptr, bool owned, bool placement)
    : NativeObject<TParagraph>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TParagraph_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TParagraph_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TParagraph_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TParagraph(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TParagraph()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TParagraph_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TParagraph_delete(Ptr));
    }

    protected override bool EqualsCore(TParagraph other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TParagraph_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TParagraph_hash(Ptr, out var hash));
        return hash;
    }

    public TParagraph? Next
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TParagraph_get_next(Ptr, out var result));
            return result != null ? new TParagraph(result, owned: false, placement: false) : null;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            void* ptr = value != null ? value.Ptr : null;
            TurboVisionException.Check(NativeMethods.TV_TParagraph_set_next(Ptr, ptr));
        }
    }

    public bool Wrap
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TParagraph_get_wrap(Ptr, out var wrap));
            return wrap;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TParagraph_set_wrap(Ptr, value));
        }
    }

    public ushort Size
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TParagraph_get_size(Ptr, out var size));
            return size;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TParagraph_set_size(Ptr, value));
        }
    }

    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TParagraph_get_text(Ptr, out var result));
            return Marshal.PtrToStringUTF8((nint)result) ?? string.Empty;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TParagraph_set_text(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_get_next(void* self, out void* result);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_set_next(void* self, void* value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_get_wrap(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool result
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_set_wrap(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_get_size(void* self, out ushort result);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_set_size(void* self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TParagraph_get_text(void* self, out byte* result);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TParagraph_set_text(void* self, string value);
    }
}
