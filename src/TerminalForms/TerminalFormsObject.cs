namespace TerminalForms;

/// <summary>
/// Base class for all native Terminal Forms objects.
/// This class manages the lifecycle of the native object.
/// </summary>
public abstract unsafe class TerminalFormsObject : IDisposable
{
    private readonly MetaObject _metaObject;

    /// <summary>
    /// Initializes a new instance of the <see cref="TerminalFormsObject"/> class.
    /// </summary>
    /// <param name="metaObject">The meta object that defines the native functions for this object type.</param>
    internal TerminalFormsObject(MetaObject metaObject)
    {
        _metaObject = metaObject;

        Check(_metaObject.NativeNew(out var ptr));
        Ptr = ptr;

        ObjectRegistry.Register(this);
    }

    /// <summary>
    /// Whether the object has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    internal void* Ptr { get; }
    internal bool IsOwned { get; set; } = true;

    /// <summary>
    /// Determines whether the specified object is equal to this Terminal Forms object
    /// by comparing their native representations using the appropriate native equality function.
    /// </summary>
    /// <param name="obj">The object to compare with this Terminal Forms object.</param>
    /// <returns>true if the objects are equal according to their native implementations; otherwise, false.</returns>
    /// <remarks>
    /// This method uses the native equality function defined in the object's MetaObject to perform
    /// the comparison. Objects of different types are never considered equal, and null objects
    /// are never equal to non-null objects.
    /// </remarks>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (GetType() != obj.GetType())
            return false;

        Check(_metaObject.NativeEquals(Ptr, ((TerminalFormsObject)obj).Ptr, out var result));
        return result;
    }

    /// <summary>
    /// Returns the hash code for this Terminal Forms object, computed using the
    /// native hash function defined in the object's MetaObject.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code computed by the native implementation.</returns>
    /// <remarks>
    /// The hash code is computed by the native library to ensure consistency with native
    /// operations and equality comparisons. Objects that are equal according to the
    /// <see cref="Equals"/> method will have the same hash code.
    /// </remarks>
    public override int GetHashCode()
    {
        Check(_metaObject.NativeHash(Ptr, out var hash));
        return hash;
    }

    /// <summary>
    /// Releases the unmanaged resources used by the Terminal Forms object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    /// <remarks>
    /// This method is called by the public <see cref="Dispose()"/> method and the finalizer.
    /// When <paramref name="disposing"/> is true, this method releases all resources held by managed objects.
    /// The native object is only deleted if this managed object still owns it (IsOwned is true).
    /// Ownership may be transferred to other objects, such as when a form is shown on the desktop.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                // dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            if (IsOwned)
            {
                _metaObject.NativeDelete(Ptr);
            }

            ObjectRegistry.Unregister(this);

            // set large fields to null
            IsDisposed = true;
        }
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="TerminalFormsObject"/> class.
    /// This destructor will run only if the <see cref="Dispose()"/> method is not called.
    /// </summary>
    /// <remarks>
    /// The finalizer ensures that native resources are cleaned up even if the object
    /// is not properly disposed. However, it's always better to explicitly call
    /// <see cref="Dispose()"/> to ensure timely cleanup of resources.
    /// </remarks>
    ~TerminalFormsObject()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <remarks>
    /// This method releases both managed and unmanaged resources and suppresses finalization
    /// of this object. After calling this method, the object should not be used again.
    /// Multiple calls to this method are safe and will not cause errors.
    /// </remarks>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
