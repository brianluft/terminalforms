using System.Runtime.InteropServices;

namespace TerminalForms;

public sealed class Event : IDisposable, IEquatable<Event>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public Event()
    {
        TerminalFormsException.Check(NativeMethods.TV_Event_new(out var ptr));
        Ptr = ptr;
    }

    public Event(IntPtr ptr)
    {
        Ptr = ptr;
    }

    ~Event()
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
            TerminalFormsException.Check(NativeMethods.TV_Event_delete(Ptr));
            IsDisposed = true;
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as Event);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        TerminalFormsException.Check(NativeMethods.TV_Event_hash(Ptr, out var hash));
        return hash;
    }

    public bool Equals(Event? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);

        TerminalFormsException.Check(NativeMethods.TV_Event_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    public ushort What
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_Event_get_what(Ptr, out var what));
            return what;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_Event_set_what(Ptr, value));
        }
    }

    public MouseEventType Mouse
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_Event_get_mouse(Ptr, out var mousePtr));
            return new MouseEventType(mousePtr);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_Event_set_mouse(Ptr, value.Ptr));
        }
    }

    public KeyDownEvent KeyDown
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(
                NativeMethods.TV_Event_get_keyDown(Ptr, out var keyDownPtr)
            );
            return new KeyDownEvent(keyDownPtr);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_Event_set_keyDown(Ptr, value.Ptr));
        }
    }

    public MessageEvent Message
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(
                NativeMethods.TV_Event_get_message(Ptr, out var messagePtr)
            );
            return new MessageEvent(messagePtr);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_Event_set_message(Ptr, value.Ptr));
        }
    }

    public void GetMouseEvent()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TerminalFormsException.Check(NativeMethods.TV_Event_getMouseEvent(Ptr));
    }

    public void GetKeyEvent()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TerminalFormsException.Check(NativeMethods.TV_Event_getKeyEvent(Ptr));
    }
}
