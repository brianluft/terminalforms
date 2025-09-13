namespace TerminalForms;

/// <summary>
/// Represents a location in 2D space.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Point : IEquatable<Point>
{
    /// <summary>
    /// Gets or sets the X coordinate of this point in character cells.
    /// </summary>
    public int X;

    /// <summary>
    /// Gets or sets the Y coordinate of this point in character cells.
    /// </summary>
    public int Y;

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> struct with coordinates (0, 0).
    /// </summary>
    public Point() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> struct with the specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate in character cells.</param>
    /// <param name="y">The Y coordinate in character cells.</param>
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Determines whether the specified object is equal to this point.
    /// </summary>
    /// <param name="obj">The object to compare with this point.</param>
    /// <returns>true if the specified object is a Point and has the same coordinates; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is not Point other)
            return false;

        return Equals(other: other);
    }

    /// <summary>
    /// Returns the hash code for this point. The hash code is computed by the native library
    /// to ensure consistency with native point operations.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        fixed (void* ptr = &this)
        {
            Check(NativeMethods.TfPointHash(ptr, out var hash));
            return hash;
        }
    }

    /// <summary>
    /// Determines whether this point is equal to another point by comparing their coordinates.
    /// </summary>
    /// <param name="other">The point to compare with this point.</param>
    /// <returns>true if the points have the same X and Y coordinates; otherwise, false.</returns>
    public bool Equals(Point other)
    {
        fixed (void* ptr = &this)
        {
            Check(NativeMethods.TfPointEquals(ptr, &other, out var result));
            return result;
        }
    }

    /// <summary>
    /// Determines whether two points have the same coordinates.
    /// </summary>
    /// <param name="left">The first point to compare.</param>
    /// <param name="right">The second point to compare.</param>
    /// <returns>true if the points have the same coordinates; otherwise, false.</returns>
    public static bool operator ==(Point left, Point right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two points have different coordinates.
    /// </summary>
    /// <param name="left">The first point to compare.</param>
    /// <param name="right">The second point to compare.</param>
    /// <returns>true if the points have different coordinates; otherwise, false.</returns>
    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }

    private static unsafe partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfPointEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfPointHash(void* self, out int @out);
    }
}
