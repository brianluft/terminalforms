using System.Collections;

namespace TerminalForms;

/// <summary>
/// Represents a collection of string items in a <see cref="ListBox"/> control.
/// This collection maintains synchronization between the managed list and the native
/// list box item storage.
/// </summary>
/// <remarks>
/// The collection supports standard list operations such as adding, removing, and modifying items.
/// Changes to the collection are immediately reflected in the visual display of the list box.
/// When items are removed, the selected index is automatically adjusted to maintain a valid selection.
/// Duplicate strings are allowed in the collection.
/// </remarks>
public unsafe partial class ListBoxItemCollection : IList<string>
{
    private readonly ListBox _owner;
    private readonly List<string> _items = [];

    internal ListBoxItemCollection(ListBox owner)
    {
        _owner = owner;
    }

    /// <summary>
    /// Gets the number of items in the collection.
    /// </summary>
    public int Count
    {
        get
        {
            ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
            return _items.Count;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the collection is read-only. Always returns false.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Gets or sets the item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the item to get or set.</param>
    /// <returns>The item at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is null when setting.
    /// </exception>
    public string this[int index]
    {
        get
        {
            ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _items[index];
        }
        set
        {
            ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
            ArgumentNullException.ThrowIfNull(value);
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            _items[index] = value;
            Check(NativeMethods.TfListBoxSetItemAt(_owner.Ptr, index, value));
        }
    }

    /// <summary>
    /// Adds an item to the end of the collection.
    /// </summary>
    /// <param name="item">The string to add to the collection.</param>
    /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
    /// <remarks>
    /// If this is the first item added to an empty list, it will automatically become selected.
    /// </remarks>
    public void Add(string item)
    {
        ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
        Check(NativeMethods.TfListBoxAddItem(_owner.Ptr, item));
    }

    /// <summary>
    /// Removes all items from the collection.
    /// </summary>
    /// <remarks>
    /// After clearing the collection, the list box will have no items to display and
    /// <see cref="ListBox.SelectedIndex"/> will be -1.
    /// </remarks>
    public void Clear()
    {
        ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
        _items.Clear();
        Check(NativeMethods.TfListBoxClearItems(_owner.Ptr));
    }

    /// <summary>
    /// Determines whether the collection contains a specific item.
    /// </summary>
    /// <param name="item">The string to locate in the collection.</param>
    /// <returns>true if <paramref name="item"/> is found in the collection; otherwise, false.</returns>
    public bool Contains(string item)
    {
        ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
        return _items.Contains(item);
    }

    /// <summary>
    /// Copies the elements of the collection to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    public void CopyTo(string[] array, int arrayIndex)
    {
        ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
        _items.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    public IEnumerator<string> GetEnumerator()
    {
        ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
        return _items.GetEnumerator();
    }

    /// <summary>
    /// Searches for the specified item and returns its zero-based index.
    /// </summary>
    /// <param name="item">The string to locate in the collection.</param>
    /// <returns>
    /// The zero-based index of <paramref name="item"/> if found; otherwise, -1.
    /// If the same string appears multiple times, the index of the first occurrence is returned.
    /// </returns>
    public int IndexOf(string item)
    {
        ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
        return _items.IndexOf(item);
    }

    /// <summary>
    /// Inserts an item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
    /// <param name="item">The string to insert.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than 0 or greater than <see cref="Count"/>.
    /// </exception>
    /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
    /// <remarks>
    /// When an item is inserted before or at the current selection, the selected index
    /// is automatically incremented to maintain the same item selected.
    /// </remarks>
    public void Insert(int index, string item)
    {
        ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
        ArgumentNullException.ThrowIfNull(item);
        if (index < 0 || index > _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _items.Insert(index, item);
        Check(NativeMethods.TfListBoxInsertItemAt(_owner.Ptr, index, item));
    }

    /// <summary>
    /// Removes the first occurrence of a specific item from the collection.
    /// </summary>
    /// <param name="item">The string to remove from the collection.</param>
    /// <returns>
    /// true if <paramref name="item"/> was successfully removed; otherwise, false.
    /// This method also returns false if <paramref name="item"/> was not found.
    /// </returns>
    /// <remarks>
    /// When an item is removed, the selected index may be automatically adjusted:
    /// <list type="bullet">
    /// <item>If the removed item was selected, the selection moves to the same index position (or previous if at end).</item>
    /// <item>If the removed item was before the selected item, the selected index is decremented.</item>
    /// </list>
    /// </remarks>
    public bool Remove(string item)
    {
        ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
        var index = _items.IndexOf(item);
        if (index < 0)
            return false;
        RemoveAt(index);
        return true;
    }

    /// <summary>
    /// Removes the item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the item to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="index"/> is less than 0 or greater than or equal to <see cref="Count"/>.
    /// </exception>
    /// <remarks>
    /// When an item is removed, the selected index may be automatically adjusted:
    /// <list type="bullet">
    /// <item>If the removed item was selected, the selection moves to the same index position (or previous if at end).</item>
    /// <item>If the removed item was before the selected item, the selected index is decremented.</item>
    /// </list>
    /// </remarks>
    public void RemoveAt(int index)
    {
        ObjectDisposedException.ThrowIf(_owner.IsDisposed, _owner);
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _items.RemoveAt(index);
        Check(NativeMethods.TfListBoxRemoveItemAt(_owner.Ptr, index));
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Initializes the collection with items from the native side.
    /// Called during ListBox construction if items already exist.
    /// </summary>
    internal void SyncFromNative()
    {
        _items.Clear();
        Check(NativeMethods.TfListBoxGetItemCount(_owner.Ptr, out var count));
        for (var i = 0; i < count; i++)
        {
            Check(NativeMethods.TfListBoxGetItemAt(_owner.Ptr, i, out var item));
            _items.Add(item);
        }
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxGetItemCount(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfListBoxGetItemAt(void* self, int index, out string @out);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfListBoxSetItemAt(void* self, int index, string text);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfListBoxAddItem(void* self, string text);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfListBoxInsertItemAt(void* self, int index, string text);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxRemoveItemAt(void* self, int index);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfListBoxClearItems(void* self);
    }
}
