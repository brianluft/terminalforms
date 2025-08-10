using System.Runtime.InteropServices;

namespace TurboVision.Application;

public partial class Application
{
    static Application()
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

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial int TV_healthCheck();

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_new(out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_delete(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_suspend(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_suspend_base(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_resume(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_resume_base(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_getTileRect(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_getTileRect_base(IntPtr self, out IntPtr @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_handleEvent(IntPtr self, IntPtr @event);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_handleEvent_base(IntPtr self, IntPtr @event);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_writeShellMsg(IntPtr self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_Application_writeShellMsg_base(IntPtr self);
    }
}
