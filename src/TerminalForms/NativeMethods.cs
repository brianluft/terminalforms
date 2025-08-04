using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    public const string DLL_NAME = "tvision4c";

    #region Application.h
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
    #endregion // Application.h

    #region common.h
    [LibraryImport(DLL_NAME)]
    public static partial int TV_healthCheck();
    #endregion // common.h

    #region Error.h
    [LibraryImport(DLL_NAME)]
    public static partial Error TV_getLastErrorMessageLength(out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_getLastErrorMessage(Span<byte> buffer, int bufferSize);
    #endregion // Error.h

    #region Event.h
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
    #endregion // Event.h

    #region KeyDownEvent.h
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
    #endregion // KeyDownEvent.h

    #region MessageEvent.h
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
    #endregion // MessageEvent.h

    #region MouseEventType.h
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
    #endregion // MouseEventType.h

    #region Point.h
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
    #endregion // Point.h

    #region Rect.h
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
    #endregion // Rect.h

    #region VirtualMethodTable.h
    [LibraryImport(DLL_NAME)]
    public static partial void TV_overrideMethod(
        VirtualMethod virtualMethod,
        IntPtr functionPointer
    );
    #endregion // VirtualMethodTable.h
}
