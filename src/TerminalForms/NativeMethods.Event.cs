using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_new(out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_equals(
        IntPtr self,
        IntPtr other,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_hash(IntPtr self, out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_get_what(IntPtr self, out ushort @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_set_what(IntPtr self, ushort value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_get_mouse(IntPtr self, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_set_mouse(IntPtr self, IntPtr value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_get_keyDown(IntPtr self, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_set_keyDown(IntPtr self, IntPtr value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_get_message(IntPtr self, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_set_message(IntPtr self, IntPtr value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_getMouseEvent(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Event_getKeyEvent(IntPtr self);
}
