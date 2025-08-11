namespace TurboVision;

/// <summary>
/// A factory for creating native objects.
/// </summary>
public abstract class NativeObjectFactory<T>
    where T : NativeObjectFactory<T>, new()
{
    public delegate Error PlacementSizeDelegate(out int outSize, out int outAlignment);
    public delegate Error PlacementNewDelegate(IntPtr self);
    public delegate Error NewDelegate(out IntPtr self);

    public static T Instance { get; } = new();

    private readonly nuint _alignment;
    private readonly PlacementNewDelegate _placementNew;
    private readonly NewDelegate _new;
    
    public int PlacementSize { get; } // Oversized to account for a byte-aligned buffer.

    protected NativeObjectFactory(
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

    public IntPtr PlacementNew(IntPtr ptr)
    {
        // Align the pointer.
        nuint p = (nuint)ptr;
        nuint bumps = (_alignment - (p % _alignment)) % _alignment; // 0 when already aligned
        ptr = IntPtr.Add(ptr, (int)bumps);

        TurboVisionException.Check(_placementNew(ptr));
        return ptr;
    }

    public IntPtr New()
    {
        TurboVisionException.Check(_new(out var ptr));
        return ptr;
    }
}
