using System.Runtime.CompilerServices;

namespace TerminalForms;

/// <summary>
/// Represents a single-line text input control that allows users to enter and edit text.
/// </summary>
/// <remarks>
/// TextBox provides a standard single-line text entry field with support for text selection,
/// clipboard operations (cut, copy, paste), keyboard navigation, and maximum length constraints.
/// The control raises events when the text content changes, allowing applications to validate
/// input or respond to user modifications in real-time.
/// </remarks>
public unsafe partial class TextBox : Control
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfTextBoxNew,
        NativeMethods.TfTextBoxDelete,
        NativeMethods.TfTextBoxEquals,
        NativeMethods.TfTextBoxHash
    );

    /// <summary>
    /// Initializes a new instance of the <see cref="TextBox"/> class.
    /// Creates a text input control with a default maximum length of 256 characters.
    /// </summary>
    /// <remarks>
    /// The text box is initially empty and enabled. Users can type text, use arrow keys
    /// for navigation, select text with Shift+arrows or mouse dragging, and perform
    /// clipboard operations with standard keyboard shortcuts (Ctrl+X, Ctrl+C, Ctrl+V).
    /// </remarks>
    public TextBox()
        : base(_metaObject)
    {
        Check(
            NativeMethods.TfTextBoxSetTextChangedEventHandler(
                Ptr,
                &NativeTextChangedEventHandler,
                Ptr
            )
        );
    }

    #region Text Property

    /// <summary>
    /// Gets or sets the current text in the TextBox.
    /// </summary>
    /// <value>The text contained in the control. The default is an empty string ("").</value>
    /// <remarks>
    /// Setting this property replaces all current text and resets the selection to empty.
    /// The cursor is positioned at the end of the new text. If the text exceeds <see cref="MaxLength"/>,
    /// it will be truncated to fit. Setting Text triggers the <see cref="TextChanged"/> event if the
    /// value differs from the current text.
    /// </remarks>
    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfTextBoxGetText(Ptr, out var text));
            return text;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfTextBoxSetText(Ptr, value ?? string.Empty));
        }
    }

    #endregion

    #region MaxLength Property

    /// <summary>
    /// Gets the maximum number of characters that can be entered into the text box.
    /// </summary>
    /// <value>
    /// The maximum number of characters that can be entered in the control. The default is 256.
    /// This value is set when the control is created and cannot be changed afterward.
    /// </value>
    /// <remarks>
    /// The MaxLength property enforces a hard limit on text entry. When the limit is reached,
    /// the control will not accept additional characters from keyboard input or paste operations.
    /// Attempting to set the <see cref="Text"/> property with a string longer than MaxLength will
    /// result in the string being truncated.
    /// </remarks>
    public int MaxLength
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfTextBoxGetMaxLength(Ptr, out var value));
            return value;
        }
    }

    #endregion

    #region Selection Properties

    /// <summary>
    /// Gets or sets the starting point of text selected in the text box.
    /// </summary>
    /// <value>
    /// The starting position of text selected in the text box, measured in characters from the beginning.
    /// Valid values range from 0 to the length of the text. Out-of-range values are automatically clamped.
    /// </value>
    /// <remarks>
    /// Use this property in conjunction with <see cref="SelectionLength"/> to programmatically control
    /// text selection. Setting SelectionStart maintains the current SelectionLength if possible.
    /// If the selection would exceed the text bounds, the length is automatically adjusted.
    /// </remarks>
    public int SelectionStart
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfTextBoxGetSelectionStart(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfTextBoxSetSelectionStart(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets the number of characters selected in the text box.
    /// </summary>
    /// <value>
    /// The number of characters selected in the text box. Valid values range from 0 to the length
    /// of the text minus <see cref="SelectionStart"/>. Negative values are treated as 0.
    /// Out-of-range values are automatically clamped.
    /// </value>
    /// <remarks>
    /// A value of 0 indicates no selection (insertion point only). Setting this property maintains
    /// the current <see cref="SelectionStart"/> and extends or shrinks the selection from that point.
    /// </remarks>
    public int SelectionLength
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfTextBoxGetSelectionLength(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfTextBoxSetSelectionLength(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets the selected text in the control.
    /// </summary>
    /// <value>
    /// A string containing the currently selected text. If no text is selected, returns an empty string.
    /// </value>
    /// <remarks>
    /// Getting this property returns a copy of the selected text. Setting this property replaces
    /// the currently selected text with the new value. If no text is selected, the new text is
    /// inserted at the current cursor position.
    /// Setting SelectedText is subject to the <see cref="MaxLength"/> constraint. If inserting
    /// the text would exceed the maximum length, it will be truncated.
    /// Setting this property triggers the <see cref="TextChanged"/> event if the text is modified.
    /// </remarks>
    public string SelectedText
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);

            // Get selection length first to size the buffer
            Check(NativeMethods.TfTextBoxGetSelectionLength(Ptr, out var length));
            if (length == 0)
                return string.Empty;

            // Allocate buffer and retrieve selected text (+1 for null terminator)
            var bufferSize = length + 1;
            var buffer = stackalloc byte[bufferSize];
            Check(NativeMethods.TfTextBoxGetSelectedText(Ptr, buffer, bufferSize));

            // Find actual length (null-terminated)
            var actualLength = 0;
            while (actualLength < length && buffer[actualLength] != 0)
                actualLength++;

            return Global.UTF8Encoding.GetString(buffer, actualLength);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfTextBoxSetSelectedText(Ptr, value ?? string.Empty));
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Selects a range of text in the text box.
    /// </summary>
    /// <param name="start">The position of the first character in the current text selection within the text box.</param>
    /// <param name="length">The number of characters to select.</param>
    /// <remarks>
    /// This method provides a convenient way to select text programmatically. It is equivalent to
    /// setting both <see cref="SelectionStart"/> and <see cref="SelectionLength"/> properties,
    /// but does so in a single operation.
    /// If the start parameter is less than zero, it is set to zero. If the start parameter is
    /// greater than the text length, it is set to the text length. If the length parameter is
    /// negative, it is set to zero. If the length would extend beyond the end of the text,
    /// it is reduced to select to the end of the text.
    /// </remarks>
    public void Select(int start, int length)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfTextBoxSelect(Ptr, start, length));
    }

    /// <summary>
    /// Selects all text in the text box.
    /// </summary>
    /// <remarks>
    /// This method is equivalent to calling <c>Select(0, Text.Length)</c>. It is commonly used
    /// when the text box receives focus to make it easy for users to replace all existing text
    /// by simply starting to type.
    /// </remarks>
    public void SelectAll()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfTextBoxSelectAll(Ptr));
    }

    /// <summary>
    /// Clears all text from the text box control.
    /// </summary>
    /// <remarks>
    /// This method is equivalent to setting <c>Text = ""</c>. It removes all text and resets
    /// the selection to empty. The <see cref="TextChanged"/> event is triggered if the text box
    /// contained any text before clearing.
    /// </remarks>
    public void Clear()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfTextBoxClear(Ptr));
    }

    #endregion

    #region TextChanged Event

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NativeTextChangedEventHandler(void* userData)
    {
        try
        {
            if (!ObjectRegistry.TryGet(userData, out var obj))
                return;

            var textBox = (TextBox)obj!;
            ObjectDisposedException.ThrowIf(textBox.IsDisposed, textBox);
            textBox.OnTextChanged();
        }
        catch { }
    }

    /// <summary>
    /// Occurs when the value of the <see cref="Text"/> property changes.
    /// </summary>
    /// <remarks>
    /// This event is raised whenever the text content is modified, whether by user input
    /// (typing, pasting, deleting), programmatic changes through the <see cref="Text"/> property,
    /// or operations like <see cref="Clear"/> or setting <see cref="SelectedText"/>.
    /// Use this event to perform validation, update related controls, or respond to user input
    /// in real-time.
    /// </remarks>
    public event EventHandler? TextChanged;

    /// <summary>
    /// Raises the <see cref="TextChanged"/> event.
    /// </summary>
    /// <remarks>
    /// When overriding this method in a derived class, be sure to call the base implementation
    /// to ensure that registered event handlers are invoked.
    /// </remarks>
    protected virtual void OnTextChanged()
    {
        TextChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region NativeMethods

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxHash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfTextBoxGetText(void* self, out string @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfTextBoxSetText(void* self, string text);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxGetMaxLength(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxGetSelectionStart(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxSetSelectionStart(void* self, int value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxGetSelectionLength(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxSetSelectionLength(void* self, int value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxGetSelectedText(
            void* self,
            byte* buffer,
            int bufferSize
        );

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfTextBoxSetSelectedText(void* self, string text);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxSelect(void* self, int start, int length);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxSelectAll(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxClear(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfTextBoxSetTextChangedEventHandler(
            void* self,
            delegate* unmanaged[Cdecl]<void*, void> function,
            void* userData
        );
    }

    #endregion
}
