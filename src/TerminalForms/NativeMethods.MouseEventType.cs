using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_new(out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_equals(
        IntPtr self,
        IntPtr other,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_hash(IntPtr self, out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_get_where(IntPtr self, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_set_where(IntPtr self, IntPtr value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_get_eventFlags(IntPtr self, out ushort @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_set_eventFlags(IntPtr self, ushort value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_get_controlKeyState(IntPtr self, out ushort @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_set_controlKeyState(IntPtr self, ushort value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_get_buttons(IntPtr self, out byte @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_set_buttons(IntPtr self, byte value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_get_wheel(IntPtr self, out byte @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_MouseEventType_set_wheel(IntPtr self, byte value);
}
