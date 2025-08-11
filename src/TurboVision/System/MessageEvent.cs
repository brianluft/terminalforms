using System.Runtime.InteropServices;

namespace TurboVision.System;

public partial class MessageEvent(IntPtr ptr, bool owned, bool placement) : NativeObject<MessageEvent>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(
                NativeMethods.TV_MessageEvent_placementSize,
                NativeMethods.TV_MessageEvent_placementNew,
                NativeMethods.TV_MessageEvent_new
            )
        {
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public MessageEvent(IntPtr placement) : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public MessageEvent() : this(Factory.Instance.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(IntPtr ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_MessageEvent_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_MessageEvent_delete(Ptr));
    }

    protected override bool EqualsCore(MessageEvent other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_MessageEvent_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_MessageEvent_hash(Ptr, out var hash));
        return hash;
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
        public static partial Error TV_MessageEvent_placementSize(out int outSize, out int outAlignment);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_placementNew(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_MessageEvent_placementDelete(IntPtr self);

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
