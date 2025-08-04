using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_new(out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_new2(int ax, int ay, int bx, int by, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_new3(IntPtr p1, IntPtr p2, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_equals(
        IntPtr self,
        IntPtr other,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_hash(IntPtr self, out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_move(IntPtr self, int aDX, int aDY);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_grow(IntPtr self, int aDX, int aDY);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_intersect(IntPtr self, IntPtr r);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_Union(IntPtr self, IntPtr r);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_contains(
        IntPtr self,
        IntPtr p,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_isEmpty(
        IntPtr self,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_get_a(IntPtr self, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_get_b(IntPtr self, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_set_a(IntPtr self, IntPtr p);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Rect_set_b(IntPtr self, IntPtr p);
}
