namespace TerminalForms;

/// <summary>
/// Represents a window form in the Terminal Forms application. Forms are specialized container controls
/// that wrap Turbo Vision's TWindow functionality, providing bordered rectangular displays that can
/// contain other controls. Forms can be moved, resized, and managed as part of the application's desktop.
/// </summary>
/// <remarks>
/// Forms inherit all the functionality of container controls, allowing them to host child controls.
/// They also provide window-specific features like frames, titles, and desktop integration.
/// Use <see cref="Show"/> to make the form visible on the desktop.
/// </remarks>
public unsafe partial class Form() : ContainerControl(_metaObject)
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfFormNew,
        NativeMethods.TfFormDelete,
        NativeMethods.TfFormEquals,
        NativeMethods.TfFormHash
    );

    /// <summary>
    /// Displays the form on the application's desktop. Once shown, the form becomes part of the
    /// desktop's view hierarchy and can be interacted with by the user. The desktop takes ownership
    /// of the form, managing its lifetime and z-order relative to other windows.
    /// </summary>
    /// <remarks>
    /// After calling this method, the form is added to the desktop's collection of windows and
    /// becomes visible to the user. The desktop handles window management tasks such as activation,
    /// deactivation, and cleanup. The form's ownership is transferred to the desktop, meaning
    /// the desktop will handle disposing of the form when appropriate.
    /// </remarks>
    public void Show()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFormShow(Ptr));

        // TProgram::deskTop takes ownership.
        IsOwned = false;
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormHash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormShow(void* self);
    }
}
