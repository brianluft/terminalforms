namespace TerminalForms;

/// <summary>
/// Maintains a mapping from C++ object pointers to their corresponding C# objects.
/// This allows callbacks to make the leap from the C++ side to the correct C# object.
/// </summary>
public static unsafe class ObjectRegistry
{
    private static readonly Dictionary<IntPtr, WeakReference<TerminalFormsObject>> _objects = [];

    /// <summary>
    /// Registers a Terminal Forms object in the registry, creating a mapping from its native
    /// pointer to the managed object instance. This enables callbacks from native code to
    /// find the correct managed object.
    /// </summary>
    /// <param name="obj">The Terminal Forms object to register.</param>
    /// <remarks>
    /// This method uses weak references to avoid creating circular references that would
    /// prevent garbage collection. Objects are automatically registered when created and
    /// should be unregistered when disposed.
    /// </remarks>
    public static void Register(TerminalFormsObject obj)
    {
        _objects[(IntPtr)obj.Ptr] = new WeakReference<TerminalFormsObject>(obj);
    }

    /// <summary>
    /// Removes a Terminal Forms object from the registry. This should be called when an object
    /// is being disposed to clean up the mapping and prevent memory leaks.
    /// </summary>
    /// <param name="obj">The Terminal Forms object to unregister.</param>
    /// <remarks>
    /// This method is typically called automatically during object disposal. After unregistering,
    /// callbacks from native code will no longer be able to locate this managed object.
    /// </remarks>
    public static void Unregister(TerminalFormsObject obj)
    {
        _objects.Remove((IntPtr)obj.Ptr);
    }

    /// <summary>
    /// Attempts to retrieve a managed Terminal Forms object by its native pointer.
    /// This method is primarily used by callback functions to convert native pointers
    /// back to their corresponding managed objects.
    /// </summary>
    /// <param name="ptr">The native pointer to look up.</param>
    /// <param name="out">
    /// When this method returns, contains the managed object associated with the pointer
    /// if found and still alive; otherwise, null.
    /// </param>
    /// <returns>
    /// true if a live managed object was found for the specified pointer; otherwise, false.
    /// </returns>
    /// <remarks>
    /// This method may return false if the object was never registered, has been unregistered,
    /// or has been garbage collected (since weak references are used). This is safe behavior
    /// that prevents callbacks from operating on disposed objects.
    /// </remarks>
    public static bool TryGet(void* ptr, out TerminalFormsObject? @out)
    {
        @out = null;
        return _objects.TryGetValue((IntPtr)ptr, out var weakReference)
            && weakReference.TryGetTarget(out @out);
    }
}
