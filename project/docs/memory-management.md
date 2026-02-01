# Memory Management

This document describes the memory management and ownership patterns used throughout Terminal Forms. Proper memory management is critical for preventing leaks, crashes, and other stability issues in the mixed managed/native environment.

## Ownership Model

Terminal Forms uses a **dual ownership model** where either the C# managed object or the native subsystem can own a native object at any given time.

### Ownership States

Every `TerminalFormsObject` tracks its ownership state through the `IsOwned` property:

- **`IsOwned = true`**: The C# object owns the native object and is responsible for cleanup
- **`IsOwned = false`**: Another system (like Turbo Vision's desktop) owns the native object

### Ownership Transfer Examples

**Form.Show() - Ownership Transfer:**
```csharp
public void Show()
{
    ObjectDisposedException.ThrowIf(IsDisposed, this);
    Check(NativeMethods.TfFormShow(Ptr));
    
    // TProgram::deskTop takes ownership
    IsOwned = false;  // C# no longer owns this form
}
```

**ControlCollection.Add() - Ownership Transfer:**
```csharp
public void Add(Control control)
{
    // Add to C# list first
    _controls.Add(control);
    
    // Then add to C++ TGroup
    Check(NativeMethods.TfControlCollectionInsert(containerControl.Ptr, control.Ptr));
    
    // The TGroup has taken ownership of the control
    control.IsOwned = false;
}
```

**ControlCollection.RemoveAt() - Ownership Return:**
```csharp
public void RemoveAt(int index)
{
    // Remove from C++ side first
    Check(NativeMethods.TfControlCollectionRemoveAt(containerControl.Ptr, index));
    
    // Then remove from C# list
    var control = _controls[index];
    _controls.RemoveAt(index);
    
    // The TGroup has relinquished ownership back to us
    control.IsOwned = true;
}
```

## Object Registry Pattern

The `ObjectRegistry` provides bidirectional mapping between native pointers and managed objects.

### Registration Lifecycle

**Registration (Constructor):**
```csharp
internal TerminalFormsObject(MetaObject metaObject)
{
    _metaObject = metaObject;
    Check(_metaObject.NativeNew(out var ptr));
    Ptr = ptr;
    
    ObjectRegistry.Register(this);  // Map native pointer to managed object
}
```

**Lookup (Event Callbacks):**
```csharp
[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
private static void NativeClickEventHandler(void* userData)
{
    try
    {
        // Look up managed object from native pointer
        if (!ObjectRegistry.TryGet(userData, out var obj))
            return;
            
        var button = (Button)obj!;
        button.PerformClick();
    }
    catch { /* Swallow exceptions in callbacks */ }
}
```

**Unregistration (Dispose):**
```csharp
protected virtual void Dispose(bool disposing)
{
    if (!IsDisposed)
    {
        // Clean up native resources if we still own them
        if (IsOwned)
        {
            _metaObject.NativeDelete(Ptr);
        }
        
        ObjectRegistry.Unregister(this);  // Remove from registry
        IsDisposed = true;
    }
}
```

### Weak Reference Pattern

The registry uses `WeakReference<T>` to prevent circular references:

```csharp
internal static unsafe class ObjectRegistry
{
    private static readonly Dictionary<IntPtr, WeakReference<TerminalFormsObject>> _objects = [];
    
    public static void Register(TerminalFormsObject obj)
    {
        _objects[(IntPtr)obj.Ptr] = new WeakReference<TerminalFormsObject>(obj);
    }
    
    public static bool TryGet(void* ptr, out TerminalFormsObject? @out)
    {
        @out = null;
        return _objects.TryGetValue((IntPtr)ptr, out var weakReference)
            && weakReference.TryGetTarget(out @out);
    }
}
```

**Why Weak References:**
- **Prevents GC Cycles**: Strong references would prevent garbage collection
- **Automatic Cleanup**: Objects can be GC'd when no longer referenced
- **Safe Callbacks**: Failed lookups indicate disposed objects

## Disposal Patterns

### Standard Disposal Pattern

All Terminal Forms objects implement the standard .NET disposal pattern:

```csharp
public abstract unsafe class TerminalFormsObject : IDisposable
{
    public bool IsDisposed { get; private set; }
    internal bool IsOwned { get; set; } = true;
    
    protected virtual void Dispose(bool disposing)
    {
        if (!IsDisposed)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects)
            }
            
            // Free unmanaged resources only if we own them
            if (IsOwned)
            {
                _metaObject.NativeDelete(Ptr);
            }
            
            ObjectRegistry.Unregister(this);
            IsDisposed = true;
        }
    }
    
    ~TerminalFormsObject()
    {
        Dispose(disposing: false);
    }
    
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
```

### Conditional Cleanup

The key insight is **conditional cleanup** based on ownership:

```csharp
// Only delete native object if C# still owns it
if (IsOwned)
{
    _metaObject.NativeDelete(Ptr);
}
```

This prevents:
- **Double Deletion**: Native systems won't try to delete already-deleted objects
- **Use After Free**: Native systems can safely continue using transferred objects
- **Premature Deletion**: Objects remain valid while native systems are using them

### ObjectDisposedException Guards

All public methods check disposal state:

```csharp
public string Text
{
    get
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);  // Guard check
        Check(NativeMethods.TfButtonGetText(Ptr, out var text));
        return text;
    }
    set
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);  // Guard check
        Check(NativeMethods.TfButtonSetText(Ptr, value));
    }
}
```

## Native Memory Management

### C++ Error Handling Templates

All native operations use template functions that provide exception safety:

**Safe Object Creation:**
```cpp
template<typename T, typename... Args>
Error checkedNew(T** out, Args&&... args) {
    if (!out) {
        return tf::Error_ArgumentNull;
    }
    
    try {
        *out = new T(std::forward<Args>(args)...);
        return tf::Success;
    } catch (const std::bad_alloc&) {
        return tf::Error_OutOfMemory;
    } catch (const std::exception& e) {
        setLastErrorMessage(e.what());
        return static_cast<tf::Error>(Error_Unknown | Error_HasMessage);
    }
}
```

**Safe Object Deletion:**
```cpp
template<typename T>
Error checkedDelete(T* self) {
    if (!self) {
        return tf::Success;  // Deleting null is not an error
    }
    
    try {
        delete self;
        return tf::Success;
    } catch (const std::exception& e) {
        setLastErrorMessage(e.what());
        return static_cast<tf::Error>(Error_Unknown | Error_HasMessage);
    }
}
```

### Native Constructor Pattern

The standard native constructor pattern:

```cpp
TF_DEFAULT_CONSTRUCTOR(Button)
// Expands to:
TF_EXPORT tf::Error TfButtonNew(tf::Button** out) {
    return tf::checkedNew(out);
}

TF_BOILERPLATE_FUNCTIONS(Button)  
// Expands to delete, equals, and hash functions
```

## Common Ownership Scenarios

### Scenario 1: Button in Form (Contained Controls)

```csharp
using var form = new Form();
var button = new Button();  // button.IsOwned = true

form.Controls.Add(button);   // button.IsOwned = false (form owns it)
// When form disposes, it cleans up button
// When button disposes, it does NOT delete native object
```

### Scenario 2: Standalone Form (Desktop Management)

```csharp
var form = new Form();       // form.IsOwned = true
form.Show();                 // form.IsOwned = false (desktop owns it)
// form.Dispose() will NOT delete native object
// Desktop manages lifetime
```

### Scenario 3: Temporary Objects (Full C# Ownership)

```csharp
using var button = new Button();  // button.IsOwned = true
button.Text = "Click me";
// Dispose() will delete native object
```

### Scenario 4: Removed Controls (Ownership Return)

```csharp
using var form = new Form();
var button = new Button();

form.Controls.Add(button);      // button.IsOwned = false
form.Controls.Remove(button);   // button.IsOwned = true
// Now button.Dispose() will clean up native object
```

## Memory Leak Prevention

### Automatic Cleanup Mechanisms

1. **Finalizers**: Ensure cleanup even without explicit disposal
2. **Weak References**: Prevent circular references in registry
3. **Conditional Deletion**: Only delete owned objects
4. **Exception Safety**: C++ templates handle all allocation failures

### Best Practices for Developers

**Always Use `using` Statements:**
```csharp
// Good - automatic disposal
using var form = new Form();

// Bad - manual disposal required  
var form = new Form();
// ... must call form.Dispose() manually
```

**Don't Dispose Transferred Objects:**
```csharp
var form = new Form();
var button = new Button();

form.Controls.Add(button);  // Ownership transferred
form.Show();               // Form ownership transferred

// Don't dispose button - form owns it
// Don't dispose form - desktop owns it
```

**Check Disposal in Event Handlers:**
```csharp
button.Click += (sender, e) => {
    // Objects might be disposed when events fire
    if (button.IsDisposed) return;
    
    // Safe to use button here
    Console.WriteLine(button.Text);
};
```

## Threading Considerations

### Thread Safety

- **Single-Threaded UI**: All UI operations must occur on the main thread
- **Thread-Local Errors**: Error messages are stored per-thread
- **No Synchronization**: No locks in the memory management code

### Cross-Thread Cleanup

Objects can be safely disposed from any thread:

```csharp
// Safe to call from any thread
Task.Run(() => button.Dispose());
```

The native cleanup is thread-safe, but UI operations are not.

## Application.OpenForms Pattern

The `Application.OpenForms` collection keeps strong references to all shown forms, preventing garbage collection while they are visible. This is critical for ensuring event handlers can safely access their captured state.

### How It Works

```csharp
// When Form.Show() is called:
public void Show()
{
    Check(NativeMethods.TfFormShow(Ptr));
    IsOwned = false;  // Desktop owns native object

    Application.RegisterOpenForm(this);  // Strong reference prevents GC
}
```

```csharp
// When form is closed (user clicks X or Close() called):
// C++ fires the closed event handler, which calls:
Application.UnregisterOpenForm(this);  // Allow GC
```

### Reference Chain

When a form is shown, the following reference chain keeps all related objects alive:

```
Application.OpenForms (static)
    └── Form
        └── Controls (ControlCollection)
            └── Button
                └── Click (EventHandler delegate)
                    └── Target (user's object with instance fields)
```

This ensures that:
- Forms are not garbage collected while visible
- Child controls are kept alive by their parent's Controls collection
- Event handler targets (objects with instance methods/fields) are kept alive by the delegate

### The Form.Closed Event

Forms fire the `Closed` event after being removed from the desktop:

```csharp
form.Closed += (sender, e) =>
{
    // Form has been closed
    // After this event, form may be garbage collected
    Console.WriteLine("Form closed");
};
```

This event fires for both programmatic closes (`form.Close()`) and user-initiated closes (clicking the X button).

## Debugging Memory Issues

### Common Patterns to Watch For

**Double Disposal:**
```csharp
form.Controls.Add(button);  // Ownership transfer
button.Dispose();           // BAD - form still owns button
// Results in double deletion when form disposes
```

**Use After Disposal:**
```csharp
button.Dispose();
button.Text = "Hello";      // Throws ObjectDisposedException
```

**Forgetting to Dispose:**
```csharp
var button = new Button();  // IsOwned = true
// Never disposed - native object leaked
```
