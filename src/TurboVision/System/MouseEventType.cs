using System.Runtime.InteropServices;
using TurboVision.Objects;

namespace TurboVision.System;

public partial class MouseEventType(IntPtr ptr, bool owned, bool placement) : NativeObject<MouseEventType>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_MouseEventType_placementSize,
                NativeMethods.TV_MouseEventType_placementNew,
                NativeMethods.TV_MouseEventType_new
            )
        {
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public MouseEventType(IntPtr placement) : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public MouseEventType() : this(Factory.Instance.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(IntPtr ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_MouseEventType_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_MouseEventType_delete(Ptr));
    }

    protected override bool EqualsCore(MouseEventType other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_MouseEventType_equals(Ptr, other.Ptr, out var result)
        );
        return result;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_MouseEventType_hash(Ptr, out var hash));
        return hash;
    }

    public void GetWhere(Point dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_MouseEventType_get_where(Ptr, dst.Ptr));
    }

    public void SetWhere(Point src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_MouseEventType_set_where(Ptr, src.Ptr));
    }

    public ushort EventFlags
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_MouseEventType_get_eventFlags(Ptr, out var eventFlags)
            );
            return eventFlags;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_MouseEventType_set_eventFlags(Ptr, value));
        }
    }

    public ushort ControlKeyState
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_MouseEventType_get_controlKeyState(Ptr, out var controlKeyState)
            );
            return controlKeyState;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_MouseEventType_set_controlKeyState(Ptr, value)
            );
        }
    }

    public byte Buttons
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_MouseEventType_get_buttons(Ptr, out var buttons)
            );
            return buttons;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_MouseEventType_set_buttons(Ptr, value));
        }
    }

    public byte Wheel
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_MouseEventType_get_wheel(Ptr, out var wheel)
            );
            return wheel;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_MouseEventType_set_wheel(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_placementNew(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_placementDelete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_new(out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_delete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_equals(
            IntPtr self,
            IntPtr other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_hash(IntPtr self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_where(IntPtr self, IntPtr dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_where(IntPtr self, IntPtr src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_eventFlags(IntPtr self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_eventFlags(IntPtr self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_controlKeyState(
            IntPtr self,
            out ushort @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_controlKeyState(
            IntPtr self,
            ushort value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_buttons(IntPtr self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_buttons(IntPtr self, byte value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_wheel(IntPtr self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_wheel(IntPtr self, byte value);
    }
}
