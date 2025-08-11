using System.Runtime.InteropServices;

namespace TurboVision.System;

public partial class Event(IntPtr ptr, bool owned, bool placement) : NativeObject<Event>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_Event_placementSize,
                NativeMethods.TV_Event_placementNew,
                NativeMethods.TV_Event_new
            )
        {
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public Event(IntPtr placement) : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public Event() : this(Factory.Instance.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(IntPtr ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_Event_placementDelete(ptr));
    }

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

    public void GetMouse(MouseEventType dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_Event_get_mouse(Ptr, dst.Ptr));
    }

    public void SetMouse(MouseEventType src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_Event_set_mouse(Ptr, src.Ptr));
    }

    public void GetKeyDown(KeyDownEvent dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_Event_get_keyDown(Ptr, dst.Ptr));
    }

    public void SetKeyDown(KeyDownEvent src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_Event_set_keyDown(Ptr, src.Ptr));
    }

    public void GetMessage(MessageEvent dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_Event_get_message(Ptr, dst.Ptr));
    }

    public void SetMessage(MessageEvent src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_Event_set_message(Ptr, src.Ptr));
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
        public static partial Error TV_Event_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_placementNew(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_placementDelete(IntPtr self);

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
        public static partial Error TV_Event_get_mouse(IntPtr self, IntPtr dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_set_mouse(IntPtr self, IntPtr src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_get_keyDown(IntPtr self, IntPtr dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_set_keyDown(IntPtr self, IntPtr src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_get_message(IntPtr self, IntPtr dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_set_message(IntPtr self, IntPtr src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_getMouseEvent(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Event_getKeyEvent(IntPtr self);
    }
}
