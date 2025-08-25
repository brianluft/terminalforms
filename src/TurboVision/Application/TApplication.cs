using System.Runtime.InteropServices;

namespace TurboVision.Application;

public unsafe partial class TApplication(void* ptr, bool owned, bool placement)
    : NativeObject<TApplication>(ptr, owned, placement)
{
    private sealed class Factory : NativeObjectFactory<Factory>
    {
        public Factory()
            : base(NativeMethods.TV_TApplication_placementSize) { }

        public unsafe void* PlacementNew(byte* ptr)
        {
            ptr = Align(ptr);
            TurboVisionException.Check(NativeMethods.TV_TApplication_placementNew(ptr));
            return ptr;
        }

        public static unsafe void* New()
        {
            TurboVisionException.Check(NativeMethods.TV_TApplication_new(out var ptr));
            return ptr;
        }
    }

    public static int PlacementSize => Factory.Instance.PlacementSize;

    public TApplication(byte* placement)
        : this(Factory.Instance.PlacementNew(placement), owned: true, placement: true) { }

    public TApplication()
        : this(Factory.New(), owned: true, placement: false) { }

    protected override void PlacementDeleteCore(void* ptr)
    {
        TurboVisionException.Check(NativeMethods.TV_TApplication_placementDelete(ptr));
    }

    protected override void DeleteCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TApplication_delete(Ptr));
    }

    protected override bool EqualsCore(TApplication other)
    {
        TurboVisionException.Check(
            NativeMethods.TV_TApplication_equals(Ptr, other.Ptr, out var equals)
        );
        return equals;
    }

    protected override int GetHashCodeCore()
    {
        TurboVisionException.Check(NativeMethods.TV_TApplication_hash(Ptr, out var hash));
        return hash;
    }

    public void Suspend()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_TApplication_suspend(Ptr));
    }

    public virtual void SuspendCore()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TurboVisionException.Check(NativeMethods.TV_TApplication_suspend_base(Ptr));
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial int TV_healthCheck();

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_placementSize(
            out int outSize,
            out int outAlignment
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_placementNew(byte* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_placementDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_new(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_delete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_equals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_hash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_suspend(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_suspend_base(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_resume(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_resume_base(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_getTileRect(void* self, out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_getTileRect_base(void* self, out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_handleEvent(void* self, void* @event);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_handleEvent_base(void* self, void* @event);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_writeShellMsg(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TApplication_writeShellMsg_base(void* self);
    }

    static TApplication()
    {
        // Test the P/Invoke connection.
        int result;
        try
        {
            result = NativeMethods.TV_healthCheck();
        }
        catch (Exception ex)
        {
            throw new TurboVisionException(
                Error.Error_NativeInteropFailure,
                Error.Error_NativeInteropFailure.GetDefaultMessage() + "\n\n" + ex.Message
            );
        }

        if (result != 123)
        {
            throw new TurboVisionException(Error.Error_NativeInteropFailure);
        }
    }
}
