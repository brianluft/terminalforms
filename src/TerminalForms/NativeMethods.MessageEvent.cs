using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MessageEvent_new(out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MessageEvent_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MessageEvent_equals(
        IntPtr self,
        IntPtr other,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MessageEvent_hash(IntPtr self, out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MessageEvent_get_command(IntPtr self, out ushort @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MessageEvent_set_command(IntPtr self, ushort value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MessageEvent_get_infoPtr(IntPtr self, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MessageEvent_set_infoPtr(IntPtr self, IntPtr value);
}
