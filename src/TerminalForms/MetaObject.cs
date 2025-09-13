namespace TerminalForms;

/// <summary>
/// Encapsulates the native function pointers required for managing a specific type of Terminal Forms object.
/// This record provides the bridge between managed C# objects and their corresponding native C++ counterparts
/// by storing delegates to the native construction, destruction, equality, and hashing functions.
/// </summary>
/// <param name="NativeNew">Delegate that creates a new instance of the native object.</param>
/// <param name="NativeDelete">Delegate that destroys an instance of the native object.</param>
/// <param name="NativeEquals">Delegate that compares two instances of the native object for equality.</param>
/// <param name="NativeHash">Delegate that computes a hash code for an instance of the native object.</param>
/// <remarks>
/// Each Terminal Forms object type (Button, Form, etc.) has its own MetaObject instance that defines
/// the specific native functions to use for that type. This provides type safety and ensures that
/// each managed object calls the correct native functions for its corresponding C++ type.
/// </remarks>
internal unsafe record class MetaObject(
    MetaObject.NewDelegate NativeNew,
    MetaObject.DeleteDelegate NativeDelete,
    MetaObject.EqualsDelegate NativeEquals,
    MetaObject.HashDelegate NativeHash
)
{
    /// <summary>
    /// Defines the signature for native object construction functions.
    /// These functions allocate and initialize a new native object instance.
    /// </summary>
    /// <param name="out">Receives a pointer to the newly created native object.</param>
    /// <returns>An <see cref="Error"/> code indicating success or failure.</returns>
    public delegate Error NewDelegate(out void* @out);

    /// <summary>
    /// Defines the signature for native object destruction functions.
    /// These functions clean up and deallocate a native object instance.
    /// </summary>
    /// <param name="self">Pointer to the native object to destroy.</param>
    /// <returns>An <see cref="Error"/> code indicating success or failure.</returns>
    public delegate Error DeleteDelegate(void* self);

    /// <summary>
    /// Defines the signature for native object equality comparison functions.
    /// These functions determine if two native objects are equal.
    /// </summary>
    /// <param name="self">Pointer to the first native object to compare.</param>
    /// <param name="other">Pointer to the second native object to compare.</param>
    /// <param name="out">Receives true if the objects are equal, false otherwise.</param>
    /// <returns>An <see cref="Error"/> code indicating success or failure.</returns>
    public delegate Error EqualsDelegate(
        void* self,
        void* other,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );

    /// <summary>
    /// Defines the signature for native object hash code computation functions.
    /// These functions generate a hash code for a native object instance.
    /// </summary>
    /// <param name="self">Pointer to the native object to hash.</param>
    /// <param name="out">Receives the computed hash code.</param>
    /// <returns>An <see cref="Error"/> code indicating success or failure.</returns>
    public delegate Error HashDelegate(void* self, out int @out);
}
