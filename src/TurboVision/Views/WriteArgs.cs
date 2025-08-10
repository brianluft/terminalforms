using System.Runtime.InteropServices;

namespace TurboVision.Views;

public partial class WriteArgs : IDisposable, IEquatable<WriteArgs>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public WriteArgs()
    {
        TurboVisionException.Check(NativeMethods.TV_WriteArgs_new(out var ptr));
        Ptr = ptr;
    }

    public WriteArgs(IntPtr ptr)
    {
        Ptr = ptr;
    }

    ~WriteArgs()
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
            TurboVisionException.Check(NativeMethods.TV_WriteArgs_delete(Ptr));
            IsDisposed = true;
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as WriteArgs);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_WriteArgs_hash(Ptr, out var hash));
        return hash;
    }

    public bool Equals(WriteArgs? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);
        TurboVisionException.Check(NativeMethods.TV_WriteArgs_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    public IntPtr Self
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_WriteArgs_get_self(Ptr, out var self));
            return self;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_WriteArgs_set_self(Ptr, value));
        }
    }

    public IntPtr Target
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_WriteArgs_get_target(Ptr, out var target));
            return target;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_WriteArgs_set_target(Ptr, value));
        }
    }

    public IntPtr Buf
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_WriteArgs_get_buf(Ptr, out var buf));
            return buf;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_WriteArgs_set_buf(Ptr, value));
        }
    }

    public ushort Offset
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_WriteArgs_get_offset(Ptr, out var offset));
            return offset;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_WriteArgs_set_offset(Ptr, value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_new(out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_delete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_equals(
            IntPtr self,
            IntPtr other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_hash(IntPtr self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_get_self(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_set_self(IntPtr self, IntPtr value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_get_target(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_set_target(IntPtr self, IntPtr value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_get_buf(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_set_buf(IntPtr self, IntPtr value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_get_offset(IntPtr self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_WriteArgs_set_offset(IntPtr self, ushort value);
    }
}
