namespace TerminalForms;

public sealed class Rect : IDisposable, IEquatable<Rect>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public Rect()
    {
        TerminalFormsException.Check(NativeMethods.TV_Rect_new(out var ptr));
        Ptr = ptr;
    }

    public Rect(int ax, int ay, int bx, int by)
    {
        TerminalFormsException.Check(NativeMethods.TV_Rect_new2(ax, ay, bx, by, out var ptr));
        Ptr = ptr;
    }

    public Rect(Point p1, Point p2)
    {
        TerminalFormsException.Check(NativeMethods.TV_Rect_new3(p1.Ptr, p2.Ptr, out var ptr));
        Ptr = ptr;
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
            TerminalFormsException.Check(NativeMethods.TV_Rect_delete(Ptr));
            IsDisposed = true;
        }
    }

    public void Move(int aDX, int aDY)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TerminalFormsException.Check(NativeMethods.TV_Rect_move(Ptr, aDX, aDY));
    }

    public void Grow(int aDX, int aDY)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TerminalFormsException.Check(NativeMethods.TV_Rect_grow(Ptr, aDX, aDY));
    }

    public void Intersect(Rect r)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(r.IsDisposed, r);
        TerminalFormsException.Check(NativeMethods.TV_Rect_intersect(Ptr, r.Ptr));
    }

    public void Union(Rect r)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(r.IsDisposed, r);
        TerminalFormsException.Check(NativeMethods.TV_Rect_Union(Ptr, r.Ptr));
    }

    public bool Contains(Point p)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(p.IsDisposed, p);
        TerminalFormsException.Check(NativeMethods.TV_Rect_contains(Ptr, p.Ptr, out var contains));
        return contains;
    }

    public bool IsEmpty()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TerminalFormsException.Check(NativeMethods.TV_Rect_isEmpty(Ptr, out var isEmpty));
        return isEmpty;
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as Rect);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        TerminalFormsException.Check(NativeMethods.TV_Rect_hash(Ptr, out var hash));
        return hash;
    }

    public bool Equals(Rect? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);
        TerminalFormsException.Check(NativeMethods.TV_Rect_equals(Ptr, other.Ptr, out var equals));
        return equals;
    }

    public Point A
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_Rect_get_a(Ptr, out var a));
            return new Point(a);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ObjectDisposedException.ThrowIf(value.IsDisposed, value);
            TerminalFormsException.Check(NativeMethods.TV_Rect_set_a(Ptr, value.Ptr));
        }
    }

    public Point B
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            TerminalFormsException.Check(NativeMethods.TV_Rect_get_b(Ptr, out var b));
            return new Point(b);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            ObjectDisposedException.ThrowIf(value.IsDisposed, value);
            TerminalFormsException.Check(NativeMethods.TV_Rect_set_b(Ptr, value.Ptr));
        }
    }
}
