# Type Mapping Reference

This document defines the precise type mappings between C# and C++ in Terminal Forms. Consistency in type usage is critical for proper marshalling and interoperability.

## Fundamental Type Mappings

### Integer Types

| C# Type | C++ Type | Size | Usage | Notes |
|---------|----------|------|-------|-------|
| `int` | `int32_t` | 4 bytes | General integers, coordinates, counts | Always prefer `int32_t` over `int` in C++ |
| `short` | `int16_t` | 2 bytes | Compact integers, legacy TV types | Used for Turbo Vision compatibility |
| `long` | `int64_t` | 8 bytes | Large integers, file sizes | Rarely used in UI code |
| `byte` | `uint8_t` | 1 byte | Byte data, color values | Unsigned byte values |

**Example:**
```cpp
// C++ - Always use explicit sized types
TF_EXPORT tf::Error TfFooSetPosition(tf::Foo* self, int32_t x, int32_t y);
```

```csharp
// C# - Use standard .NET types
[LibraryImport(Global.DLL_NAME)]
public static partial Error TfFooSetPosition(void* self, int x, int y);
```

### Boolean Types

| C# Type | C++ Type | Marshalling | Usage |
|---------|----------|-------------|--------|
| `bool` | `BOOL` (int32_t) | `[MarshalAs(UnmanagedType.I4)]` | All boolean values |

**Critical Rules:**
- **Never use C++ `bool`** - it's 1 byte and causes marshalling issues
- **Always use `BOOL`** - it's defined as `int32_t` (4 bytes)
- **Always specify marshalling** in C# with `[MarshalAs(UnmanagedType.I4)]`

**Example:**
```cpp
// C++ - Use BOOL (4-byte int)
TF_EXPORT tf::Error TfButtonGetIsDefault(tf::Button* self, BOOL* out);
```

```csharp
// C# - Explicit marshalling required
[LibraryImport(Global.DLL_NAME)]
public static partial Error TfButtonGetIsDefault(
    void* self,
    [MarshalAs(UnmanagedType.I4)] out bool @out);
```

### String Types

| C# Type | C++ Type | Marshalling | Usage |
|---------|----------|-------------|-------|
| `string` | `const char*` (UTF-8) | `StringMarshalling.Utf8` | Text properties, names |
| `string` (out) | `const char**` | Custom handling | Return strings from C++ |

**String Handling Rules:**
- **Always use UTF-8 encoding** - specified with `StringMarshalling = StringMarshalling.Utf8`
- **Input strings**: Use `const char*` in C++, `string` in C# with UTF-8 marshalling
- **Output strings**: Use `const char**` in C++, `out string` in C#
- **Avoid manual marshalling** - prefer `LibraryImport` over `Marshal.StringToHGlobalAnsi`

**Example:**
```cpp
// C++ - UTF-8 input string
TF_EXPORT tf::Error TfButtonSetText(tf::Button* self, const char* text);

// C++ - UTF-8 output string  
TF_EXPORT tf::Error TfButtonGetText(tf::Button* self, const char** out);
```

```csharp
// C# - UTF-8 marshalling for input
[LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
public static partial Error TfButtonSetText(void* self, string text);

// C# - UTF-8 marshalling for output
[LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
public static partial Error TfButtonGetText(void* self, out string @out);
```

## Structure Types

### Rectangle (Geometric Types)

| C# Type | C++ Type | Layout | Usage |
|---------|----------|--------|-------|
| `Rectangle` | `tf::Rectangle` | Sequential | Control bounds, screen regions |
| `Point` | `tf::Point` | Sequential | Coordinates, positions |
| `Size` | C# record struct | N/A | Dimensions (C# only) |

**Layout Requirements:**
- Use `[StructLayout(LayoutKind.Sequential)]` in C#
- Ensure C++ struct matches C# field order exactly
- Use `int32_t` for all coordinate and size values

**Example:**
```cpp
// C++ struct definition
namespace tf {
struct Rectangle {
    int32_t x;
    int32_t y; 
    int32_t width;
    int32_t height;
    
    Rectangle(const TRect& rect);
    TRect toTRect() const;
};
}
```

```csharp
// C# struct definition - must match C++ layout
[StructLayout(LayoutKind.Sequential)]
public unsafe partial struct Rectangle : IEquatable<Rectangle>
{
    public int X;     // matches tf::Rectangle::x
    public int Y;     // matches tf::Rectangle::y
    public int Width; // matches tf::Rectangle::width
    public int Height;// matches tf::Rectangle::height
}
```

### Pointer Types

| C# Type | C++ Type | Usage | Notes |
|---------|----------|-------|-------|
| `void*` | `void*` | Object handles | Native object pointers |
| `void*` (out) | `void**` | Constructor output | New object creation |
| `delegate* unmanaged[Cdecl]` | Function pointer | Event callbacks | C# to C++ callbacks |

**Object Handle Pattern:**
```cpp
// C++ - Object creation
TF_EXPORT tf::Error TfButtonNew(tf::Button** out);

// C++ - Object method
TF_EXPORT tf::Error TfButtonSetText(tf::Button* self, const char* text);
```

