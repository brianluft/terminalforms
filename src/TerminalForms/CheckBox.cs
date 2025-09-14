using System.Runtime.CompilerServices;

namespace TerminalForms;

/// <summary>
/// Represents a check box control that can be toggled between checked and unchecked states.
/// Check boxes allow users to make binary choices and are commonly used in forms for
/// enabling or disabling options, selecting features, or indicating preferences.
/// </summary>
/// <remarks>
/// Check boxes maintain their state independently of other controls, unlike radio buttons
/// which form mutually exclusive groups. Users can interact with check boxes through mouse
/// clicks, keyboard navigation (Tab to focus, Space to toggle), or programmatic control.
/// The control provides events for responding to state changes and supports customization
/// of text labels and enabled state.
/// </remarks>
public unsafe partial class CheckBox : Control
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfCheckBoxNew,
        NativeMethods.TfCheckBoxDelete,
        NativeMethods.TfCheckBoxEquals,
        NativeMethods.TfCheckBoxHash
    );

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckBox"/> class.
    /// Creates a check box control that can respond to user interaction through mouse clicks,
    /// keyboard navigation, or programmatic invocation.
    /// </summary>
    /// <remarks>
    /// The check box is initially unchecked and enabled. Use the <see cref="Checked"/> property
    /// to set the initial state, and <see cref="Text"/> to set the display label.
    /// Event handlers can be attached to <see cref="CheckedChanged"/> to respond to state changes.
    /// </remarks>
    public CheckBox()
        : base(_metaObject)
    {
        Check(
            NativeMethods.TfCheckBoxSetStateChangedEventHandler(
                Ptr,
                &NativeStateChangedEventHandler,
                Ptr
            )
        );
    }

    /// <summary>
    /// Gets or sets the text displayed as the check box label.
    /// The text appears next to the check box indicator and can include shortcut key
    /// indicators using tilde (~) characters around a letter to create Alt+letter shortcuts.
    /// </summary>
    /// <value>The text to display next to the check box.</value>
    /// <remarks>
    /// The label text helps users understand what option or setting the check box controls.
    /// Shortcut keys defined in the text (e.g., "~E~nable feature") allow users to toggle
    /// the check box state using keyboard combinations without focusing the control first.
    /// Clear, descriptive text improves accessibility and user experience.
    /// </remarks>
    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfCheckBoxGetText(Ptr, out var text));
            return text;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfCheckBoxSetText(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the check box is in the checked state.
    /// When true, the check box displays a checkmark or X indicator; when false,
    /// the check box appears empty.
    /// </summary>
    /// <value>true if the check box is checked; otherwise, false.</value>
    /// <remarks>
    /// This property represents the primary state of the check box control. Changes to this
    /// property trigger the <see cref="CheckedChanged"/> event, allowing other parts of your
    /// application to respond to state changes. The visual representation typically shows
    /// an X or checkmark when checked, and an empty box when unchecked.
    /// </remarks>
    public bool Checked
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfCheckBoxGetChecked(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfCheckBoxSetChecked(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the check box is enabled and can respond to user interaction.
    /// When false, the check box appears dimmed and does not respond to mouse clicks or keyboard input.
    /// </summary>
    /// <value>true if the check box is enabled; otherwise, false.</value>
    /// <remarks>
    /// Disabled check boxes are useful when certain options should not be available based on
    /// application state or user permissions. The control maintains its checked state when
    /// disabled but will not respond to user interaction until re-enabled. This provides
    /// visual feedback about feature availability without losing the current selection.
    /// </remarks>
    public bool Enabled
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfCheckBoxGetEnabled(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfCheckBoxSetEnabled(Ptr, value));
        }
    }

    #region CheckedChanged
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NativeStateChangedEventHandler(void* userData)
    {
        try
        {
            if (!ObjectRegistry.TryGet(userData, out var obj))
                return;

            var checkBox = (CheckBox)obj!;
            ObjectDisposedException.ThrowIf(checkBox.IsDisposed, checkBox);
            checkBox.OnCheckedChanged();
        }
        catch { }
    }

    /// <summary>
    /// Occurs when the value of the <see cref="Checked"/> property changes, either through
    /// user interaction or programmatic changes.
    /// </summary>
    /// <remarks>
    /// This event is raised after the check box state has been updated, providing an
    /// opportunity for your application to respond to the change. Common uses include
    /// updating related controls, saving preferences, or triggering other application logic
    /// based on the new checked state.
    /// </remarks>
    public event EventHandler? CheckedChanged;

    /// <summary>
    /// Raises the <see cref="CheckedChanged"/> event. This method is called whenever the
    /// check box state changes, regardless of whether the change came from user interaction
    /// or programmatic modification.
    /// </summary>
    /// <remarks>
    /// When overriding this method in derived classes, be sure to call the base implementation
    /// to ensure that registered event handlers are properly invoked. This method follows
    /// the standard .NET event pattern for controls and provides a point for customization
    /// in derived check box implementations.
    /// </remarks>
    protected virtual void OnCheckedChanged()
    {
        CheckedChanged?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfCheckBoxNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfCheckBoxDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfCheckBoxEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfCheckBoxHash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfCheckBoxSetText(void* self, string text);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfCheckBoxGetText(void* self, out string @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfCheckBoxSetChecked(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfCheckBoxGetChecked(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfCheckBoxSetEnabled(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfCheckBoxGetEnabled(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfCheckBoxSetStateChangedEventHandler(
            void* self,
            delegate* unmanaged[Cdecl]<void*, void> function,
            void* userData
        );
    }
}
