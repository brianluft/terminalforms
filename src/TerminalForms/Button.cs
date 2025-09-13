using System.Runtime.CompilerServices;

namespace TerminalForms;

/// <summary>
/// Specifies the alignment of text within a button control.
/// </summary>
public enum ButtonTextAlignment
{
    /// <summary>
    /// Text is centered within the button bounds (default behavior).
    /// </summary>
    Center = 0,

    /// <summary>
    /// Text is left-aligned within the button bounds.
    /// </summary>
    Left = 1,
}

/// <summary>
/// Represents a button control that can be clicked to perform an action.
/// </summary>
public unsafe partial class Button : Control
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfButtonNew,
        NativeMethods.TfButtonDelete,
        NativeMethods.TfButtonEquals,
        NativeMethods.TfButtonHash
    );

    /// <summary>
    /// Initializes a new instance of the <see cref="Button"/> class.
    /// Creates a button control that can respond to user interaction through mouse clicks,
    /// keyboard shortcuts, or programmatic invocation.
    /// </summary>
    public Button()
        : base(_metaObject)
    {
        Check(NativeMethods.TfButtonSetClickEventHandler(Ptr, &NativeClickEventHandler, Ptr));
    }

    /// <summary>
    /// Gets or sets the text displayed on the button.
    /// The text can include a shortcut key indicator using tilde (~) characters around a letter
    /// to create an Alt+letter shortcut (e.g., "~O~K" creates an Alt+O shortcut).
    /// </summary>
    /// <value>The text to display on the button.</value>
    /// <remarks>
    /// Button text is automatically centered within the button bounds unless specific alignment
    /// flags are set. Shortcut keys defined in the text allow users to activate the button
    /// without clicking it directly.
    /// </remarks>
    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonGetText(Ptr, out var text));
            return text;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonSetText(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this button is the default button.
    /// When set to true, this button will respond to the Enter key and be visually
    /// distinguished as the primary action.
    /// </summary>
    /// <value>true if this button is the default button; otherwise, false.</value>
    /// <remarks>
    /// Only one button in a container should be marked as the default button.
    /// Default buttons are typically used for positive actions like "OK" or "Save".
    /// Users can activate the default button by pressing Enter regardless of which
    /// control currently has focus.
    /// </remarks>
    public bool IsDefault
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonGetIsDefault(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonSetIsDefault(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets the alignment of text within the button.
    /// </summary>
    /// <value>A <see cref="ButtonTextAlignment"/> value that specifies how the button text is aligned.</value>
    /// <remarks>
    /// By default, button text is centered within the button bounds. Setting this property
    /// to <see cref="ButtonTextAlignment.Left"/> will left-align the text, which can be
    /// useful for buttons with longer text or when consistent alignment is desired across
    /// multiple buttons of different widths.
    /// </remarks>
    public ButtonTextAlignment TextAlign
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonGetTextAlign(Ptr, out var value));
            return (ButtonTextAlignment)value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonSetTextAlign(Ptr, (int)value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the button should grab focus when clicked.
    /// </summary>
    /// <value>true if the button should grab focus when clicked; otherwise, false.</value>
    /// <remarks>
    /// When this property is true, clicking the button will cause it to receive input focus
    /// in addition to performing its click action. This can be useful for buttons that
    /// represent state changes where the user might want to see visual feedback that the
    /// button is now active. Most buttons should leave this as false to maintain focus
    /// on the previously focused control.
    /// </remarks>
    public bool GrabsFocus
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonGetGrabsFocus(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonSetGrabsFocus(Ptr, value));
        }
    }

    #region Click
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NativeClickEventHandler(void* userData)
    {
        try
        {
            if (!ObjectRegistry.TryGet(userData, out var obj))
                return;

            var button = (Button)obj!;
            button.PerformClick();
        }
        catch { }
    }

    /// <summary>
    /// Occurs when the button is clicked, either by mouse interaction, keyboard shortcut,
    /// or programmatic invocation through <see cref="PerformClick"/>.
    /// </summary>
    /// <remarks>
    /// This event is raised after the button's internal processing is complete and before
    /// any command events are generated. Subscribers to this event can perform custom
    /// logic in response to button activation.
    /// </remarks>
    public event EventHandler? Click;

    /// <summary>
    /// Programmatically triggers the button's click action, simulating user interaction.
    /// This method raises the <see cref="Click"/> event and performs all the same actions
    /// that would occur if the user clicked the button directly.
    /// </summary>
    /// <remarks>
    /// This method is useful for automated testing or for triggering button actions
    /// from other parts of your application. It will generate the same command events
    /// and state changes as a physical button press.
    /// </remarks>
    public void PerformClick()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        OnClick();
    }

    /// <summary>
    /// Raises the <see cref="Click"/> event. This method is called whenever the button
    /// is activated, regardless of whether the activation came from user interaction
    /// or programmatic invocation.
    /// </summary>
    /// <remarks>
    /// When overriding this method in derived classes, be sure to call the base implementation
    /// to ensure that registered event handlers are properly invoked. This method follows
    /// the standard .NET event pattern for controls.
    /// </remarks>
    protected virtual void OnClick()
    {
        Click?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonHash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfButtonSetText(void* self, string text);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfButtonGetText(void* self, out string @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonSetClickEventHandler(
            void* self,
            delegate* unmanaged[Cdecl]<void*, void> function,
            void* userData
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonGetIsDefault(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonSetIsDefault(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonGetTextAlign(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonSetTextAlign(void* self, int value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonGetGrabsFocus(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonSetGrabsFocus(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );
    }
}
