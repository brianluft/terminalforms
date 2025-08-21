using System.Runtime.InteropServices;

namespace TurboVision.Views;

public unsafe partial class WriteArgs(void* ptr, bool owned, bool placement)
    : NativeObject<WriteArgs>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_write_args_placementSize,
                NativeMethods.TV_write_args_placementNew,
                NativeMethods.TV_write_args_new
            ) { }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public WriteArgs(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public WriteArgs()
        : this(Factory.Instance.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_write_args_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_write_args_delete(Ptr));
    }

    protected override bool EqualsCore(WriteArgs other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_write_args_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_write_args_hash(Ptr, out var hash));
        return hash;
    }

    public void* Self
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_write_args_get_self(Ptr, out var self));
            return self;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_write_args_set_self(Ptr, value));
        }
    }

    public void* Target
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_write_args_get_target(Ptr, out var target));
            return target;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_write_args_set_target(Ptr, value));
        }
    }

    public void* Buf
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_write_args_get_buf(Ptr, out var buf));
            return buf;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_write_args_set_buf(Ptr, value));
        }
    }

    public ushort Offset
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_write_args_get_offset(Ptr, out var offset));
            return offset;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_write_args_set_offset(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_get_self(void* self, out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_set_self(void* self, void* value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_get_target(void* self, out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_set_target(void* self, void* value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_get_buf(void* self, out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_set_buf(void* self, void* value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_get_offset(void* self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_write_args_set_offset(void* self, ushort value);
    }
}
