namespace TerminalForms;

/// <summary>
/// Maintains a mapping from C++ object pointers to their corresponding C# objects.
/// This allows callbacks to make the leap from the C++ side to the correct C# object.
/// </summary>
public static unsafe class ObjectRegistry
{
    private static readonly Dictionary<IntPtr, WeakReference<TerminalFormsObject>> _objects = [];

    public static void Register(TerminalFormsObject obj)
    {
        _objects[(IntPtr)obj.Ptr] = new WeakReference<TerminalFormsObject>(obj);
    }

    public static void Unregister(TerminalFormsObject obj)
    {
        _objects.Remove((IntPtr)obj.Ptr);
    }

    public static bool TryGet(void* ptr, out TerminalFormsObject? @out)
    {
        @out = null;
        return _objects.TryGetValue((IntPtr)ptr, out var weakReference)
            && weakReference.TryGetTarget(out @out);
    }
}
