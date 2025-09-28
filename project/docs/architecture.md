# Terminal Forms Architecture

Terminal Forms is a C# wrapper for the Turbo Vision TUI library, providing a modern .NET interface to classic text-mode user interfaces. The architecture consists of three distinct layers, each with specific responsibilities.

## Three-Layer Architecture

### Layer 1: C# TerminalForms Layer (Public API)
**Location:** `src/TerminalForms/`
**Purpose:** Managed object model and public API

The C# layer provides the public API that .NET developers interact with. Key responsibilities:

- **Public API**: Classes like `Application`, `Form`, `Button`, `CheckBox` that developers use
- **Managed Object Model**: Type-safe wrappers around native objects with proper lifecycle management
- **P/Invoke Declarations**: `LibraryImport` declarations that interface with the C++ layer
- **Memory Safety**: Automatic disposal, object disposed checks, and resource management
- **Event System**: .NET-style events (`Click`, `CheckedChanged`) with proper delegate handling

Key classes:
- `TerminalFormsObject`: Base class for all managed wrappers
- `Control`: Base class for UI controls
- `ContainerControl`: Base for controls that can contain other controls  
- `Form`, `Button`, `CheckBox`: Concrete control implementations
- `ObjectRegistry`: Maps native pointers back to managed objects
- `MetaObject`: Type-safe function dispatch mechanism

### Layer 2: C++ tfcore Layer (C-Compatible Wrapper)
**Location:** `src/tfcore/`
**Purpose:** C-compatible wrapper around Turbo Vision

The C++ layer acts as a bridge between the managed C# code and the native Turbo Vision library. Key responsibilities:

- **C-Compatible Interface**: All exported functions use `extern "C"` and C-compatible types
- **tf Namespace**: Internal C++ implementation details live in the `tf` namespace
- **TF_EXPORT Functions**: Global namespace functions that C# can call via P/Invoke
- **Memory Management**: Safe allocation/deallocation with exception handling
- **Type Safety**: Template-based error handling and object lifecycle management

Key patterns:
- `tf::` namespace for internal C++ classes that inherit from Turbo Vision classes
- `TF_EXPORT` functions in global namespace for C# interop
- Template functions (`checkedNew`, `checkedDelete`, `checkedEquals`, `checkedHash`) for safe operations
- `EventHandler` pattern for C# callbacks

### Layer 3: Turbo Vision Layer (Core TUI Library)
**Location:** `build/sources/tvision/` (external dependency)
**Purpose:** Core terminal user interface functionality

The Turbo Vision layer provides the underlying TUI functionality:

- **Cross-Platform TUI**: Terminal-based user interface rendering and event handling
- **View Hierarchy**: Tree of views that manage screen regions and input
- **Event System**: Low-level keyboard, mouse, and system events
- **Drawing System**: Character-based drawing with colors and attributes
- **Window Management**: Overlapping windows, focus management, desktop coordination

## Key Architectural Decisions

### No Command System Usage
**Decision:** We do not use Turbo Vision's built-in command system.
**Implementation:** Override message-sending functions to invoke C# callbacks instead.
**Rationale:** Provides better integration with .NET event patterns and eliminates the need to bridge two different command/event systems.

**Example in Button:**
```cpp
// Button.cpp - Override press() to call C# callback
void Button::press() {
    TButton::press();
    clickEventHandler();  // Direct C# callback instead of command
}
```

### ObjectRegistry Pattern
**Decision:** Use a registry to map native pointers back to managed objects.
**Implementation:** `ObjectRegistry` maintains `WeakReference<TerminalFormsObject>` mappings.
**Rationale:** Enables callbacks from native code to find the correct managed object without creating GC cycles.

**Example:**
```csharp
// Button.cs - Event handler callback
[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
private static void NativeClickEventHandler(void* userData)
{
    if (!ObjectRegistry.TryGet(userData, out var obj))
        return;
    var button = (Button)obj!;
    button.PerformClick();
}
```

### MetaObject Pattern
**Decision:** Use function pointers for type-safe dispatch to native operations.
**Implementation:** Each control type has a `MetaObject` with delegates to its native functions.
**Rationale:** Provides type safety and eliminates the need for massive switch statements or reflection.

**Example:**
```csharp
// Button.cs - Type-safe function dispatch
private static readonly MetaObject _metaObject = new(
    NativeMethods.TfButtonNew,
    NativeMethods.TfButtonDelete, 
    NativeMethods.TfButtonEquals,
    NativeMethods.TfButtonHash
);
```

## Error Handling Architecture

Terminal Forms uses a comprehensive error handling system that bridges C++ exceptions to .NET exceptions:

### tf::Error Enum
**Synchronization:** `src/tfcore/common.h` and `src/TerminalForms/Error.cs` must stay synchronized.
**Core Errors:**
- `Success`: Operation completed successfully
- `ArgumentNull`: Required argument was null
- `InvalidArgument`: Argument was invalid or out of range
- `OutOfMemory`: Insufficient memory
- `Unknown`: Unspecified error occurred
- `HasMessage`: Flag indicating additional error message available

### Error Flow
1. **C++ Layer**: Catches all exceptions in template functions (`checkedNew`, `checkedDelete`, etc.)
2. **Error Codes**: Converts exceptions to `tf::Error` codes
3. **Error Messages**: Stores detailed messages in thread-local storage when `HasMessage` flag is set
4. **C# Layer**: `TerminalFormsException.Check()` converts error codes to .NET exceptions
5. **Message Retrieval**: Calls `TfGetLastErrorMessage()` when `HasMessage` is present

### Example Error Flow:
```cpp
// C++ - Safe object creation
template<typename T, typename... Args>
Error checkedNew(T** out, Args&&... args) {
    try {
        *out = new T(std::forward<Args>(args)...);
        return Success;
    } catch (const std::exception& e) {
        setLastErrorMessage(e.what());
        return static_cast<Error>(Error_Unknown | Error_HasMessage);
    }
}
```

```csharp
// C# - Convert to .NET exception
public static void Check(Error error) {
    if (error == Error.Success) return;
    
    var hasMessage = error.HasFlag(Error.HasMessage);
    error &= ~Error.HasMessage;
    
    if (hasMessage) {
        var error2 = NativeMethods.TfGetLastErrorMessage(out var message);
        if (error2 == Error.Success)
            throw new TerminalFormsException(error, message);
    }
    throw new TerminalFormsException(error);
}
```

## Thread Safety

Terminal Forms follows the typical GUI pattern:
- **Single-Threaded UI**: All UI operations must occur on the main application thread
- **Thread-Local Error Storage**: Error messages are stored per-thread to avoid race conditions
- **No Synchronization**: No locks or synchronization primitives in the hot path

## Performance Considerations

- **Direct P/Invoke**: Minimal marshalling overhead with `LibraryImport` and UTF-8 strings
- **Weak References**: `ObjectRegistry` uses weak references to prevent GC pressure
- **Template Metaprogramming**: C++ templates eliminate runtime type checking overhead  
- **Stack Allocation**: Value types like `Point`, `Rectangle`, `Size` live on the stack
- **Minimal Allocations**: String interning and careful object lifecycle management

## Extension Points

The architecture supports extension through:
- **Custom Controls**: Inherit from `Control` or `ContainerControl` in C#, create corresponding C++ classes
- **New Turbo Vision Features**: Add C++ wrapper classes and corresponding C# bindings
- **Event Handlers**: Extensible callback system for custom events
- **Error Types**: New error codes can be added to synchronized enums

This layered approach provides clean separation of concerns while maintaining high performance and type safety across the managed/native boundary.
