# Binding Patterns

This document establishes the standardized conventions for creating bindings between C# and C++ code in Terminal Forms. These patterns are derived from the established implementations in `Button`, `Form`, and `CheckBox`.

## C++ Side Patterns

### File Structure Pattern

Each bindable type requires two files:

**Header File (`Foo.h`)**:
```cpp
#pragma once

#include "common.h"
// Include EventHandler.h if the type has events
#include "EventHandler.h"

// Include required Turbo Vision headers
#define Uses_TFooTVClass
#include <tvision/tv.h>

namespace tf {

class Foo : public TFooTVClass {
public:
    Foo();
    
    // Override virtual methods as needed
    virtual void someMethod() override;
    
    // Property getter/setter methods
    int32_t getSomeProperty() const;
    void setSomeProperty(int32_t value);
    BOOL getBoolProperty() const;
    void setBoolProperty(BOOL value);
    
    // Event handler methods
    void setEventHandler(EventHandlerFunction function, void* userData);

private:
    EventHandler eventHandler{};
};

// Equality comparison specialization
template<>
struct equals<Foo> {
    bool operator()(const Foo& self, const Foo& other) const { 
        return &self == &other;  // Reference equality by default
    }
};

} // namespace tf

// Hash specialization for std::unordered_map support
namespace std {
template<>
struct hash<tf::Foo> {
    std::size_t operator()(const tf::Foo& p) const noexcept {
        std::size_t x{};
        tf::combineHash(&p, &x);
        return x;
    }
};
} // namespace std
```

**Implementation File (`Foo.cpp`)**:
```cpp
#include "Foo.h"

// Include additional Turbo Vision headers as needed
#define Uses_TRect
#include <tvision/tv.h>

namespace tf {

Foo::Foo() : TFooTVClass(/* appropriate constructor args */) {}

void Foo::someMethod() {
    TFooTVClass::someMethod();
    // Call event handler if this triggers an event
    eventHandler();
}

void Foo::setEventHandler(EventHandlerFunction function, void* userData) {
    eventHandler = EventHandler(function, userData);
}

int32_t Foo::getSomeProperty() const {
    // Implementation specific to the property
    return someValue;
}

void Foo::setSomeProperty(int32_t value) {
    // Implementation specific to the property
    someValue = value;
    drawView(); // Refresh display if visual change
}

BOOL Foo::getBoolProperty() const {
    return (flags & SOME_FLAG) != 0;
}

void Foo::setBoolProperty(BOOL value) {
    if (value) {
        flags |= SOME_FLAG;
    } else {
        flags &= ~SOME_FLAG;
    }
    drawView(); // Refresh display if visual change
}

} // namespace tf

// Standard constructor/destructor/equals/hash exports
TF_DEFAULT_CONSTRUCTOR(Foo)
TF_BOILERPLATE_FUNCTIONS(Foo)

// Property getters/setters
TF_EXPORT tf::Error TfFooGetSomeProperty(tf::Foo* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getSomeProperty();
    return tf::Success;
}

TF_EXPORT tf::Error TfFooSetSomeProperty(tf::Foo* self, int32_t value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setSomeProperty(value);
    return tf::Success;
}

TF_EXPORT tf::Error TfFooGetBoolProperty(tf::Foo* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getBoolProperty();
    return tf::Success;
}

TF_EXPORT tf::Error TfFooSetBoolProperty(tf::Foo* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setBoolProperty(value);
    return tf::Success;
}

// String property getters - IMPORTANT: Use TF_STRDUP to allocate a copy!
// The .NET marshaller frees the returned pointer after copying to a managed string.
// Returning internal pointers directly causes memory corruption.
TF_EXPORT tf::Error TfFooGetText(tf::Foo* self, const char** out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = TF_STRDUP(self->getText());
    if (*out == nullptr) {
        return tf::Error_OutOfMemory;
    }
    return tf::Success;
}

TF_EXPORT tf::Error TfFooSetText(tf::Foo* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setText(text);
    return tf::Success;
}

// Event handler setup
TF_EXPORT tf::Error TfFooSetEventHandler(tf::Foo* self, tf::EventHandlerFunction function, void* userData) {
    if (self == nullptr || function == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setEventHandler(function, userData);
    return tf::Success;
}
```

