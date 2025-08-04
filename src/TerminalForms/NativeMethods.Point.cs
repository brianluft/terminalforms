using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_new(out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_equals(
        IntPtr self,
        IntPtr other,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_hash(IntPtr self, out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_operator_add_in_place(IntPtr self, IntPtr adder);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_operator_subtract_in_place(IntPtr self, IntPtr subber);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_operator_add(IntPtr one, IntPtr two, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_operator_subtract(IntPtr one, IntPtr two, out IntPtr @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_get_x(IntPtr self, out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_set_x(IntPtr self, int x);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_get_y(IntPtr self, out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_Point_set_y(IntPtr self, int y);
}
