using System.Collections;

namespace TerminalForms;

/// <summary>
/// Manages the child controls of a <see cref="ContainerControl"/>.
/// </summary>
/// <param name="containerControl">The container control that owns this control collection.</param>
public unsafe partial class ControlCollection(ContainerControl containerControl) : IList<Control>
{
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
    /// Gets the first control with the specified name.
    /// </summary>
    /// <param name="name">The name of the control to find.</param>
    /// <returns>
    /// The first <see cref="Control"/> with the specified name, or null if no control
    /// with that name exists in the collection.
    /// </returns>
    /// <remarks>
    /// Control names do not need to be unique within a collection. If multiple controls
    /// have the same name, this indexer returns the first one found in the collection.
    /// The search is case-sensitive.
    /// </remarks>
    public Control? this[string name]
    {
        get
        {
            ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
            ArgumentNullException.ThrowIfNull(name);

            foreach (var control in _controls)
            {
                if (control.Name == name)
                    return control;
            }
            return null;
        }
    }

    /// <summary>
    /// Adds a control to the end of the collection.
    /// </summary>
    /// <param name="control">The control to add.</param>
    public void Add(Control control)
    {
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
        ArgumentNullException.ThrowIfNull(control);

        // Add to C# list first
        _controls.Add(control);

        // Then add to C++ TGroup
        Check(NativeMethods.TfControlCollectionInsert(containerControl.Ptr, control.Ptr));

        // The TGroup has taken ownership of the control
        control.IsOwned = false;
    }

    /// <summary>
    /// Removes all controls from the collection.
    /// </summary>
    public void Clear()
    {
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
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
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
        return _controls.Contains(control);
    }

    /// <summary>
    /// Copies the controls to an array, starting at a particular array index.
    /// </summary>
    /// <param name="array">The destination array.</param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    public void CopyTo(Control[] array, int arrayIndex)
    {
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
        _controls.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    public IEnumerator<Control> GetEnumerator()
    {
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
        return _controls.GetEnumerator();
    }

    /// <summary>
    /// Determines the index of a specific control in the collection.
    /// </summary>
    /// <param name="control">The control to locate.</param>
    /// <returns>The index of the control if found; otherwise, -1.</returns>
    public int IndexOf(Control control)
    {
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
        return _controls.IndexOf(control);
    }

    /// <summary>
    /// Inserts a control at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index at which to insert the control.</param>
    /// <param name="control">The control to insert.</param>
    public void Insert(int index, Control control)
    {
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
        ArgumentNullException.ThrowIfNull(control);
        if (index < 0 || index > _controls.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        // Insert into C# list first
        _controls.Insert(index, control);

        // Then insert into C++ TGroup
        Check(NativeMethods.TfControlCollectionInsertAt(containerControl.Ptr, index, control.Ptr));

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
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
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
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
        if (index < 0 || index >= _controls.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        // Remove from C++ side first
        Check(NativeMethods.TfControlCollectionRemoveAt(containerControl.Ptr, index));

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
        ObjectDisposedException.ThrowIf(containerControl.IsDisposed, containerControl);
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