### Required Macros

Every bindable type must include these macros:

```cpp
// If the type has a default constructor:
TF_DEFAULT_CONSTRUCTOR(Foo)

// Always required:
TF_BOILERPLATE_FUNCTIONS(Foo)
```

These macros generate:
- `TfFooNew(tf::Foo** out)` - Constructor
- `TfFooDelete(tf::Foo* self)` - Destructor  
- `TfFooEquals(tf::Foo* self, tf::Foo* other, BOOL* out)` - Equality
- `TfFooHash(tf::Foo* self, int32_t* out)` - Hash function

### Naming Conventions

**Function Naming:**
- Static members: `TfFooStaticBar(args)`
- Instance members: `TfFooBaz(Foo* self, args)`
- Primitive getters: `TfFooGetBar(Foo* self, Type* out)`
- Reference getters: `TfFooGetBar(Foo* self, TBar** out)`
- Primitive setters: `TfFooSetBar(Foo* self, Type value)`
- Reference setters: `TfFooSetBar(Foo* self, TBar* value)`

**Parameter Conventions:**
- Always use `self` as first parameter name for instance methods
- Always use `out` for output parameters
- Use descriptive names that match C# property names

### CMakeLists.txt Updates

Every new source file must be added to `CMakeLists.txt`:

```cmake
add_library(tfcore SHARED
    Application.cpp
    Button.cpp
    CheckBox.cpp
    common.cpp
    Control.cpp
    ControlCollection.cpp
    Form.cpp
    Foo.cpp          # <- Add new files here
    Point.cpp
    Rectangle.cpp
)
```

## C# Side Patterns

### Class Structure Pattern

```csharp
using System.Runtime.CompilerServices;  // For UnmanagedCallersOnly

namespace TerminalForms;

/// <summary>
/// XML documentation describing the control's purpose and behavior.
/// Focus on how developers will use this control, not implementation details.
/// </summary>
public unsafe partial class Foo : Control  // or ContainerControl
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfFooNew,
        NativeMethods.TfFooDelete,
        NativeMethods.TfFooEquals,
        NativeMethods.TfFooHash
    );

    /// <summary>
    /// Initializes a new instance of the Foo class.
    /// Explain what the constructor sets up and any default values.
    /// </summary>
    public Foo() : base(_metaObject)
    {
        // Set up event handlers
        Check(NativeMethods.TfFooSetEventHandler(Ptr, &NativeEventHandler, Ptr));
    }

    /// <summary>
    /// Property documentation explaining purpose, behavior, and usage.
    /// Include information about visual effects, validation, or constraints.
    /// </summary>
    public int SomeProperty
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFooGetSomeProperty(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFooSetSomeProperty(Ptr, value));
        }
    }

    /// <summary>
    /// Boolean property documentation with special marshalling requirements.
    /// </summary>
    public bool BoolProperty
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFooGetBoolProperty(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfFooSetBoolProperty(Ptr, value));
        }
    }

    #region Event Pattern
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NativeEventHandler(void* userData)
    {
        try
        {
            if (!ObjectRegistry.TryGet(userData, out var obj))
                return;

            var foo = (Foo)obj!;
            ObjectDisposedException.ThrowIf(foo.IsDisposed, foo);
            foo.OnSomeEvent();
        }
        catch { /* Swallow exceptions in native callbacks */ }
    }

    /// <summary>
    /// Occurs when something happens that developers care about.
    /// Describe when this event is raised and what developers can do with it.
    /// </summary>
    public event EventHandler? SomeEvent;

    /// <summary>
    /// Raises the SomeEvent. Called when the event condition occurs.
    /// Override in derived classes to customize event handling.
    /// </summary>
    protected virtual void OnSomeEvent()
    {
        SomeEvent?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFooNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFooDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFooEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFooHash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFooGetSomeProperty(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFooSetSomeProperty(void* self, int value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFooGetBoolProperty(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFooSetBoolProperty(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFooSetEventHandler(
            void* self,
            delegate* unmanaged[Cdecl]<void*, void> function,
            void* userData);
    }
}
```

### Property Patterns

