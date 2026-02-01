using System.Runtime.CompilerServices;

namespace TerminalForms;

/// <summary>
/// Represents a group of mutually exclusive radio buttons, where selecting one automatically
/// deselects all others in the group. Radio button groups are commonly used for presenting
/// a set of options where only one choice is valid at a time.
/// </summary>
/// <remarks>
/// Unlike Windows Forms where each radio button is an individual control, this control
/// represents the entire group as a single control with multiple items. This design
/// matches the underlying Turbo Vision cluster pattern and provides simpler management
/// of mutually exclusive options.
///
/// Users can interact with radio buttons through mouse clicks, keyboard navigation
/// (arrow keys to move between options, Space to select), or keyboard shortcuts
/// (using tilde characters in item text, e.g., "~O~ption 1" for Alt+O).
///
/// The control automatically ensures that exactly one item is selected at all times
/// (as long as items exist). When items are added, removed, or modified, the selection
/// is automatically adjusted to remain valid.
/// </remarks>
/// <example>
/// <code>
/// var colorChoice = new RadioButtonGroup("~R~ed", "~G~reen", "~B~lue");
/// colorChoice.SelectedIndexChanged += (sender, e) =>
/// {
///     Console.WriteLine($"Selected: {colorChoice.SelectedItem}");
/// };
/// form.Controls.Add(colorChoice);
/// </code>
/// </example>
public unsafe partial class RadioButtonGroup : Control
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfRadioButtonGroupNew,
        NativeMethods.TfRadioButtonGroupDelete,
        NativeMethods.TfRadioButtonGroupEquals,
        NativeMethods.TfRadioButtonGroupHash
    );

    private RadioButtonItemCollection? _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="RadioButtonGroup"/> class with a single
    /// default item. The first item is automatically selected.
    /// </summary>
    /// <remarks>
    /// After construction, use the <see cref="Items"/> collection to modify the available
    /// options, or use the constructor overload that accepts initial items.
    /// </remarks>
    public RadioButtonGroup()
        : base(_metaObject)
    {
        Check(
            NativeMethods.TfRadioButtonGroupSetSelectedIndexChangedEventHandler(
                Ptr,
                &NativeSelectedIndexChangedEventHandler,
                Ptr
            )
        );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RadioButtonGroup"/> class with the
    /// specified items. The first item is automatically selected.
    /// </summary>
    /// <param name="items">The initial items to display as radio button options.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="items"/> is null, or any item in the array is null.
    /// </exception>
    /// <remarks>
    /// The items can include keyboard shortcut indicators using tilde (~) characters.
    /// For example, "~O~ption 1" creates an Alt+O shortcut for that option.
    /// </remarks>
    public RadioButtonGroup(params string[] items)
        : this()
    {
        ArgumentNullException.ThrowIfNull(items);

        // Clear the default item and add the provided items
        Items.Clear();
        foreach (var item in items)
        {
            ArgumentNullException.ThrowIfNull(item);
            Items.Add(item);
        }
    }

    /// <summary>
    /// Gets the collection of items displayed in this radio button group.
    /// </summary>
    /// <value>
    /// A <see cref="RadioButtonItemCollection"/> containing the string labels for each
    /// radio button option.
    /// </value>
    /// <remarks>
    /// Use this collection to add, remove, or modify the available options at runtime.
    /// The collection maintains synchronization with the visual display automatically.
    /// When items are modified, the selected index is adjusted to remain valid.
    /// </remarks>
    public RadioButtonItemCollection Items
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (_items == null)
            {
                _items = new RadioButtonItemCollection(this);
                _items.SyncFromNative();
            }
            return _items;
        }
    }

    /// <summary>
    /// Gets or sets the zero-based index of the currently selected item.
    /// </summary>
    /// <value>
    /// The zero-based index of the selected item, or 0 if no items exist.
    /// </value>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The value being set is less than 0 or greater than or equal to <see cref="RadioButtonItemCollection.Count"/>.
    /// </exception>
    /// <remarks>
    /// Setting this property changes the selected item and raises the
    /// <see cref="SelectedIndexChanged"/> event. The event is raised regardless of
    /// whether the change was made programmatically or through user interaction.
    /// </remarks>
    public int SelectedIndex
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfRadioButtonGroupGetSelectedIndex(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            var count = Items.Count;
            if (count == 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Cannot set SelectedIndex when there are no items."
                );
            }
            if (value < 0 || value >= count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"SelectedIndex must be between 0 and {count - 1}."
                );
            }
            Check(NativeMethods.TfRadioButtonGroupSetSelectedIndex(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets the currently selected item text.
    /// </summary>
    /// <value>
    /// The string value of the currently selected item, or null if no items exist.
    /// </value>
    /// <exception cref="ArgumentException">
    /// The value being set is not found in the <see cref="Items"/> collection.
    /// </exception>
    /// <remarks>
    /// Setting this property finds the matching item in the collection and sets
    /// <see cref="SelectedIndex"/> accordingly. The comparison is case-sensitive.
    /// If the same string appears multiple times, the first occurrence is selected.
    /// </remarks>
    public string? SelectedItem
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            var index = SelectedIndex;
            if (index < 0 || index >= Items.Count)
                return null;
            return Items[index];
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (value == null)
            {
                throw new ArgumentNullException(
                    nameof(value),
                    "Cannot set SelectedItem to null. Use SelectedIndex to change selection."
                );
            }
            var index = Items.IndexOf(value);
            if (index < 0)
            {
                throw new ArgumentException(
                    $"Item '{value}' not found in the collection.",
                    nameof(value)
                );
            }
            SelectedIndex = index;
        }
    }

    #region SelectedIndexChanged

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NativeSelectedIndexChangedEventHandler(void* userData)
    {
        try
        {
            if (!ObjectRegistry.TryGet(userData, out var obj))
                return;

            var radioButtonGroup = (RadioButtonGroup)obj!;
            ObjectDisposedException.ThrowIf(radioButtonGroup.IsDisposed, radioButtonGroup);
            radioButtonGroup.OnSelectedIndexChanged();
        }
        catch { }
    }

    /// <summary>
    /// Occurs when the <see cref="SelectedIndex"/> property value changes, either through
    /// user interaction or programmatic changes.
    /// </summary>
    /// <remarks>
    /// This event is raised after the selection has been updated. Use it to respond to
    /// user choices or to update related controls based on the new selection.
    /// The event fires for both user-initiated changes (clicking or keyboard navigation)
    /// and programmatic changes (setting <see cref="SelectedIndex"/> or <see cref="SelectedItem"/>).
    /// </remarks>
    public event EventHandler? SelectedIndexChanged;

    /// <summary>
    /// Raises the <see cref="SelectedIndexChanged"/> event.
    /// </summary>
    /// <remarks>
    /// When overriding this method in derived classes, be sure to call the base implementation
    /// to ensure that registered event handlers are properly invoked.
    /// </remarks>
    protected virtual void OnSelectedIndexChanged()
    {
        SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfRadioButtonGroupNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfRadioButtonGroupDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfRadioButtonGroupEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfRadioButtonGroupHash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfRadioButtonGroupSetSelectedIndexChangedEventHandler(
            void* self,
            delegate* unmanaged[Cdecl]<void*, void> function,
            void* userData
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfRadioButtonGroupGetSelectedIndex(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfRadioButtonGroupSetSelectedIndex(void* self, int value);
    }
}
