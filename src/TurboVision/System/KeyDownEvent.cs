using System.Runtime.InteropServices;
using System.Text;

namespace TurboVision.System;

public unsafe partial class KeyDownEvent(void* ptr, bool owned, bool placement)
    : NativeObject<KeyDownEvent>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_KeyDownEvent_placementSize,
                NativeMethods.TV_KeyDownEvent_placementNew,
                NativeMethods.TV_KeyDownEvent_new
            ) { }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public KeyDownEvent(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public KeyDownEvent()
        : this(Factory.Instance.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_KeyDownEvent_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_KeyDownEvent_delete(Ptr));
    }

    protected override bool EqualsCore(KeyDownEvent other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_KeyDownEvent_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_KeyDownEvent_hash(Ptr, out var hash));
        return hash;
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
        public static partial Error TV_KeyDownEvent_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_get_keyCode(void* self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_keyCode(void* self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_get_charCode(void* self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_charCode(void* self, byte value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_get_scanCode(void* self, out byte @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_scanCode(void* self, byte value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_get_controlKeyState(
            void* self,
            out ushort @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_controlKeyState(void* self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static unsafe partial Error TV_KeyDownEvent_get_text(
            void* self,
            out byte* @out,
            out byte outTextLength
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_KeyDownEvent_set_text(
            void* self,
            Span<byte> value,
            byte textLength
        );
    }
}