**Simple Properties:**
```csharp
public int SimpleProperty
{
    get
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFooGetSimpleProperty(Ptr, out var value));
        return value;
    }
    set
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFooSetSimpleProperty(Ptr, value));
    }
}
```

**String Properties:**
```csharp
public string Text
{
    get
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFooGetText(Ptr, out var text));
        return text;
    }
    set
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFooSetText(Ptr, value ?? ""));
    }
}
```

**Boolean Properties (Special Marshalling):**
```csharp
public bool BoolProperty
{
    get
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFooGetBoolProperty(Ptr, out var value));
        return value;
    }
    set
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFooSetBoolProperty(Ptr, value));
    }
}
```

**Enum Properties:**
```csharp
public SomeEnum EnumProperty
{
    get
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFooGetEnumProperty(Ptr, out var value));
        return (SomeEnum)value;
    }
    set
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfFooSetEnumProperty(Ptr, (int)value));
    }
}
```

### Event Handling Pattern

All events follow this exact pattern:

```csharp
#region EventName
[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
private static void NativeEventNameEventHandler(void* userData)
{
    try
    {
        if (!ObjectRegistry.TryGet(userData, out var obj))
            return;

        var foo = (Foo)obj!;
        ObjectDisposedException.ThrowIf(foo.IsDisposed, foo);
        foo.OnEventName();
    }
    catch { /* Swallow all exceptions in native callbacks */ }
}

/// <summary>
/// Occurs when [describe the event condition].
/// </summary>
public event EventHandler? EventName;

/// <summary>
/// Raises the EventName event.
/// </summary>
protected virtual void OnEventName()
{
    EventName?.Invoke(this, EventArgs.Empty);
}
#endregion
```

### NativeMethods Patterns

**Constructor/Destructor/Boilerplate:**
```csharp
[LibraryImport(Global.DLL_NAME)]
public static partial Error TfFooNew(out void* @out);

[LibraryImport(Global.DLL_NAME)]
public static partial Error TfFooDelete(void* self);

[LibraryImport(Global.DLL_NAME)]
public static partial Error TfFooEquals(
    void* self,
    void* other,
    [MarshalAs(UnmanagedType.I4)] out bool @out);

[LibraryImport(Global.DLL_NAME)]
public static partial Error TfFooHash(void* self, out int @out);
```

**String Methods:**
```csharp
[LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
public static partial Error TfFooSetText(void* self, string text);

[LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
public static partial Error TfFooGetText(void* self, out string @out);
```

**Boolean Methods:**
```csharp
[LibraryImport(Global.DLL_NAME)]
public static partial Error TfFooGetBoolProperty(
    void* self,
    [MarshalAs(UnmanagedType.I4)] out bool @out);

[LibraryImport(Global.DLL_NAME)]
public static partial Error TfFooSetBoolProperty(
    void* self,
    [MarshalAs(UnmanagedType.I4)] bool value);
```

**Event Handler Setup:**
```csharp
[LibraryImport(Global.DLL_NAME)]
public static partial Error TfFooSetEventHandler(
    void* self,
    delegate* unmanaged[Cdecl]<void*, void> function,
    void* userData);
```

## Documentation Requirements

### XML Documentation Guidelines

**Class Documentation:**
- Focus on what the control does for the developer
- Don't mention C++, Turbo Vision, or implementation details
- Describe typical usage scenarios
- Mention inheritance relationships

**Property Documentation:**
- Explain the property's purpose and effects
- Document value ranges or constraints
- Mention visual effects or side effects
- Include examples for complex properties

**Event Documentation:**
- Describe when the event occurs
- Explain what developers can do in response
- Mention timing relative to other operations

**Method Documentation:**
- Explain the method's purpose and behavior
- Document parameters and return values
- Note any exceptions that might be thrown
- Provide usage guidance

## Testing Patterns

Each new control should include:

1. **Demo Implementation**: Create demo classes in `TerminalFormsDemo`
2. **Unit Tests**: Add to `Tests` project following MSTest patterns
3. **Expected Output**: Generate `.txt` files using `NO_TESTS=1 scripts/build.sh` then `scripts/run-demo.sh`

This standardized approach ensures consistency across all Terminal Forms bindings and makes the codebase easier to understand and maintain.
