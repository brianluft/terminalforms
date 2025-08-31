namespace TerminalForms;

public abstract unsafe class TerminalFormsObject : IDisposable
{
    private readonly MetaObject _metaObject;

    public TerminalFormsObject(MetaObject metaObject)
    {
        _metaObject = metaObject;

        Check(_metaObject.NativeNew(out var ptr));
        Ptr = ptr;
    }

    public bool IsDisposed { get; private set; }
    internal void* Ptr { get; }
    internal bool IsOwned { get; set; } = true;

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (GetType() != obj.GetType())
            return false;

        Check(_metaObject.NativeEquals(Ptr, ((TerminalFormsObject)obj).Ptr, out var result));
        return result;
    }

    public override int GetHashCode()
    {
        Check(_metaObject.NativeHash(Ptr, out var hash));
        return hash;
    }

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

            // set large fields to null
            IsDisposed = true;
        }
    }

    ~TerminalFormsObject()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
