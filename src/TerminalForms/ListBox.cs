using System.Runtime.CompilerServices;

namespace TerminalForms;

/// <summary>
/// Represents a scrollable list of string items that allows the user to select a single item.
/// The list box displays items in a vertical scrollable list, with keyboard and mouse support
/// for navigation and selection.
/// </summary>
/// <remarks>
/// The ListBox control provides a simple way to present a list of choices to the user.
/// Users can navigate the list using arrow keys, Page Up/Down, Home/End, or by clicking
/// with the mouse. Items can be selected by double-clicking or pressing Space/Enter.
///
/// The control automatically manages a vertical scrollbar when the list contains more
/// items than can be displayed at once.
///
/// The <see cref="SelectedIndex"/> property returns -1 when no item is selected (either
/// because the list is empty or selection has been explicitly cleared). The
/// <see cref="SelectedIndexChanged"/> event fires when the selection changes, and the
/// <see cref="ItemActivated"/> event fires when the user activates an item through
/// double-click or keyboard action.
/// </remarks>
/// <example>
/// <code>
/// var listBox = new ListBox();
/// listBox.Items.Add("Apple");
/// listBox.Items.Add("Banana");
/// listBox.Items.Add("Cherry");
///
/// listBox.SelectedIndexChanged += (sender, e) =>
/// {
///     Console.WriteLine($"Selected: {listBox.SelectedItem}");
/// };
///
/// listBox.ItemActivated += (sender, e) =>
/// {
///     Console.WriteLine($"Activated: {listBox.SelectedItem}");
/// };
///
/// form.Controls.Add(listBox);
/// </code>
/// </example>
public unsafe partial class ListBox : Control
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfListBoxNew,
        NativeMethods.TfListBoxDelete,
        NativeMethods.TfListBoxEquals,
        NativeMethods.TfListBoxHash
    );

    private ListBoxItemCollection? _items;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListBox"/> class with an empty item list.
    /// The list box starts with no items and no selection (SelectedIndex = -1).
    /// </summary>
    /// <remarks>
    /// After construction, use the <see cref="Items"/> collection to add items to the list.
    /// A vertical scrollbar is automatically created and managed by the control.
    /// </remarks>
    public ListBox()
        : base(_metaObject)
    {
        Check(
            NativeMethods.TfListBoxSetSelectedIndexChangedEventHandler(
                Ptr,
                &NativeSelectedIndexChangedEventHandler,
                Ptr
            )
        );
        Check(
            NativeMethods.TfListBoxSetItemActivatedEventHandler(
                Ptr,
                &NativeItemActivatedEventHandler,
                Ptr
            )
        );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ListBox"/> class with the specified items.
    /// The first item is automatically selected if any items are provided.
    /// </summary>
    /// <param name="items">The initial items to display in the list box.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="items"/> is null, or any item in the array is null.
    /// </exception>
    public ListBox(params string[] items)
        : this()
    {
        ArgumentNullException.ThrowIfNull(items);

        foreach (var item in items)
        {
            ArgumentNullException.ThrowIfNull(item);
            Items.Add(item);
        }
    }

    /// <summary>
    /// Gets the collection of items displayed in this list box.
    /// </summary>
    /// <value>
    /// A <see cref="ListBoxItemCollection"/> containing the string labels for each list item.
    /// </value>
    /// <remarks>
    /// Use this collection to add, remove, or modify items at runtime. The collection
    /// maintains synchronization with the visual display automatically. When items are
    /// modified, the selected index may be adjusted to remain valid. Duplicate strings
    /// are allowed in the collection.
    /// </remarks>
    public ListBoxItemCollection Items
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            if (_items == null)
            {
                _items = new ListBoxItemCollection(this);
                _items.SyncFromNative();
            }
            return _items;
        }
    }

    /// <summary>
    /// Gets or sets the zero-based index of the currently selected item.
    /// </summary>
    /// <value>
    /// The zero-based index of the selected item, or -1 if no item is selected.
    /// </value>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The value being set is less than -1 or greater than or equal to the item count.
    /// </exception>
    /// <remarks>
    /// Setting this property changes the focused item and raises the
    /// <see cref="SelectedIndexChanged"/> event. Setting to -1 clears the selection
    /// (though if items exist, focus remains on the first item for keyboard navigation).
    /// The event fires for both programmatic and user-initiated changes.
    /// </remarks>
    public int SelectedIndex
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfListBoxGetSelectedIndex(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            var count = Items.Count;
            if (count == 0)
            {
                if (value != -1)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "SelectedIndex must be -1 when there are no items."
                    );
                }
                return;
            }
            if (value < -1 || value >= count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"SelectedIndex must be between -1 and {count - 1}."
                );
            }
            Check(NativeMethods.TfListBoxSetSelectedIndex(Ptr, value));
        }
    }

    /// <summary>
    /// Gets or sets the currently selected item text.
    /// </summary>
    /// <value>
    /// The string value of the currently selected item, or null if no item is selected.
    /// </value>
    /// <exception cref="ArgumentException">
    /// The value being set is not found in the <see cref="Items"/> collection.
    /// </exception>
    /// <remarks>
    /// Setting this property finds the matching item in the collection and sets
    /// <see cref="SelectedIndex"/> accordingly. The comparison is case-sensitive.
    /// If the same string appears multiple times, the first occurrence is selected.
    /// Setting to null clears the selection.
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
                ClearSelection();
                return;
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

    /// <summary>
    /// Clears the current selection, setting <see cref="SelectedIndex"/> to -1.
    /// </summary>
    /// <remarks>
    /// If the list contains items, keyboard focus will remain on an item for navigation
    /// purposes, but the <see cref="SelectedIndex"/> will report -1 to indicate no
    /// logical selection. Use this method when you want to reset the list to an
    /// unselected state.
    /// </remarks>
    public void ClearSelection()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfListBoxClearSelection(Ptr));
    }

    #region SelectedIndexChanged Event

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NativeSelectedIndexChangedEventHandler(void* userData)
    {
        try
        {
            if (!ObjectRegistry.TryGet(userData, out var obj))
                return;

            var listBox = (ListBox)obj!;
            ObjectDisposedException.ThrowIf(listBox.IsDisposed, listBox);
            listBox.OnSelectedIndexChanged();
        }
        catch { }
    }

    /// <summary>
    /// Occurs when the <see cref="SelectedIndex"/> property value changes, either through
    /// user navigation or programmatic changes.
    /// </summary>
    /// <remarks>
    /// This event fires when the user navigates to a different item using the keyboard
    /// or mouse, or when <see cref="SelectedIndex"/> or <see cref="SelectedItem"/> is
    /// set programmatically. Use this event to respond to selection changes, such as
    /// updating a details panel or enabling/disabling related controls.
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

    #region ItemActivated Event

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NativeItemActivatedEventHandler(void* userData)
    {
        try
        {
            if (!ObjectRegistry.TryGet(userData, out var obj))
                return;

            var listBox = (ListBox)obj!;
            ObjectDisposedException.ThrowIf(listBox.IsDisposed, listBox);
            listBox.OnItemActivated();
        }
        catch { }
    }

    /// <summary>
    /// Occurs when the user activates an item by double-clicking it or pressing Space/Enter.
    /// </summary>
    /// <remarks>
    /// This event represents a deliberate action by the user to "activate" or "open" the
    /// selected item, as opposed to simply navigating to it. Use this event to perform
    /// the primary action associated with the selected item, such as opening a file,
    /// applying a setting, or navigating to a detail view.
    ///
    /// Note that <see cref="SelectedIndexChanged"/> will typically fire before this event
    /// if the activation also changes the selection.
    /// </remarks>
    public event EventHandler? ItemActivated;

    /// <summary>
    /// Raises the <see cref="ItemActivated"/> event.
    /// </summary>
    /// <remarks>
    /// When overriding this method in derived classes, be sure to call the base implementation
    /// to ensure that registered event handlers are properly invoked.
    /// </remarks>
    protected virtual void OnItemActivated()
    {
        ItemActivated?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxHash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxSetSelectedIndexChangedEventHandler(
            void* self,
            delegate* unmanaged[Cdecl]<void*, void> function,
            void* userData
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxSetItemActivatedEventHandler(
            void* self,
            delegate* unmanaged[Cdecl]<void*, void> function,
            void* userData
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxGetSelectedIndex(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxSetSelectedIndex(void* self, int value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxClearSelection(void* self);
    }
}
