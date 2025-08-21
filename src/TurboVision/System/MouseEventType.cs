using System.Runtime.InteropServices;
using TurboVision.Objects;

namespace TurboVision.System;

public unsafe partial class MouseEventType(void* ptr, bool owned, bool placement)
    : NativeObject<MouseEventType>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_MouseEventType_placementSize,
                NativeMethods.TV_MouseEventType_placementNew,
                NativeMethods.TV_MouseEventType_new
            ) { }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public MouseEventType(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public MouseEventType()
        : this(Factory.Instance.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
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

    public void GetWhere(TPoint dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_MouseEventType_get_where(Ptr, dst.Ptr));
    }

    public void SetWhere(TPoint src)
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
        public static partial Error TV_MouseEventType_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_where(void* self, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_where(void* self, void* src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_eventFlags(void* self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_eventFlags(void* self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_controlKeyState(
            void* self,
            out ushort @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_controlKeyState(void* self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_buttons(void* self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_buttons(void* self, byte value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_get_wheel(void* self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_wheel(void* self, byte value);
    }
}