```csharp
// C# - Constructor
[LibraryImport(Global.DLL_NAME)]
public static partial Error TfButtonNew(out void* @out);

// C# - Method call
[LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
public static partial Error TfButtonSetText(void* self, string text);
```

## Enum Mappings

| C# Enum | C++ Representation | Marshalling | Usage |
|---------|-------------------|-------------|-------|
| Custom enums | `int32_t` | Cast to/from int | Property values, flags |
| `Error` enum | `tf::Error` enum | Direct | Error codes |

**Enum Pattern:**
```csharp
// C# enum definition
public enum ButtonTextAlignment
{
    Center = 0,
    Left = 1,
}

// C# property using enum
public ButtonTextAlignment TextAlign
{
    get
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfButtonGetTextAlign(Ptr, out var value));
        return (ButtonTextAlignment)value;  // Cast from int
    }
    set
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfButtonSetTextAlign(Ptr, (int)value));  // Cast to int
    }
}
```

```cpp
// C++ - Store/return as int32_t
TF_EXPORT tf::Error TfButtonGetTextAlign(tf::Button* self, int32_t* out);
TF_EXPORT tf::Error TfButtonSetTextAlign(tf::Button* self, int32_t value);
```

## Function Pointer Types

### Event Callbacks

| C# Type | C++ Type | Calling Convention |
|---------|----------|--------------------|
| `delegate* unmanaged[Cdecl]<void*, void>` | `void (*)(void*)` | `__cdecl` |

**Event Handler Pattern:**
```cpp
// C++ callback type
typedef void(TF_CDECL* EventHandlerFunction)(void* userData);

// C++ method to set callback
TF_EXPORT tf::Error TfButtonSetClickEventHandler(
    tf::Button* self, 
    tf::EventHandlerFunction function, 
    void* userData);
```

```csharp
// C# callback declaration
[LibraryImport(Global.DLL_NAME)]
public static partial Error TfButtonSetClickEventHandler(
    void* self,
    delegate* unmanaged[Cdecl]<void*, void> function,
    void* userData);

// C# callback implementation
[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
private static void NativeClickEventHandler(void* userData)
{
    // Implementation
}
```

## Special Cases and Considerations

### Turbo Vision Types

When interfacing with Turbo Vision types, convert at the C++ boundary:

```cpp
// C++ - Convert TV types to our types
TF_EXPORT tf::Error TfControlGetBounds(TView* view, tf::Rectangle* out) {
    *out = tf::Rectangle(view->getBounds());  // Convert TRect to tf::Rectangle
    return tf::Success;
}

TF_EXPORT tf::Error TfControlSetBounds(TView* view, const tf::Rectangle* value) {
    TRect bounds = value->toTRect();  // Convert tf::Rectangle to TRect
    view->locate(bounds);
    return tf::Success;
}
```

### Null Handling

**C++ Side:**
```cpp
// Always check for null pointers
TF_EXPORT tf::Error TfButtonSetText(tf::Button* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    // Implementation
}
```

**C# Side:**
```csharp
// Use conditional logic for void* nulls (not ?. or ??)
public string Text
{
    get
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfButtonGetText(Ptr, out var text));
        return text;
    }
    set
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfButtonSetText(Ptr, value ?? ""));  // Handle null strings
    }
}
```

### Array and Collection Types

Arrays and collections are not directly marshalled. Instead, use individual element access patterns:

```cpp
// C++ - Collection operations
TF_EXPORT tf::Error TfControlCollectionInsertAt(void* groupPtr, int32_t index, void* controlPtr);
TF_EXPORT tf::Error TfControlCollectionRemoveAt(void* groupPtr, int32_t index);
```

```csharp
// C# - Wrap in collection class
public unsafe partial class ControlCollection : IList<Control>
{
    public void Insert(int index, Control control)
    {
        // Individual element operations
        Check(NativeMethods.TfControlCollectionInsertAt(containerControl.Ptr, index, control.Ptr));
    }
}
```

## Performance Guidelines

### Minimize Marshalling Overhead
- Use `LibraryImport` instead of `DllImport` for better performance
- Prefer value types over reference types for simple data
- Use UTF-8 string marshalling instead of manual conversion
- Avoid frequent string allocations

### Memory Layout Considerations
- Ensure struct layouts match exactly between C# and C++
- Use sequential layout for all marshalled structs  
- Prefer stack allocation for small, short-lived objects
- Use appropriate calling conventions (`Cdecl` for callbacks)

## Validation Rules

Before adding new type mappings:

1. **Verify Size Compatibility**: Ensure types have the same size and alignment
2. **Test Marshalling**: Create simple test cases for new type combinations  
3. **Check Endianness**: Verify correct byte order on target platforms
4. **Document Lifetime**: Clarify who owns and manages object lifetime
5. **Validate Thread Safety**: Ensure types work correctly across thread boundaries

Following these type mapping rules ensures reliable and efficient interop between the managed C# layer and the native C++ layer.
