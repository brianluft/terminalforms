using System.Runtime.InteropServices;

namespace TurboVision.System;

public unsafe partial class TEvent(void* ptr, bool owned, bool placement)
    : NativeObject<TEvent>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TEvent_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TEvent_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TEvent_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TEvent(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TEvent()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TEvent_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TEvent_delete(Ptr));
    }

    protected override bool EqualsCore(TEvent other)
    {
        TurboVisionException.Check(NativeMethods.TV_TEvent_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TEvent_hash(Ptr, out var hash));
        return hash;
    }

    public ushort What
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TEvent_get_what(Ptr, out var what));
            return what;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TurboVisionException.Check(NativeMethods.TV_TEvent_set_what(Ptr, value));
        }
    }

    public void GetMouse(MouseEventType dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_TEvent_get_mouse(Ptr, dst.Ptr));
    }

    public void SetMouse(MouseEventType src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_TEvent_set_mouse(Ptr, src.Ptr));
    }

    public void GetKeyDown(KeyDownEvent dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_TEvent_get_keyDown(Ptr, dst.Ptr));
    }

    public void SetKeyDown(KeyDownEvent src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_TEvent_set_keyDown(Ptr, src.Ptr));
    }

    public void GetMessage(MessageEvent dst)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(dst.IsDisposed, dst);
        TurboVisionException.Check(NativeMethods.TV_TEvent_get_message(Ptr, dst.Ptr));
    }

    public void SetMessage(MessageEvent src)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(src.IsDisposed, src);
        TurboVisionException.Check(NativeMethods.TV_TEvent_set_message(Ptr, src.Ptr));
    }

    public void GetMouseEvent()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_TEvent_getMouseEvent(Ptr));
    }

    public void GetKeyEvent()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_TEvent_getKeyEvent(Ptr));
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_get_what(void* self, out ushort @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_set_what(void* self, ushort value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_get_mouse(void* self, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_set_mouse(void* self, void* src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_get_keyDown(void* self, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_set_keyDown(void* self, void* src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_get_message(void* self, void* dst);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_set_message(void* self, void* src);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_getMouseEvent(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TEvent_getKeyEvent(void* self);
    }
}
