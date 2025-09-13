namespace TerminalForms;

/// <summary>
/// Represents the location and size of a rectangle.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Rectangle : IEquatable<Rectangle>
{
    /// <summary>
    /// Gets or sets the X coordinate of the left edge of this rectangle in character cells.
    /// </summary>
    public int X;

    /// <summary>
    /// Gets or sets the Y coordinate of the top edge of this rectangle in character cells.
    /// </summary>
    public int Y;

    /// <summary>
    /// Gets or sets the width of this rectangle in character cells.
    /// </summary>
    public int Width;

    /// <summary>
    /// Gets or sets the height of this rectangle in character cells.
    /// </summary>
    public int Height;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct with all dimensions set to zero.
    /// </summary>
    public Rectangle() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct with the specified position and size.
    /// </summary>
    /// <param name="x">The X coordinate of the left edge in character cells.</param>
    /// <param name="y">The Y coordinate of the top edge in character cells.</param>
    /// <param name="width">The width in character cells.</param>
    /// <param name="height">The height in character cells.</param>
    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> struct with the specified location and size.
    /// </summary>
    /// <param name="location">The location of the upper-left corner of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
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

    /// <summary>
    /// Determines whether the specified object is equal to this rectangle.
    /// </summary>
    /// <param name="obj">The object to compare with this rectangle.</param>
    /// <returns>true if the specified object is a Rectangle with the same position and size; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is not Rectangle other)
            return false;

        return Equals(other: other);
    }

    /// <summary>
    /// Returns the hash code for this rectangle. The hash code is computed by the native library
    /// to ensure consistency with native rectangle operations.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        fixed (void* ptr = &this)
        {
            Check(NativeMethods.TfRectangleHash(ptr, out var hash));
            return hash;
        }
    }

    /// <summary>
    /// Determines whether this rectangle is equal to another rectangle by comparing their position and size.
    /// </summary>
    /// <param name="other">The rectangle to compare with this rectangle.</param>
    /// <returns>true if the rectangles have the same X, Y, Width, and Height values; otherwise, false.</returns>
    public bool Equals(Rectangle other)
    {
        fixed (void* ptr = &this)
        {
            Check(NativeMethods.TfRectangleEquals(ptr, &other, out var result));
            return result;
        }
    }

    /// <summary>
    /// Determines whether two rectangles have the same position and size.
    /// </summary>
    /// <param name="left">The first rectangle to compare.</param>
    /// <param name="right">The second rectangle to compare.</param>
    /// <returns>true if the rectangles have the same position and size; otherwise, false.</returns>
    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two rectangles have different position or size.
    /// </summary>
    /// <param name="left">The first rectangle to compare.</param>
    /// <param name="right">The second rectangle to compare.</param>
    /// <returns>true if the rectangles have different position or size; otherwise, false.</returns>
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
