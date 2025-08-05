using System.Runtime.InteropServices;

namespace TerminalForms;

public sealed class MessageEvent : IDisposable, IEquatable<MessageEvent>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public MessageEvent()
    {
        TerminalFormsException.Check(NativeMethods.TV_MessageEvent_new(out var ptr));
        Ptr = ptr;
    }

    public MessageEvent(IntPtr ptr)
    {
        Ptr = ptr;
    }

    ~MessageEvent()
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
            TerminalFormsException.Check(NativeMethods.TV_MessageEvent_delete(Ptr));
            IsDisposed = true;
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as MessageEvent);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        TerminalFormsException.Check(NativeMethods.TV_MessageEvent_hash(Ptr, out var hash));
        return hash;
    }

    public bool Equals(MessageEvent? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);

        TerminalFormsException.Check(
            NativeMethods.TV_MessageEvent_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    public ushort Command
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(
                NativeMethods.TV_MessageEvent_get_command(Ptr, out var command)
            );
            return command;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_MessageEvent_set_command(Ptr, value));
        }
    }

    public IntPtr InfoPtr
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(
                NativeMethods.TV_MessageEvent_get_infoPtr(Ptr, out var infoPtr)
            );
            return infoPtr;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_MessageEvent_set_infoPtr(Ptr, value));
        }
    }
}
