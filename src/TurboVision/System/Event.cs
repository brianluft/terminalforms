using System.Runtime.InteropServices;

namespace TurboVision.System;

public partial class Event : NativeObject<Event>
{
    public Event()
        : base(New(), owned: true) { }

    private static IntPtr New()
    {
        TurboVisionException.Check(NativeMethods.TV_Event_new(out var ptr));
        return ptr;
    }

    internal Event(IntPtr ptr, bool owned)
        : base(ptr, owned) { }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_Event_delete(Ptr));
    }

    protected override bool EqualsCore(Event other)
    {
        TurboVisionException.Check(NativeMethods.TV_Event_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_Event_hash(Ptr, out var hash));
        return hash;
    }

    public ushort What
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Event_get_what(Ptr, out var what));
            return what;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Event_set_what(Ptr, value));
        }
    }

    public MouseEventType Mouse
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Event_get_mouse(Ptr, out var mousePtr));
            return new MouseEventType(mousePtr, owned: true);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Event_set_mouse(Ptr, value.Ptr));
        }
    }

    public KeyDownEvent KeyDown
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Event_get_keyDown(Ptr, out var keyDownPtr));
            return new KeyDownEvent(keyDownPtr, owned: true);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Event_set_keyDown(Ptr, value.Ptr));
        }
    }

    public MessageEvent Message
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Event_get_message(Ptr, out var messagePtr));
            return new MessageEvent(messagePtr, owned: true);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_Event_set_message(Ptr, value.Ptr));
        }
    }

    public void GetMouseEvent()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_Event_getMouseEvent(Ptr));
    }

    public void GetKeyEvent()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_Event_getKeyEvent(Ptr));
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_new(out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_delete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_equals(
            IntPtr self,
            IntPtr other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_hash(IntPtr self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_get_what(IntPtr self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_set_what(IntPtr self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_get_mouse(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_set_mouse(IntPtr self, IntPtr value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_get_keyDown(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_set_keyDown(IntPtr self, IntPtr value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_get_message(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_set_message(IntPtr self, IntPtr value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_getMouseEvent(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_getKeyEvent(IntPtr self);
    }
}
