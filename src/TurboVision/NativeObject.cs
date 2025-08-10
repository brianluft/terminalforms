namespace TurboVision;

/// <summary>
/// A base class for all native objects, with conventions for construction, destruction, equality, and hashing to make
/// the C++ objects into well-behaved and predictable .NET objects.
/// </summary>
/// <typeparam name="T">The type of the subclass.</typeparam>
/// <param name="ptr">The pointer to the native object.</param>
/// <param name="owned">Whether the native object is owned by the current instance.</param>
public abstract class NativeObject<T>(IntPtr ptr, bool owned) : IDisposable, IEquatable<T>
    where T : NativeObject<T>
{
    public bool IsDisposed { get; private set; }

    internal IntPtr Ptr { get; } = ptr;
    public bool Owned { get; } = owned;

    ~NativeObject()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (Owned)
            {
                DeleteCore();
            }
            IsDisposed = true;
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as T);
    }

    public bool Equals(T? other)
    {
        if (ReferenceEquals(this, other))
            return true;
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);

        return EqualsCore(other);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);

        return GetHashCodeCore();
    }

    protected abstract void DeleteCore();
    protected abstract bool EqualsCore(T other);
    protected abstract int GetHashCodeCore();
}
