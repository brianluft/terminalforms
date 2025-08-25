using System.Runtime.InteropServices;

namespace TurboVision.ScreenCells;

public unsafe partial class TCellChar(void* ptr, bool owned, bool placement)
    : NativeObject<TCellChar>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TCellChar_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TCellChar_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TCellChar_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TCellChar(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TCellChar()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TCellChar_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TCellChar_delete(Ptr));
    }

    protected override bool EqualsCore(TCellChar other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TCellChar_equals(Ptr, other.Ptr, out var result)
        );
        return result;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TCellChar_hash(Ptr, out var hash));
        return hash;
    }

    // Constants
    public static byte FWide
    {
        get
        {
            TurboVisionException.Check(NativeMethods.TV_TCellChar_get_fWide(out var value));
            return value;
        }
    }

    public static byte FTrail
    {
        get
        {
            TurboVisionException.Check(NativeMethods.TV_TCellChar_get_fTrail(out var value));
            return value;
        }
    }

    // Properties
    public byte TextLength
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TCellChar_get__textLength(Ptr, out var textLength)
            );
            return textLength;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TCellChar_set__textLength(Ptr, value));
        }
    }

    public byte Flags
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TCellChar_get__flags(Ptr, out var flags));
            return flags;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TCellChar_set__flags(Ptr, value));
        }
    }

    public int Size
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TCellChar_size(Ptr, out var size));
            return size;
        }
    }

    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            // First get the size
            TurboVisionException.Check(NativeMethods.TV_TCellChar_size(Ptr, out var size));

            if (size <= 0)
                return string.Empty;

            // Allocate buffer and get text
            Span<byte> buffer = stackalloc byte[size];
            fixed (byte* bufferPtr = buffer)
            {
                TurboVisionException.Check(
                    NativeMethods.TV_TCellChar_getText(
                        Ptr,
                        (char*)bufferPtr,
                        size,
                        out var actualLength
                    )
                );
                return Global.UTF8Encoding.GetString(buffer[..actualLength]);
            }
        }
    }

    // Methods
    public void MoveChar(char ch)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_TCellChar_moveChar(Ptr, (byte)ch));
    }

    public void MoveWideCharTrail()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_TCellChar_moveWideCharTrail(Ptr));
    }

    public bool IsWide
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TCellChar_isWide(Ptr, out var result));
            return result;
        }
    }

    public bool IsWideCharTrail
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(
                NativeMethods.TV_TCellChar_isWideCharTrail(Ptr, out var result)
            );
            return result;
        }
    }

    // Array indexer access
    public char this[int index]
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TCellChar_getAt(Ptr, index, out var value));
            return (char)value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TCellChar_setAt(Ptr, index, (byte)value));
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_get_fWide(out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_get_fTrail(out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_get__textLength(void* self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_set__textLength(void* self, byte value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_get__flags(void* self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_set__flags(void* self, byte value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_getText(
            void* self,
            char* buffer,
            int bufferSize,
            out int actualLength
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_size(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_moveChar(void* self, byte ch);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_moveWideCharTrail(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_isWide(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_isWideCharTrail(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_getAt(void* self, int index, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TCellChar_setAt(void* self, int index, byte value);
    }
}
