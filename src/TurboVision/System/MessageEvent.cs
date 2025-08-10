using System.Runtime.InteropServices;

namespace TurboVision.System;

public partial class MessageEvent : IDisposable, IEquatable<MessageEvent>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public MessageEvent()
    {
        TurboVisionException.Check(NativeMethods.TV_MessageEvent_new(out var ptr));
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
            TurboVisionException.Check(NativeMethods.TV_MessageEvent_delete(Ptr));
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

        TurboVisionException.Check(NativeMethods.TV_MessageEvent_hash(Ptr, out var hash));
        return hash;
    }

    public bool Equals(MessageEvent? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);

        TurboVisionException.Check(
            NativeMethods.TV_MessageEvent_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    public ushort Command
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_MessageEvent_get_command(Ptr, out var command)
            );
            return command;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_MessageEvent_set_command(Ptr, value));
        }
    }

    public IntPtr InfoPtr
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_MessageEvent_get_infoPtr(Ptr, out var infoPtr)
            );
            return infoPtr;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_MessageEvent_set_infoPtr(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_new(out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_delete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_equals(
            IntPtr self,
            IntPtr other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_hash(IntPtr self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_get_command(IntPtr self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_set_command(IntPtr self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_get_infoPtr(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_set_infoPtr(IntPtr self, IntPtr value);
    }
}
