namespace TurboVision;

/// <summary>
/// A factory for creating native objects.
/// </summary>
public abstract class NativeObjectFactory<T>
    where T : NativeObjectFactory<T>, new()
{
    public delegate Error PlacementSizeDelegate(out int outSize, out int outAlignment);

    public static T Instance { get; } = new();

    private readonly nuint _alignment;

    public int PlacementSize { get; } // Oversized to account for a byte-aligned buffer.

    public NativeObjectFactory(PlacementSizeDelegate placementSizeDelegate)
    {
        TurboVisionException.Check(placementSizeDelegate(out var size, out var alignment));
        _alignment = (nuint)alignment;
        PlacementSize = size + alignment;
    }

    /// <summary>
    /// Bumps the pointer until it is properly aligned for the underlying type.
    /// It is assumed that the allocation is sufficiently oversized to account for this.
    /// </summary>
    /// <param name="ptr">Possibly unaligned pointer.</param>
    /// <returns>Aligned pointer.</returns>
    protected unsafe byte* Align(byte* ptr)
    {
        nuint p = (nuint)ptr;
        nuint bumps = (_alignment - (p % _alignment)) % _alignment; // 0 when already aligned
        return ptr + bumps;
    }

    // The subclass must implement this for each constructor.
    // public unsafe void* PlacementNew(byte* ptr)
    // {
    //     ptr = Align(ptr);
    //     TurboVisionException.Check(TV_TFoo_placementNew(ptr));
    //     return ptr;
    // }

    // The subclass must implement this for each constructor.
    // public static unsafe void* New()
    // {
    //     TurboVisionException.Check(TV_TFoo_new(out var ptr));
    //     return ptr;
    // }
}
