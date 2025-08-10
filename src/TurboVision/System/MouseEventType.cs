using System.Runtime.InteropServices;
using TurboVision.Objects;

namespace TurboVision.System;

public partial class MouseEventType : IDisposable, IEquatable<MouseEventType>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public MouseEventType()
    {
        TurboVisionException.Check(NativeMethods.TV_MouseEventType_new(out var ptr));
        Ptr = ptr;
    }

    public MouseEventType(IntPtr ptr)
    {
        Ptr = ptr;
    }

    ~MouseEventType()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            TurboVisionException.Check(NativeMethods.TV_MouseEventType_delete(Ptr));
            IsDisposed = true;
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as MouseEventType);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        TurboVisionException.Check(NativeMethods.TV_MouseEventType_hash(Ptr, out var hash));
        return hash;
    }

    public bool Equals(MouseEventType? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        if (other.IsDisposed)
            return false;

        TurboVisionException.Check(
            NativeMethods.TV_MouseEventType_equals(Ptr, other.Ptr, out var result)
        );
        return result;
    }

    public Point Where
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_MouseEventType_get_where(Ptr, out var where)
            );
            return new Point(where);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ArgumentNullException.ThrowIfNull(value);
            ObjectDisposedException.ThrowIf(value.IsDisposed, value);
            TurboVisionException.Check(NativeMethods.TV_MouseEventType_set_where(Ptr, value.Ptr));
        }
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
        public static partial Error TV_MouseEventType_get_where(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MouseEventType_set_where(IntPtr self, IntPtr value);

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
