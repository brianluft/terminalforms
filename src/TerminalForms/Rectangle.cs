namespace TerminalForms;

/// <summary>
/// Represents the location and size of a rectangle.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Rectangle : IEquatable<Rectangle>
{
    public int X;
    public int Y;
    public int Width;
    public int Height;

    public Rectangle() { }

    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public Rectangle(Point location, Size size)
    {
        X = location.X;
        Y = location.Y;
        Width = size.Width;
        Height = size.Height;
    }

    /// <summary>
    /// The location of the upper-left corner of the rectangle.
    /// </summary>
    public readonly Point Location => new(X, Y);

    /// <summary>
    /// The size of the rectangle.
    /// </summary>
    public readonly Size Size => new(Width, Height);

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is not Rectangle other)
            return false;

        return Equals(other: other);
    }

    public override int GetHashCode()
    {
        fixed (void* ptr = &this)
        {
            Check(NativeMethods.TfRectangleHash(ptr, out var hash));
            return hash;
        }
    }

    public bool Equals(Rectangle other)
    {
        fixed (void* ptr = &this)
        {
            Check(NativeMethods.TfRectangleEquals(ptr, &other, out var result));
            return result;
        }
    }

    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !(left == right);
    }

    private static unsafe partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfRectangleEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfRectangleHash(void* self, out int @out);
    }
}
