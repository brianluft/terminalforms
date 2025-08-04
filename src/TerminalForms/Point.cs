namespace TerminalForms;

public sealed class Point : IDisposable, IEquatable<Point>
{
    public bool IsDisposed { get; private set; }

    public IntPtr Ptr { get; }

    public Point()
    {
        Ptr = NativeMethods.Tv_Point_new0();
    }

    public Point(int x, int y)
    {
        Ptr = NativeMethods.Tv_Point_new1(x, y);
    }

    public Point(IntPtr ptr)
    {
        Ptr = ptr;
    }

    ~Point()
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
            NativeMethods.Tv_Point_delete(Ptr);
            IsDisposed = true;
        }
    }

    public void Add(Point adder)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(adder.IsDisposed, adder);
        NativeMethods.Tv_Point_operator_add_in_place(Ptr, adder.Ptr);
    }

    public void Subtract(Point subber)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        ObjectDisposedException.ThrowIf(subber.IsDisposed, subber);
        NativeMethods.Tv_Point_operator_subtract_in_place(Ptr, subber.Ptr);
    }

    public static Point operator +(Point one, Point two)
    {
        ObjectDisposedException.ThrowIf(one.IsDisposed, one);
        ObjectDisposedException.ThrowIf(two.IsDisposed, two);
        return new Point(NativeMethods.Tv_Point_operator_add(one.Ptr, two.Ptr));
    }

    public static Point operator -(Point one, Point two)
    {
        ObjectDisposedException.ThrowIf(one.IsDisposed, one);
        ObjectDisposedException.ThrowIf(two.IsDisposed, two);
        return new Point(NativeMethods.Tv_Point_operator_subtract(one.Ptr, two.Ptr));
    }

    public override bool Equals(object? obj)
    {
        return Equals(other: obj as Point);
    }

    public override int GetHashCode()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        return NativeMethods.Tv_Point_hash(Ptr);
    }

    public bool Equals(Point? other)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        if (other is null)
            return false;
        ObjectDisposedException.ThrowIf(other.IsDisposed, other);
        return NativeMethods.Tv_Point_operator_equals(Ptr, other.Ptr);
    }

    public int X
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            return NativeMethods.Tv_Point_get_x(Ptr);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            NativeMethods.Tv_Point_set_x(Ptr, value);
        }
    }

    public int Y
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            return NativeMethods.Tv_Point_get_y(Ptr);
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            NativeMethods.Tv_Point_set_y(Ptr, value);
        }
    }
}
