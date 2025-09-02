using System.Collections;

namespace TerminalForms;

/// <summary>
/// Manages the child controls of a C++ TGroup. This collection maintains synchronization
/// between a C# List&lt;Control&gt; and the underlying C++ TGroup's linked list of views.
/// </summary>
/// <param name="groupPtr">Pointer to the C++ TGroup object to manage.</param>
public unsafe partial class ControlCollection(void* groupPtr) : IList<Control>
{
    private readonly void* _groupPtr = groupPtr;
    private readonly List<Control> _controls = [];

    /// <summary>
    /// Gets the number of controls in the collection.
    /// </summary>
    public int Count => _controls.Count;

    /// <summary>
    /// Gets a value indicating whether the collection is read-only.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Gets or sets the control at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the control to get or set.</param>
    /// <returns>The control at the specified index.</returns>
    public Control this[int index]
    {
        get => _controls[index];
        set
        {
            if (index < 0 || index >= _controls.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            RemoveAt(index);
            Insert(index, value);
        }
    }

    /// <summary>
    /// Adds a control to the end of the collection.
    /// </summary>
    /// <param name="control">The control to add.</param>
    public void Add(Control control)
    {
        ArgumentNullException.ThrowIfNull(control);

        // Add to C# list first
        _controls.Add(control);

        // Then add to C++ TGroup
        Check(NativeMethods.TfControlCollectionInsert(_groupPtr, control.Ptr));

        // The TGroup has taken ownership of the control
        control.IsOwned = false;
    }

    /// <summary>
    /// Removes all controls from the collection.
    /// </summary>
    public void Clear()
    {
        for (int i = _controls.Count - 1; i >= 0; i--)
        {
            RemoveAt(i);
        }
    }

    /// <summary>
    /// Determines whether the collection contains a specific control.
    /// </summary>
    /// <param name="control">The control to locate.</param>
    /// <returns>true if the control is found; otherwise, false.</returns>
    public bool Contains(Control control)
    {
        return _controls.Contains(control);
    }

    /// <summary>
    /// Copies the controls to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    public void CopyTo(Control[] array, int arrayIndex)
    {
        _controls.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    public IEnumerator<Control> GetEnumerator()
    {
        return _controls.GetEnumerator();
    }

    /// <summary>
    /// Determines the index of a specific control in the collection.
    /// </summary>
    /// <param name="control">The control to locate.</param>
    /// <returns>The index of the control if found; otherwise, -1.</returns>
    public int IndexOf(Control control)
    {
        return _controls.IndexOf(control);
    }

    /// <summary>
    /// Inserts a control at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which to insert the control.</param>
    /// <param name="control">The control to insert.</param>
    public void Insert(int index, Control control)
    {
        ArgumentNullException.ThrowIfNull(control);
        if (index < 0 || index > _controls.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        // Insert into C# list first
        _controls.Insert(index, control);

        // Then insert into C++ TGroup
        Check(NativeMethods.TfControlCollectionInsertAt(_groupPtr, index, control.Ptr));

        // The TGroup has taken ownership of the control
        control.IsOwned = false;
    }

    /// <summary>
    /// Removes the first occurrence of a specific control from the collection.
    /// </summary>
    /// <param name="control">The control to remove.</param>
    /// <returns>true if the control was successfully removed; otherwise, false.</returns>
    public bool Remove(Control control)
    {
        int index = IndexOf(control);
        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes the control at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the control to remove.</param>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _controls.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        // Remove from C++ side first
        Check(NativeMethods.TfControlCollectionRemoveAt(_groupPtr, index));

        // Then remove from C# list
        var control = _controls[index];
        _controls.RemoveAt(index);

        // The TGroup has relinquished ownership of the control back to us
        control.IsOwned = true;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlCollectionInsert(void* groupPtr, void* controlPtr);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlCollectionInsertAt(
            void* groupPtr,
            int index,
            void* controlPtr
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlCollectionRemoveAt(void* groupPtr, int index);
    }
}
