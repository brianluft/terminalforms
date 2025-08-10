using System.Runtime.InteropServices;
using System.Text;

namespace TurboVision.System;

public partial class KeyDownEvent : IDisposable, IEquatable<KeyDownEvent>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public KeyDownEvent()
    {
        TurboVisionException.Check(NativeMethods.TV_KeyDownEvent_new(out var ptr));
        Ptr = ptr;
    }

    public KeyDownEvent(IntPtr ptr)
    {
        Ptr = ptr;
    }

    ~KeyDownEvent()
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
            TurboVisionException.Check(NativeMethods.TV_KeyDownEvent_delete(Ptr));
            IsDisposed = true;
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as KeyDownEvent);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        TurboVisionException.Check(NativeMethods.TV_KeyDownEvent_hash(Ptr, out var hash));
        return hash;
    }

    public bool Equals(KeyDownEvent? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);

        TurboVisionException.Check(
            NativeMethods.TV_KeyDownEvent_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    public ushort KeyCode
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_KeyDownEvent_get_keyCode(Ptr, out var keyCode)
            );
            return keyCode;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_KeyDownEvent_set_keyCode(Ptr, value));
        }
    }

    public byte CharCode
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_KeyDownEvent_get_charCode(Ptr, out var charCode)
            );
            return charCode;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_KeyDownEvent_set_charCode(Ptr, value));
        }
    }

    public byte ScanCode
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_KeyDownEvent_get_scanCode(Ptr, out var scanCode)
            );
            return scanCode;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_KeyDownEvent_set_scanCode(Ptr, value));
        }
    }

    public ushort ControlKeyState
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_KeyDownEvent_get_controlKeyState(Ptr, out var controlKeyState)
            );
            return controlKeyState;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_KeyDownEvent_set_controlKeyState(Ptr, value)
            );
        }
    }

    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            unsafe
            {
                TurboVisionException.Check(
                    NativeMethods.TV_KeyDownEvent_get_text(Ptr, out var textPtr, out var textLength)
                );
                return Encoding.UTF8.GetString(textPtr, textLength);
            }
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            var textBytes = Global.UTF8Encoding.GetBytes(value);
            if (textBytes.Length > byte.MaxValue)
                throw new ArgumentException("Text is too long.", nameof(value));
            TurboVisionException.Check(
                NativeMethods.TV_KeyDownEvent_set_text(Ptr, textBytes, (byte)textBytes.Length)
            );
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_new(out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_delete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_equals(
            IntPtr self,
            IntPtr other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_hash(IntPtr self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_get_keyCode(IntPtr self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_keyCode(IntPtr self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_get_charCode(IntPtr self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_charCode(IntPtr self, byte value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_get_scanCode(IntPtr self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_scanCode(IntPtr self, byte value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_get_controlKeyState(
            IntPtr self,
            out ushort @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_controlKeyState(IntPtr self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static unsafe partial Error TV_KeyDownEvent_get_text(
            IntPtr self,
            out byte* @out,
            out byte outTextLength
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_text(
            IntPtr self,
            Span<byte> value,
            byte textLength
        );
    }
}
