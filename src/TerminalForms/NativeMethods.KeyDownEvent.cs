using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_new(out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_equals(
        IntPtr self,
        IntPtr other,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_hash(IntPtr self, out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_get_keyCode(IntPtr self, out ushort @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_set_keyCode(IntPtr self, ushort value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_get_charCode(IntPtr self, out byte @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_set_charCode(IntPtr self, byte value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_get_scanCode(IntPtr self, out byte @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_set_scanCode(IntPtr self, byte value);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_get_controlKeyState(IntPtr self, out ushort @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_set_controlKeyState(IntPtr self, ushort value);

    [LibraryImport(DLL_NAME)]
    public static unsafe partial Error TV_KeyDownEvent_get_text(
        IntPtr self,
        out byte* @out,
        out byte outTextLength
    );

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_KeyDownEvent_set_text(
        IntPtr self,
        Span<byte> value,
        byte textLength
    );
}
