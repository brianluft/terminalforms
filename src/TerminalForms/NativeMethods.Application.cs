using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_new(out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_suspend(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_suspend_base(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_resume(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_resume_base(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_getTileRect(IntPtr self, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_getTileRect_base(IntPtr self, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_handleEvent(IntPtr self, IntPtr @event);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_handleEvent_base(IntPtr self, IntPtr @event);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_writeShellMsg(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Application_writeShellMsg_base(IntPtr self);
}
