using System.Runtime.InteropServices;
using System.Text;

namespace TerminalForms;

public sealed class KeyDownEvent : IDisposable, IEquatable<KeyDownEvent>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public KeyDownEvent()
    {
        TerminalFormsException.Check(NativeMethods.TV_KeyDownEvent_new(out var ptr));
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
            TerminalFormsException.Check(NativeMethods.TV_KeyDownEvent_delete(Ptr));
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

        TerminalFormsException.Check(NativeMethods.TV_KeyDownEvent_hash(Ptr, out var hash));
        return hash;
    }

    public bool Equals(KeyDownEvent? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);

        TerminalFormsException.Check(
            NativeMethods.TV_KeyDownEvent_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    public ushort KeyCode
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(
                NativeMethods.TV_KeyDownEvent_get_keyCode(Ptr, out var keyCode)
            );
            return keyCode;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_KeyDownEvent_set_keyCode(Ptr, value));
        }
    }

    public byte CharCode
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(
                NativeMethods.TV_KeyDownEvent_get_charCode(Ptr, out var charCode)
            );
            return charCode;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_KeyDownEvent_set_charCode(Ptr, value));
        }
    }

    public byte ScanCode
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(
                NativeMethods.TV_KeyDownEvent_get_scanCode(Ptr, out var scanCode)
            );
            return scanCode;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_KeyDownEvent_set_scanCode(Ptr, value));
        }
    }

    public ushort ControlKeyState
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(
                NativeMethods.TV_KeyDownEvent_get_controlKeyState(Ptr, out var controlKeyState)
            );
            return controlKeyState;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(
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
                TerminalFormsException.Check(
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
            TerminalFormsException.Check(
                NativeMethods.TV_KeyDownEvent_set_text(Ptr, textBytes, (byte)textBytes.Length)
            );
        }
    }
}
