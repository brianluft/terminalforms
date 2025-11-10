# Button
We currently have an empty Form showing in our `src\TerminalFormsDemo`. Let's get a `Button` onto that `Form`. Barest minimum to get a button on the screen.

- [x] Implement `public abstract class Control : TerminalFormsObject`.
    - Add constructor that takes `MetaObject`; derived classes will provide their own.
    - No `NativeMethods` needed.
    - *🤖 Created `src\TerminalForms\Control.cs` with abstract class inheriting from `TerminalFormsObject`, added protected constructor for MetaObject, and added additional protected constructor for passing pointer directly which is needed for parameterized constructors.*

- [x] Implement `public unsafe readonly struct ControlCollection : IList<Control>`. This manages the child controls of a C++ `TGroup`. The constructor takes a `void* groupPtr` to manage. Internally it is simply `List<Control>` but it hooks the collection mutation methods/properties to make the corresponding change to the C++ `TGroup`. The C# `ControlCollection` and C++ `TGroup` must be kept in sync: every mutation makes the change first in the `List<T>` and then again on the C++ side via native call. This way we can serve reads directly from the C# side, returning the original C# object that was added to the collection rather than some proxy of the C++ object. Because the C++ side never adds or removes controls on its own, we are in full control of the list from the C# side.
    - Read about `TGroup` in the book, and how it relates to `TWindow` and `TButton`.
    - Write `src\TerminalForms\ControlCollection.cs`, inventing `LibraryImport`s in nested `private static class NativeMethods` for any native functions that you will need. Remember: we mirror writes over to the C++ side, but we serve reads directly from the C# side assuming that we are in sync.
    - Implement those native functions as C++ `TF_EXPORT` functions in `src\tfcore\ControlCollection.cpp`. Header file not needed.
    - *🤖 Changed to `partial class` instead of `struct` to support `LibraryImport`. Created C# implementation with `IList<Control>` interface, maintaining sync between C# `List<Control>` and C++ `TGroup` linked list. Implemented C++ functions for insert, insert-at, and remove-at operations using TGroup's native methods like `insert()`, `insertBefore()`, `remove()`, and `at()`.*

- [x] Implement `ContainerControl`.
    - This abstract C# class derives from `Control` and adds a `ControlCollection` property.
    - Add constructor that takes `MetaObject`; derived classes will provide their own.
    - Update `Form` to derive from `ContainerControl`.
    - No `NativeMethods` needed.
    - *🤖 Created `src\TerminalForms\ContainerControl.cs` as abstract class inheriting from `Control`, added `ControlCollection Controls` property, and updated `Form` to inherit from `ContainerControl` instead of directly from `TerminalFormsObject`.*

- [x] Implement `Button`. This C# class inherits from `Control`.
    - Read about `TButton` in the book.
    - Read `src\tfcore\Form.(cpp|h)`.
    - Read `src\TerminalForms\Form.cs`.
    - Write `src\tfcore\Button.cpp`. Simply expose construction for now, don't even do the button text property yet.
    - Write `src\TerminalForms\Button.cs`.
    - Update `src\TerminalFormsDemo` to add a button to our test form's `Controls` collection.
    - *🤖 Created C++ `tf::Button` class inheriting from `TButton`. Created C# `Button` class. Updated demo to create a button and add it to form's Controls collection.*

- [x] Implement `Button.Text` property.

- [x] Implement `Button.Click` event, `.OnClick` and `.PerformClick` methods.

- [x] Implement button flags.
    - Property `bool IsDefault` sets/unsets `bfDefault`.
    - Top-level enum `ButtonTextAlignment`: `Left`, `Center`
    - Property `ButtonTextAlignment TextAlign` sets/unsets `bfLeftJust` and calls `drawView()`.
    - Property `bool GrabsFocus` sets/unsets `bfGrabFocus`.
    - *🤖 Created `ButtonTextAlignment` enum in C# with `Center=0, Left=1`. Added `IsDefault`, `TextAlign`, and `GrabsFocus` properties to Button class with full XML documentation. Implemented corresponding C++ member functions to safely access protected TButton flags, and TF_EXPORT wrapper functions. All properties properly call `drawView()` when needed to refresh the button display.*
