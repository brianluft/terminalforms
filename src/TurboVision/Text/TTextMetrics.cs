using System.Runtime.InteropServices;

namespace TurboVision.Text;

public unsafe partial class TTextMetrics(void* ptr, bool owned, bool placement)
    : NativeObject<TTextMetrics>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TTextMetrics_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TTextMetrics_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TTextMetrics_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TTextMetrics(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TTextMetrics()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TTextMetrics_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TTextMetrics_delete(Ptr));
    }

    protected override bool EqualsCore(TTextMetrics other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TTextMetrics_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TTextMetrics_hash(Ptr, out var hash));
        return hash;
    }

    public uint Width
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TTextMetrics_get_width(Ptr, out var width));
            return width;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TTextMetrics_set_width(Ptr, value));
        }
    }

    public uint CharacterCount
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TTextMetrics_get_characterCount(Ptr, out var characterCount)
            );
            return characterCount;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TTextMetrics_set_characterCount(Ptr, value)
            );
        }
    }

    public uint GraphemeCount
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TTextMetrics_get_graphemeCount(Ptr, out var graphemeCount)
            );
            return graphemeCount;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TTextMetrics_set_graphemeCount(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_get_width(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_set_width(void* self, uint width);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_get_characterCount(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_set_characterCount(
            void* self,
            uint characterCount
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_get_graphemeCount(void* self, out uint @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TTextMetrics_set_graphemeCount(
            void* self,
            uint graphemeCount
        );
    }
}
