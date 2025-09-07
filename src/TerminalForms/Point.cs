namespace TerminalForms;

/// <summary>
/// Represents a location in 2D space.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Point : IEquatable<Point>
{
    public int X;
    public int Y;

    public Point() { }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (obj is not Point other)
            return false;

        return Equals(other: other);
    }

    public override int GetHashCode()
    {
        fixed (void* ptr = &this)
        {
            Check(NativeMethods.TfPointHash(ptr, out var hash));
            return hash;
        }
    }

    public bool Equals(Point other)
    {
        fixed (void* ptr = &this)
        {
            Check(NativeMethods.TfPointEquals(ptr, &other, out var result));
            return result;
        }
    }

    public static bool operator ==(Point left, Point right)
    {
        return left.Equals(right);
    }

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
