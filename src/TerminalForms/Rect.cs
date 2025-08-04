namespace TerminalForms;

public sealed class Rect : IDisposable, IEquatable<Rect>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public Rect()
    {
        Ptr = NativeMethods.Tv_Rect_new0();
    }

    public Rect(int ax, int ay, int bx, int by)
    {
        Ptr = NativeMethods.Tv_Rect_new1(ax, ay, bx, by);
    }

    public Rect(Point p1, Point p2)
    {
        Ptr = NativeMethods.Tv_Rect_new2(p1.Ptr, p2.Ptr);
    }

    public Rect(IntPtr ptr)
    {
        Ptr = ptr;
    }

    ~Rect()
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
            NativeMethods.Tv_Rect_delete(Ptr);
            IsDisposed = true;
        }
    }

    public void Move(int aDX, int aDY)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        NativeMethods.Tv_Rect_move(Ptr, aDX, aDY);
    }

    public void Grow(int aDX, int aDY)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        NativeMethods.Tv_Rect_grow(Ptr, aDX, aDY);
    }

    public void Intersect(Rect r)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(r.IsDisposed, r);
        NativeMethods.Tv_Rect_intersect(Ptr, r.Ptr);
    }

    public void Union(Rect r)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(r.IsDisposed, r);
        NativeMethods.Tv_Rect_Union(Ptr, r.Ptr);
    }

    public bool Contains(Point p)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(p.IsDisposed, p);
        return NativeMethods.Tv_Rect_contains(Ptr, p.Ptr);
    }

    public bool IsEmpty()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        return NativeMethods.Tv_Rect_isEmpty(Ptr);
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as Rect);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        return NativeMethods.Tv_Rect_hash(Ptr);
    }

    public bool Equals(Rect? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);
        return NativeMethods.Tv_Rect_operator_equals(Ptr, other.Ptr);
    }

    public Point A
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            return new Point(NativeMethods.Tv_Rect_get_a(Ptr));
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ObjectDisposedException.ThrowIf(value.IsDisposed, value);
            NativeMethods.Tv_Rect_set_a(Ptr, value.Ptr);
        }
    }

    public Point B
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            return new Point(NativeMethods.Tv_Rect_get_b(Ptr));
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ObjectDisposedException.ThrowIf(value.IsDisposed, value);
            NativeMethods.Tv_Rect_set_b(Ptr, value.Ptr);
        }
    }
}
