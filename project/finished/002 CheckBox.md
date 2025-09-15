# CheckBox ✅ COMPLETED
We will implement a CheckBox control. Turbo Vision's `TCheckBoxes` is a cluster of multiple checkboxes. We will always have a single item so that our C# CheckBox class mimics WinForms. Use `Button` as an example.

## C++ Core Implementation

- [x] Create `src/tfcore/CheckBox.h` header file
  - [x] Declare `tf::CheckBox` class inheriting from `TCheckBoxes`
  - [x] Add constructor that creates single-item TSItem chain
  - [x] Add getter/setter methods for checked state, text, enabled state
  - [x] Add event handler support for state changes
  - [x] Add template specializations for `tf::equals<CheckBox>` and `std::hash<tf::CheckBox>`
  - *🤖 Implemented CheckBox class inheriting from TCheckBoxes with single TSItem, following Button pattern. Used protected member access within class methods for string manipulation.*

- [x] Create `src/tfcore/CheckBox.cpp` implementation file
  - [x] Implement constructor that creates TSItem with single checkbox
  - [x] Override `press()` method to fire C# event callbacks
  - [x] Implement property getters/setters
  - [x] Add `TF_DEFAULT_CONSTRUCTOR(CheckBox)`
  - [x] Add `TF_BOILERPLATE_FUNCTIONS(CheckBox)`
  - [x] Create TF_EXPORT functions following naming conventions:
    - [x] `TfCheckBoxSetText(CheckBox* self, const char* text)`
    - [x] `TfCheckBoxGetText(CheckBox* self, const char** out)`
    - [x] `TfCheckBoxSetChecked(CheckBox* self, BOOL value)`
    - [x] `TfCheckBoxGetChecked(CheckBox* self, BOOL* out)`
    - [x] `TfCheckBoxSetEnabled(CheckBox* self, BOOL value)`
    - [x] `TfCheckBoxGetEnabled(CheckBox* self, BOOL* out)`
    - [x] `TfCheckBoxSetStateChangedEventHandler(CheckBox* self, EventHandlerFunction function, void* userData)`
  - *🤖 Implemented all required TF_EXPORT functions. Used proper string collection manipulation for text handling and bit manipulation for checked/enabled states.*

- [x] Update `src/tfcore/CMakeLists.txt` to include `CheckBox.cpp`
  - *🤖 Added CheckBox.cpp to the CMakeLists.txt build configuration.*

## C# Wrapper Implementation

- [x] Create `src/TerminalForms/CheckBox.cs`
  - [x] Create `CheckBox` class inheriting from `Control`
  - [x] Add `MetaObject` initialization following Button pattern
  - [x] Implement constructor with event handler setup
  - [x] Add properties:
    - [x] `Text` (string) - checkbox label text
    - [x] `Checked` (bool) - checked state
    - [x] `Enabled` (bool) - enabled/disabled state
  - [x] Add `CheckedChanged` event following Button.Click pattern
  - [x] Add `OnCheckedChanged()` method
  - [x] Create `NativeMethods` partial class with all P/Invoke declarations:
    - [x] `TfCheckBoxNew`, `TfCheckBoxDelete`, `TfCheckBoxEquals`, `TfCheckBoxHash`
    - [x] `TfCheckBoxGetText`, `TfCheckBoxSetText`
    - [x] `TfCheckBoxGetChecked`, `TfCheckBoxSetChecked`
    - [x] `TfCheckBoxGetEnabled`, `TfCheckBoxSetEnabled`
    - [x] `TfCheckBoxSetStateChangedEventHandler`
  - *🤖 Implemented complete C# wrapper with comprehensive XML documentation, following the Button pattern exactly. All P/Invoke declarations use proper marshalling attributes.*

## Demo Implementation

- [x] Create `src/TerminalFormsDemo/CheckBoxes/` directory

- [x] Create basic demos following existing Button patterns:
  - [x] `CheckBoxLayoutDemo.cs` - just a checkbox, no interaction
  - [x] `CheckBoxClickDemo.cs` - checkbox and button, clicking the checkbox changes the button text to "Clicked!", demonstrate clicking the checkbox
  - [x] `CheckBoxTextDemo.cs` - checkbox and button, clicking the button changes the checkbox text to "Clicked!", demonstrate clicking the button
  - [x] `CheckBoxDisabledClickDemo.cs` - just a checkbox with Enabled=false, demonstrate that clicking on a disabled checkbox does not check it
  - [x] `CheckBoxCheckedDemo.cs` - checkbox and button, clicking the button changes the checkbox's Checked to true, demonstrate clicking the button
  - *🤖 Created all demo classes following the current IDemo interface (Setup() method). Updated to use proper bounds and control layout.*

- [x] Create demo input files for interactive tests:
  - [x] `CheckBoxClickDemo-input.txt` - Space key to toggle checkbox
  - [x] `CheckBoxTextDemo-input.txt` - Return key to click button  
  - [x] `CheckBoxDisabledClickDemo-input.txt` - Space key on disabled checkbox
  - [x] `CheckBoxCheckedDemo-input.txt` - Return key to click button
  - *🤖 Used proper KEYDOWN event format instead of mouse events, following existing Button demo patterns.*

- [x] Create demo output files. You cannot write these yourself as they are terminal screenshots; you must `NO_TESTS=1 scripts/build.sh` then `scripts/run-demo.sh --test FooDemo --output <txt file path>` and it will write the file. I will manually verify correctness.
  - [x] `CheckBoxLayoutDemo-output.txt`
  - [x] `CheckBoxClickDemo-output.txt`
  - [x] `CheckBoxTextDemo-output.txt`
  - [x] `CheckBoxDisabledClickDemo-output.txt`
  - [x] `CheckBoxCheckedDemo-output.txt`
  - *🤖 Generated all output files using the demo screenshot system. All demos run successfully and generate proper terminal output.*

## Documentation and Polish

- [x] Add XML documentation comments to all public members in `CheckBox.cs`
- [x] Add usage examples in XML comments
  - *🤖 Added comprehensive XML documentation covering all properties and methods, with detailed explanations of usage patterns and behavior.*
