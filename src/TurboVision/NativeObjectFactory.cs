namespace TurboVision;

/// <summary>
/// A factory for creating native objects.
/// </summary>
public abstract class NativeObjectFactory<T>
    where T : NativeObjectFactory<T>, new()
{
    public delegate Error PlacementSizeDelegate(out int outSize, out int outAlignment);
    public unsafe delegate Error PlacementNewDelegate(byte* buffer);
    public unsafe delegate Error NewDelegate(out void* @out);

    public static T Instance { get; } = new();

    private readonly nuint _alignment;
    private readonly PlacementNewDelegate _placementNew;
    private readonly NewDelegate _new;

    public int PlacementSize { get; } // Oversized to account for a byte-aligned buffer.

    public NativeObjectFactory(
        PlacementSizeDelegate placementSizeDelegate,
        PlacementNewDelegate placementNewDelegate,
        NewDelegate newDelegate
    )
    {
        TurboVisionException.Check(placementSizeDelegate(out var size, out var alignment));
        _alignment = (nuint)alignment;
        _placementNew = placementNewDelegate;
        _new = newDelegate;
        PlacementSize = size + alignment;
    }

    public unsafe void* PlacementNew(byte* ptr)
    {
        // Align the pointer.
        nuint p = (nuint)ptr;
        nuint bumps = (_alignment - (p % _alignment)) % _alignment; // 0 when already aligned
        ptr += bumps;

        TurboVisionException.Check(_placementNew(ptr));
        return ptr;
    }

    public unsafe void* New()
    {
        TurboVisionException.Check(_new(out var ptr));
        return ptr;
    }
}
