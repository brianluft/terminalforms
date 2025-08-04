using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    public const string DLL_NAME = "tvision4c";

    #region tvision4c.h
    [LibraryImport(DLL_NAME)]
    public static partial int TvHealthCheck();

    [LibraryImport(DLL_NAME)]
    public static partial void TvOverrideMethod(
        NativeType type,
        NativeVirtualMethod virtualMethod,
        IntPtr functionPointer
    );
    #endregion // tvision4c.h

    #region Application.h
    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Application_new();

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Application_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Application_suspend(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Application_suspend_base(IntPtr self);
    #endregion // Application.h

    #region Point.h
    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Point_new0();

    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Point_new1(int x, int y);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Point_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial int Tv_Point_hash(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Point_operator_add_in_place(IntPtr self, IntPtr adder);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Point_operator_subtract_in_place(IntPtr self, IntPtr subber);

    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Point_operator_add(IntPtr one, IntPtr two);

    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Point_operator_subtract(IntPtr one, IntPtr two);

    [LibraryImport(DLL_NAME)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Tv_Point_operator_equals(IntPtr one, IntPtr two);

    [LibraryImport(DLL_NAME)]
    public static partial int Tv_Point_get_x(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Point_set_x(IntPtr self, int x);

    [LibraryImport(DLL_NAME)]
    public static partial int Tv_Point_get_y(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Point_set_y(IntPtr self, int y);
    #endregion // Point.h

    #region Rect.h
    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Rect_new0();

    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Rect_new1(int ax, int ay, int bx, int by);

    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Rect_new2(IntPtr p1, IntPtr p2);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Rect_delete(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial int Tv_Rect_hash(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Rect_move(IntPtr self, int aDX, int aDY);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Rect_grow(IntPtr self, int aDX, int aDY);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Rect_intersect(IntPtr self, IntPtr r);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Rect_Union(IntPtr self, IntPtr r);

    [LibraryImport(DLL_NAME)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Tv_Rect_contains(IntPtr self, IntPtr p);

    [LibraryImport(DLL_NAME)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Tv_Rect_operator_equals(IntPtr self, IntPtr r);

    [LibraryImport(DLL_NAME)]
    [return: MarshalAs(UnmanagedType.I4)]
    public static partial bool Tv_Rect_isEmpty(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Rect_get_a(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial IntPtr Tv_Rect_get_b(IntPtr self);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Rect_set_a(IntPtr self, IntPtr p);

    [LibraryImport(DLL_NAME)]
    public static partial void Tv_Rect_set_b(IntPtr self, IntPtr p);
    #endregion // Rect.h
}
