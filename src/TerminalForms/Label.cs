namespace TerminalForms;

/// <summary>
/// Represents a label control that displays static text and can provide hotkey functionality for navigation.
/// </summary>
/// <remarks>
/// The Label control provides both static text display and hotkey functionality following the Windows Forms model.
/// Unlike traditional Turbo Vision labels which are explicitly linked to specific controls, TerminalForms labels
/// use hotkey navigation that moves focus to the next control in tab order, providing more flexible and intuitive
/// keyboard navigation patterns.
///
/// When <see cref="UseMnemonic"/> is enabled, labels can define hotkey shortcuts by surrounding a letter with
/// tilde (~) characters in the text (e.g., "~F~ile" creates an Alt+F shortcut). Activating the hotkey will
/// move focus to the next focusable control in the container's tab order.
/// </remarks>
public unsafe partial class Label : Control
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfLabelNew,
        NativeMethods.TfLabelDelete,
        NativeMethods.TfLabelEquals,
        NativeMethods.TfLabelHash
    );

    /// <summary>
    /// Initializes a new instance of the <see cref="Label"/> class.
    /// Creates a label control that can display static text and optionally provide hotkey navigation functionality.
    /// </summary>
    public Label()
        : base(_metaObject) { }

    /// <summary>
    /// Gets or sets the text displayed by the label.
    /// The text can include a hotkey indicator using tilde (~) characters around a letter
    /// to create an Alt+letter shortcut (e.g., "~O~pen" creates an Alt+O shortcut).
    /// </summary>
    /// <value>The text to display in the label.</value>
    /// <remarks>
    /// When <see cref="UseMnemonic"/> is enabled, hotkey indicators in the text will be processed
    /// to create keyboard shortcuts. The indicated letter will be underlined in the display and
    /// the Alt+letter combination will activate the hotkey, causing focus to move to the next
    /// focusable control in the tab order.
    ///
    /// If <see cref="UseMnemonic"/> is disabled, tilde characters will be displayed literally
    /// without creating hotkey functionality.
    /// </remarks>
    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfLabelGetText(Ptr, out var text));
            return text;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfLabelSetText(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the label processes mnemonic characters for hotkey functionality.
    /// </summary>
    /// <value>true if the label should process mnemonic characters; otherwise, false. The default is true.</value>
    /// <remarks>
    /// When this property is true, the label will process tilde (~) characters in the <see cref="Text"/> property
    /// to create hotkey functionality. The character surrounded by tildes will be underlined and can be activated
    /// with Alt+character to move focus to the next focusable control.
    ///
    /// When this property is false, the label will display text literally without processing mnemonic characters,
    /// and no hotkey functionality will be available. This is useful for displaying text that contains tilde
    /// characters that should be shown rather than processed as mnemonics.
    /// </remarks>
    public bool UseMnemonic
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfLabelGetUseMnemonic(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfLabelSetUseMnemonic(Ptr, value));
        }
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfLabelNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfLabelDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfLabelEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfLabelHash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfLabelSetText(void* self, string text);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfLabelGetText(void* self, out string @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfLabelGetUseMnemonic(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfLabelSetUseMnemonic(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );
    }
}
