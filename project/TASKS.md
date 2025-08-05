We are doing vertical slices of the bindings: one specific C++ class at a time.

# Example 1
- `src\TerminalForms\KeyDownEvent.cs`
- `src\TerminalForms\NativeMethods.KeyDownEvent.cs`
- `src\tvision4c\KeyDownEvent.h`
- `src\tvision4c\KeyDownEvent.cpp`
- `build\prefix\include\tvision\system.h` (lines 178-216)

# Example 2
- `src\TerminalForms\Rect.cs`
- `src\TerminalForms\NativeMethods.Rect.cs`
- `src\tvision4c\Rect.h`
- `src\tvision4c\Rect.cpp`
- `build\prefix\include\tvision\objects.h` (lines 96-200)

# General procedure to write bindings for underlying class "Foo"
- Find the .h and .cpp files for the C++ wrapper class in `src\tvision4c\Foo.[h|cpp]`
- Find the underlying tvision C++ class in `build\prefix\include\tvision\*.h`, grep for it.
- Read the first example above to understand the pattern.
- Write the C# code: the corresponding class in `src\TerminalForms\Foo.cs` and, if it's not already present, the `[LibraryImport]` declarations in `src\TerminalForms\NativeMethods.Foo.cs`. You can modify existing `LibraryImport` declarations to make it easier; in particular, changing IntPtr to `byte*` or `Span<byte>`. In C#, `unsafe` is actually better.
- If any part seems unclear, read example 2 to see if it clears it up. If you're still not sure, put a big comment on that part so I can address it. Mention it in your summary in TASKS.md.

# Bindings tasks
Remember to do only one at a time (in order) then commit and stop.
- [x] Event
  - *🤖 Created `src/TerminalForms/Event.cs` following the established pattern with disposable wrapper, properties for What, Mouse (IntPtr), KeyDown, Message (IntPtr), and methods GetMouseEvent/GetKeyEvent. Used IntPtr for Mouse and Message properties since those classes don't exist yet - they will need to be replaced with proper types when MouseEventType and MessageEvent are implemented.*
- [x] KeyDownEvent
  - *🤖 KeyDownEvent was already fully implemented with complete C# wrapper class including disposable pattern, IEquatable implementation, properties for KeyCode/CharCode/ScanCode/ControlKeyState/Text, and all necessary native method declarations with proper marshaling. The implementation follows established patterns and builds successfully.*
- [x] MessageEvent
  - *🤖 Created `src/TerminalForms/MessageEvent.cs` following the established pattern with disposable wrapper, IEquatable implementation, properties for Command (ushort) and InfoPtr (IntPtr), and all necessary native method calls. The NativeMethods.MessageEvent.cs file was already present with correct P/Invoke declarations. Implementation follows the same pattern as KeyDownEvent.*
- [x] MouseEventType
  - *🤖 Created `src/TerminalForms/MouseEventType.cs` following the established pattern with disposable wrapper, IEquatable implementation, properties for Where (Point), EventFlags (ushort), ControlKeyState (ushort), Buttons (byte), and Wheel (byte). The NativeMethods.MouseEventType.cs file was already present with correct P/Invoke declarations. Implementation follows the same pattern as KeyDownEvent and MessageEvent with proper error checking and disposal handling.*
- [x] Go back to `src/TerminalForms/Event.cs` and update it now that we've implemented `KeyDownEvent`/`MessageEvent`/`MouseEventType`.
  - *🤖 Updated the Mouse property to return `MouseEventType` instead of `IntPtr`, and the Message property to return `MessageEvent` instead of `IntPtr`. Both properties now instantiate the proper wrapper classes from the returned native pointers, and the setters use the `.Ptr` property of the passed objects. The KeyDown property was already correctly implemented. All changes maintain the established disposable pattern and error checking.*
