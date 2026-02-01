using System.Runtime.CompilerServices;

namespace TerminalForms;

/// <summary>
/// Represents a window form in the Terminal Forms application. Forms are specialized container controls
/// providing bordered rectangular displays that can contain other controls. Forms can be moved, resized, and managed
/// as part of the application's desktop.
/// </summary>
/// <remarks>
/// Forms inherit all the functionality of container controls, allowing them to host child controls.
/// They also provide window-specific features like frames, titles, and desktop integration.
/// Use <see cref="Show"/> to make the form visible on the desktop.
/// </remarks>
public unsafe partial class Form : ContainerControl
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfFormNew,
        NativeMethods.TfFormDelete,
        NativeMethods.TfFormEquals,
        NativeMethods.TfFormHash
    );

    /// <summary>
    /// Initializes a new instance of the <see cref="Form"/> class.
    /// Creates a new form that can contain child controls and be displayed on the application desktop.
    /// </summary>
    public Form()
        : base(_metaObject)
    {
        Check(NativeMethods.TfFormSetClosedEventHandler(Ptr, &NativeClosedEventHandler, Ptr));
    }

    /// <summary>
    /// Gets or sets the text displayed in the form's title bar.
    /// The title provides context to users about the form's purpose or content.
    /// </summary>
    /// <value>The text to display in the form's title bar.</value>
    /// <remarks>
    /// The title is displayed in the form's frame and helps users identify the form's purpose.
    /// Setting this property updates the visual display immediately. An empty string will
    /// display no title text, though the title bar area remains visible.
    /// </remarks>
    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormGetText(Ptr, out var text));
            return text;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormSetText(Ptr, value ?? ""));
        }
    }

    /// <summary>
    /// Gets or sets the position and size of the form in character cell coordinates.
    /// This property controls where the form appears on the desktop and its dimensions.
    /// </summary>
    /// <value>A <see cref="Rectangle"/> that represents the bounds of the form.</value>
    /// <remarks>
    /// The bounds specify the form's location (X, Y) and size (Width, Height) in character cells.
    /// Changes to this property take effect immediately, moving and/or resizing the form as needed.
    /// The coordinate system starts at (0,0) in the upper-left corner of the desktop.
    /// </remarks>
    public override Rectangle Bounds
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormGetBounds(Ptr, out var bounds));
            return bounds;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormSetBounds(Ptr, &value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the form displays a close button in its title bar.
    /// When enabled, users can close the form by clicking the close button or using keyboard shortcuts.
    /// </summary>
    /// <value>true if the form should display a close button; otherwise, false.</value>
    /// <remarks>
    /// The close button provides a standard way for users to dismiss the form. When disabled,
    /// the form can still be closed programmatically using the <see cref="Close"/> method.
    /// This property affects the visual appearance of the title bar and the available user interactions.
    /// </remarks>
    public bool ControlBox
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormGetControlBox(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormSetControlBox(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the form displays a maximize/zoom button in its title bar.
    /// When enabled, users can maximize the form to fill the available desktop space.
    /// </summary>
    /// <value>true if the form should display a maximize button; otherwise, false.</value>
    /// <remarks>
    /// The maximize button allows users to quickly resize the form to its maximum size and restore
    /// it to its previous size. The button typically appears as a zoom icon in the title bar.
    /// This feature is useful for forms that benefit from larger display areas.
    /// </remarks>
    public bool MaximizeBox
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormGetMaximizeBox(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormSetMaximizeBox(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the form can be resized by the user.
    /// When enabled, users can drag the form's edges or corners to change its size.
    /// </summary>
    /// <value>true if the form can be resized; otherwise, false.</value>
    /// <remarks>
    /// Resizable forms display resize handles and allow users to interactively adjust the form's
    /// dimensions. Non-resizable forms maintain a fixed size but can still be moved (if moving is enabled).
    /// This property is independent of the maximize functionality.
    /// </remarks>
    public bool Resizable
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormGetResizable(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFormSetResizable(Ptr, value));
        }
    }

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
    /// The form is also added to <see cref="Application.OpenForms"/> to prevent garbage collection
    /// while it is visible.
    /// </remarks>
    public void Show()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFormShow(Ptr));

        // TProgram::deskTop takes ownership.
        IsOwned = false;

        // Keep a strong reference to prevent garbage collection while the form is open.
        Application.RegisterOpenForm(this);
    }

    /// <summary>
    /// Closes the form and removes it from the desktop. Once closed, the form is no longer
    /// visible or interactive, and its resources are cleaned up.
    /// </summary>
    /// <remarks>
    /// This method programmatically closes the form, performing the same action as if the user
    /// clicked the close button (when <see cref="ControlBox"/> is enabled). After calling this method,
    /// the form should not be used further as it may be disposed by the desktop management system.
    /// The <see cref="Closed"/> event is raised after the form is removed from the desktop.
    /// </remarks>
    public void Close()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFormClose(Ptr));
    }

    #region Closed Event
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NativeClosedEventHandler(void* userData)
    {
        try
        {
            if (!ObjectRegistry.TryGet(userData, out var obj))
                return;

            var form = (Form)obj!;

            // Skip if form is already disposed (defensive check)
            if (form.IsDisposed)
                return;

            // Remove from OpenForms to allow garbage collection.
            Application.UnregisterOpenForm(form);

            form.OnClosed();
        }
        catch { }
    }

    /// <summary>
    /// Occurs when the form has been closed, either by user action (clicking the close button)
    /// or by calling the <see cref="Close"/> method programmatically.
    /// </summary>
    /// <remarks>
    /// This event is raised after the form has been removed from the desktop and is no longer visible.
    /// Use this event to perform cleanup operations or to respond to the form being closed.
    /// After this event fires, the form is removed from <see cref="Application.OpenForms"/> and
    /// may be garbage collected if no other references exist.
    /// </remarks>
    public event EventHandler? Closed;

    /// <summary>
    /// Raises the <see cref="Closed"/> event. This method is called when the form is closed,
    /// regardless of whether the close was initiated by user interaction or programmatic invocation.
    /// </summary>
    /// <remarks>
    /// When overriding this method in derived classes, be sure to call the base implementation
    /// to ensure that registered event handlers are properly invoked. This method follows
    /// the standard .NET event pattern for controls.
    /// </remarks>
    protected virtual void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
    #endregion

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

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfFormSetText(void* self, string text);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfFormGetText(void* self, out string @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormSetBounds(void* self, Rectangle* bounds);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormGetBounds(void* self, out Rectangle @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormSetControlBox(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormGetControlBox(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormSetMaximizeBox(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormGetMaximizeBox(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormSetResizable(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormGetResizable(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormClose(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormSetClosedEventHandler(
            void* self,
            delegate* unmanaged[Cdecl]<void*, void> function,
            void* userData
        );
    }
}
