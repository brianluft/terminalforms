# Terminal Forms 1.0 Release Plan

This checklist tracks progress toward the 1.0 release of Terminal Forms, a C# TUI library
built on Turbo Vision.

Update documentation as we go:
- `project/docs/` (documentation for contributors)
- XML documentation in public classes becomes user-facing documentation.

---

## Phase 1: Core Control Properties ✓

Complete the Control base class with essential properties that all controls need.

### Control Base Properties
- [x] `Visible` property (maps to `sfVisible` state flag)
- [x] `Enabled` property (maps to `sfDisabled` state flag, inverted)
- [x] `Focused` property (read-only, maps to `sfFocused` state flag)
- [x] `CanFocus` property (maps to `ofSelectable` option flag)
- [x] `TabIndex` property (Z-order position for tab navigation, get-only)
- [x] ~~`TabStop` property~~ (removed - identical to CanFocus in Turbo Vision)
- [x] `Parent` property (read-only, returns owning ContainerControl)
- [x] `Name` property (string identifier for programmatic access)
- [x] `Tag` property (user-defined object data)
- [x] `Focus()` method (programmatically set focus)

### Control Events
- [x] `Enter` event (fires when control gains focus - infrastructure ready)
- [x] `Leave` event (fires when control loses focus - infrastructure ready)
- [x] `EnabledChanged` event
- [x] `VisibleChanged` event

### ContainerControl Features
- [x] `ActiveControl` property (currently focused child)
- [x] `SelectNextControl()` method (programmatic tab navigation)
- [x] Controls collection indexer by Name

---

## Phase 2: Input Controls

Essential controls for user text and option input.

### TextBox (wraps TInputLine)
- [x] C++ binding for TInputLine
- [x] C# `TextBox` class
- [x] `Text` property (get/set current text)
- [x] `MaxLength` property (character limit)
- [ ] `ReadOnly` property (deferred)
- [ ] `PasswordChar` property (mask input - deferred)
- [x] `TextChanged` event
- [ ] `KeyDown`, `KeyUp`, `KeyPress` events (moved to Phase 13)
- [x] Selection support: `SelectionStart`, `SelectionLength`, `SelectedText`
- [x] Methods: `Select()`, `SelectAll()`, `Clear()`
- [x] Demo tests (expected output pending terminal environment)

### Interactive Demo: Calculator App
- [x] Create a full-featured demo: a four-function calculator. A form, textbox for the display, buttons, label for error reporting. Validation on the textbox if the user enters non-numeric input. Test it interactively with tmux and run-demo.sh.
- [x] Refactor Calculator to use instance fields (now safe after OpenForms fix)

### Bugs Found During Calculator Development
- [x] **BUG: Reading control.Text in Button.Click crashes after multiple clicks.** ~~Repro: Create a Button and TextBox/Label. In Button.Click handler, read from `control.Text` (e.g., `label.Text += "X"` or `var x = textBox.Text`). Click the button 3 times - crashes with "Aborted (core dumped)". Writing to Text without reading works fine.~~ **FIXED:** Root cause was string getter functions returning internal pointers that the .NET marshaller would free, corrupting control state. Fix: Use `TF_STRDUP` to allocate copies in C++ that the marshaller can safely free. See `ButtonTextReadDemo` for a test that verifies the fix.
- [x] **BUG: Demo objects with instance fields/methods in event handlers may be GC'd.** ~~If a demo class uses instance fields (e.g., `private TextBox _display`) and instance methods in lambdas (e.g., `btn.Click += (_, _) => OnDigitClick()`), the demo object can be garbage collected during `Application.Run()` since nothing holds a reference to it after `Setup()` returns. This causes crashes when event handlers try to access `this`.~~ **FIXED:** Added `Application.OpenForms` collection that keeps strong references to shown forms. Forms are added to `OpenForms` when `Show()` is called and removed when closed (via the new `Form.Closed` event). This creates a strong reference chain (OpenForms -> Form -> Controls -> Button -> Click delegate -> user object) that prevents GC of objects referenced by event handlers. See `FormInstanceFieldDemo` for a test that uses instance fields in event handlers.

### RadioButton (wraps TRadioButtons)
- [ ] C++ binding for TRadioButtons cluster
- [ ] C# `RadioButton` class
- [ ] `Text` property
- [ ] `Checked` property (only one in group can be true)
- [ ] `CheckedChanged` event
- [ ] Automatic mutual exclusion within container
- [ ] Demo tests

### NumericUpDown (TInputLine + validation)
- [ ] C# `NumericUpDown` class
- [ ] `Value` property (decimal)
- [ ] `Minimum`, `Maximum` properties
- [ ] `Increment` property
- [ ] `DecimalPlaces` property
- [ ] `ValueChanged` event
- [ ] Up/Down arrow key support
- [ ] Demo tests

---

## Phase 3: Selection Controls

Controls for selecting from lists of items.

### ListBox (wraps TListBox)
- [ ] C++ binding for TListBox
- [ ] C# `ListBox` class
- [ ] `Items` collection property (ObjectCollection)
- [ ] `SelectedIndex` property
- [ ] `SelectedItem` property
- [ ] `SelectedIndexChanged` event
- [ ] `SelectionMode` property (One, MultiSimple, MultiExtended)
- [ ] Multi-selection support: `SelectedIndices`, `SelectedItems`
- [ ] Methods: `ClearSelected()`, `SetSelected()`
- [ ] Demo tests

### ComboBox (TInputLine + THistory)
- [ ] C++ binding for THistory
- [ ] C# `ComboBox` class
- [ ] `Items` collection property
- [ ] `Text` property (current value)
- [ ] `SelectedIndex`, `SelectedItem` properties
- [ ] `DropDownStyle` property (DropDown, DropDownList)
- [ ] `SelectedIndexChanged` event
- [ ] `TextChanged` event (for editable mode)
- [ ] Demo tests

### CheckedListBox (TCheckBoxes cluster with list presentation)
- [ ] C# `CheckedListBox` class
- [ ] `Items` collection with CheckState
- [ ] `CheckedIndices`, `CheckedItems` properties
- [ ] `GetItemChecked()`, `SetItemChecked()` methods
- [ ] `ItemCheck` event
- [ ] Demo tests

---

## Phase 4: Container Controls

Controls that group and organize other controls.

### GroupBox (TGroup + frame)
- [ ] C++ binding for framed TGroup
- [ ] C# `GroupBox` class
- [ ] `Text` property (title in frame)
- [ ] Inherits Controls collection from ContainerControl
- [ ] Demo tests

### Panel (invisible TGroup)
- [ ] C++ binding for borderless TGroup
- [ ] C# `Panel` class
- [ ] `BorderStyle` property (None, FixedSingle)
- [ ] Inherits Controls collection from ContainerControl
- [ ] Demo tests

### TabControl
- [ ] C++ binding for tabbed container (custom TGroup-based view)
- [ ] C# `TabControl` class
- [ ] `TabPages` collection (TabPageCollection)
- [ ] `SelectedIndex`, `SelectedTab` properties
- [ ] `SelectedIndexChanged` event
- [ ] C# `TabPage` class (ContainerControl with Text property)
- [ ] Demo tests

---

## Phase 5: Scrolling Support

Enable viewing content larger than the viewport.

### ScrollBar (wraps TScrollBar)
- [ ] C++ binding for TScrollBar
- [ ] C# `ScrollBar` abstract base class
- [ ] C# `HScrollBar` class (horizontal)
- [ ] C# `VScrollBar` class (vertical)
- [ ] `Value` property (current position)
- [ ] `Minimum`, `Maximum` properties
- [ ] `SmallChange` property (arrow click increment)
- [ ] `LargeChange` property (page increment)
- [ ] `Scroll` event
- [ ] `ValueChanged` event
- [ ] Demo tests

### ScrollableControl base class
- [ ] `AutoScroll` property
- [ ] `AutoScrollMinSize` property
- [ ] `HorizontalScroll`, `VerticalScroll` properties
- [ ] Automatic scrollbar management

---

## Phase 6: Menu System

Application menu bar and context menus.

### MenuStrip (wraps TMenuBar)
- [ ] C++ binding for TMenuBar
- [ ] C++ binding for TMenuBox
- [ ] C# `MenuStrip` class
- [ ] `Items` collection (ToolStripItemCollection)
- [ ] Keyboard shortcut handling (Alt+letter)
- [ ] Demo tests

### ToolStripMenuItem
- [ ] C# `ToolStripMenuItem` class
- [ ] `Text` property (with & for accelerator)
- [ ] `ShortcutKeys` property
- [ ] `Checked` property (checkable menu items)
- [ ] `DropDownItems` collection (submenus)
- [ ] `Click` event
- [ ] `Enabled` property
- [ ] Demo tests

### ContextMenuStrip
- [ ] C++ binding for popup TMenuBox
- [ ] C# `ContextMenuStrip` class
- [ ] `Show()` method at location
- [ ] Control `ContextMenuStrip` property
- [ ] Demo tests

---

## Phase 7: Status Bar

Application status bar at bottom of screen.

### StatusStrip (wraps TStatusLine)
- [ ] C++ binding for TStatusLine
- [ ] C++ binding for TStatusDef and TStatusItem
- [ ] C# `StatusStrip` class
- [ ] `Items` collection
- [ ] Demo tests

### ToolStripStatusLabel
- [ ] C# `ToolStripStatusLabel` class
- [ ] `Text` property
- [ ] `Spring` property (auto-fill remaining space)
- [ ] Demo tests

---

## Phase 8: Dialog System

Standard dialogs and dialog infrastructure.

### DialogResult enum
- [ ] C# `DialogResult` enum (None, OK, Cancel, Abort, Retry, Ignore, Yes, No)
- [ ] Form `DialogResult` property
- [ ] Form `AcceptButton` property (Enter key)
- [ ] Form `CancelButton` property (Escape key)
- [ ] Modal form support with `ShowDialog()` returning DialogResult

### MessageBox
- [ ] C# `MessageBox` static class
- [ ] `Show()` method overloads
- [ ] `MessageBoxButtons` enum (OK, OKCancel, YesNo, YesNoCancel, etc.)
- [ ] `MessageBoxIcon` enum (None, Error, Warning, Information, Question)
- [ ] `MessageBoxDefaultButton` enum
- [ ] Demo tests

### OpenFileDialog (wraps TFileDialog)
- [ ] C++ binding for TFileDialog
- [ ] C# `FileDialog` abstract base class
- [ ] C# `OpenFileDialog` class
- [ ] `Filter` property (file type filters)
- [ ] `FilterIndex` property
- [ ] `InitialDirectory` property
- [ ] `FileName` property
- [ ] `FileNames` property (multi-select)
- [ ] `Multiselect` property
- [ ] `Title` property
- [ ] `ShowDialog()` method
- [ ] Demo tests

### SaveFileDialog
- [ ] C# `SaveFileDialog` class
- [ ] `OverwritePrompt` property
- [ ] `CreatePrompt` property
- [ ] Demo tests

### FolderBrowserDialog (wraps TChDirDialog)
- [ ] C++ binding for TChDirDialog
- [ ] C# `FolderBrowserDialog` class
- [ ] `SelectedPath` property
- [ ] `Description` property
- [ ] `ShowDialog()` method
- [ ] Demo tests

---

## Phase 9: Input Validation

Validation framework for input controls.

### Validation Events
- [ ] `Validating` event on Control (CancelEventArgs)
- [ ] `Validated` event on Control
- [ ] Automatic validation on focus change
- [ ] `CausesValidation` property on Control

### ErrorProvider
- [ ] C# `ErrorProvider` component
- [ ] `SetError()` method
- [ ] `GetError()` method
- [ ] `Clear()` method
- [ ] Visual error indicator (character marker)
- [ ] Demo tests

### Validators (optional, Turbo Vision style)
- [ ] C++ binding for TValidator base
- [ ] C++ binding for TFilterValidator
- [ ] C++ binding for TRangeValidator
- [ ] C# validator wrapper classes
- [ ] TextBox `Validator` property

---

## Phase 10: Layout and Anchoring

Automatic control positioning and sizing.

### Anchor property
- [ ] C# `AnchorStyles` flags enum (Top, Bottom, Left, Right)
- [ ] Control `Anchor` property
- [ ] Maps to Turbo Vision `growMode` flags
- [ ] Automatic repositioning on parent resize
- [ ] Demo tests

### Dock property
- [ ] C# `DockStyle` enum (None, Top, Bottom, Left, Right, Fill)
- [ ] Control `Dock` property
- [ ] Automatic sizing to fill parent edges
- [ ] Demo tests

### Layout events
- [ ] `Layout` event on Control
- [ ] `Resize` event on Control
- [ ] `SizeChanged` event on Control
- [ ] `LocationChanged` event on Control

---

## Phase 11: Multi-line Text Editing

Advanced text editing support.

### RichTextBox / TextBox multiline (wraps TEditor/TMemo)
- [ ] C++ binding for TMemo
- [ ] C++ binding for TEditor
- [ ] C# `TextBox` multiline mode OR `RichTextBox` class
- [ ] `Multiline` property
- [ ] `Lines` property (string array)
- [ ] `WordWrap` property
- [ ] `ScrollBars` property
- [ ] Selection support
- [ ] Undo/Redo support: `CanUndo`, `Undo()`, `Redo()`
- [ ] Clipboard support: `Cut()`, `Copy()`, `Paste()`
- [ ] `Modified` property
- [ ] Demo tests

---

## Phase 12: Tree View

Hierarchical data display.

### TreeView (wraps TOutline)
- [ ] C++ binding for TOutline
- [ ] C# `TreeView` class
- [ ] `Nodes` collection (TreeNodeCollection)
- [ ] `SelectedNode` property
- [ ] `AfterSelect` event
- [ ] `BeforeExpand`, `AfterExpand` events
- [ ] `BeforeCollapse`, `AfterCollapse` events
- [ ] Demo tests

### TreeNode
- [ ] C# `TreeNode` class
- [ ] `Text` property
- [ ] `Nodes` collection (child nodes)
- [ ] `Parent` property
- [ ] `TreeView` property
- [ ] `Expand()`, `Collapse()` methods
- [ ] `IsExpanded` property
- [ ] `Tag` property

---

## Phase 13: Keyboard Handling

Comprehensive keyboard input support.

### Key Events
- [ ] `KeyDown` event on Control (KeyEventArgs)
- [ ] `KeyUp` event on Control (KeyEventArgs)
- [ ] `KeyPress` event on Control (KeyPressEventArgs)
- [ ] `PreviewKeyDown` event for early interception

### Keys enum
- [ ] C# `Keys` enum (all key codes)
- [ ] Modifier detection: Shift, Control, Alt
- [ ] `KeyEventArgs` class with KeyCode, KeyData, Modifiers

### Shortcuts
- [ ] Form `KeyPreview` property (see keys before controls)
- [ ] Global shortcut handling in Application

---

## Phase 14: Mouse Handling

Mouse input support.

### Mouse Events
- [ ] `MouseDown` event on Control (MouseEventArgs)
- [ ] `MouseUp` event on Control
- [ ] `MouseMove` event on Control
- [ ] `MouseClick` event on Control
- [ ] `MouseDoubleClick` event on Control
- [ ] `MouseEnter`, `MouseLeave` events

### MouseEventArgs
- [ ] C# `MouseEventArgs` class
- [ ] `Button` property (MouseButtons enum)
- [ ] `Clicks` property (click count)
- [ ] `Location` property (Point)
- [ ] `X`, `Y` properties

### Cursor Support
- [ ] Control `Cursor` property
- [ ] `Cursors` static class (arrow, text, wait, etc. - limited in TUI)

---

## Phase 15: Application Features

Application-level infrastructure.

### Application Events
- [ ] `Application.Idle` event (background processing)
- [ ] `Application.ApplicationExit` event
- [ ] `Application.ThreadException` event

### Application Methods
- [ ] `Application.Exit()` method
- [ ] `Application.DoEvents()` method (process pending events)

### Clipboard
- [ ] C# `Clipboard` static class (Turbo Vision has clipboard editor)
- [ ] `SetText()`, `GetText()` methods
- [ ] `ContainsText()` method

---

## Phase 16: Color and Theming

Visual customization support.

### Color Properties
- [ ] Control `ForeColor` property
- [ ] Control `BackColor` property
- [ ] Map to Turbo Vision palette system
- [ ] Form palette selection (Blue, Gray, Cyan windows)

### Theme Support
- [ ] `Application.Theme` property or similar
- [ ] Predefined themes matching Turbo Vision palettes
- [ ] Custom palette support

---

## Phase 17: Documentation and Polish

Final preparation for 1.0 release.

### API Documentation
- [ ] XML doc comments on all public members
- [ ] IntelliSense-friendly descriptions
- [ ] Usage examples in remarks

### User Guide
- [ ] Getting started guide
- [ ] Control reference
- [ ] Common patterns and recipes
- [ ] Migration guide from Windows Forms

### Samples
- [ ] Hello World sample
- [ ] Form with controls sample
- [ ] Dialog sample
- [ ] Menu and status bar sample
- [ ] File browser sample
- [ ] Text editor sample

### Testing
- [ ] Demo tests for all controls
- [ ] Unit tests for value types
- [ ] Integration tests for common scenarios

### NuGet Package
- [ ] Package metadata
- [ ] Native library bundling (Windows, Linux, macOS)
- [ ] README for NuGet gallery
- [ ] License file

### Release
- [ ] Version 1.0.0
- [ ] GitHub release with release notes
- [ ] NuGet publish

---

## Stretch Goals (Post-1.0)

Features that would be nice but not essential for 1.0.

### Form.FormClosing Event (Cancellable)
- [ ] Add `FormClosing` event that fires before `Closed`, with `CancelEventArgs`
- [ ] Allow event handlers to cancel the close by setting `e.Cancel = true`
- [ ] Implement via `handleEvent()` override to intercept `cmClose` command before processing
- [ ] Matches Windows Forms pattern where `FormClosing` precedes `FormClosed`

### Unify Event Semantics Across Controls
- [ ] Update CheckBox to fire `CheckedChanged` when `Checked` property is set programmatically
- [ ] Update Button (if any state properties exist) to fire events on programmatic changes
- [ ] Document the Windows Forms-style event behavior: events fire for BOTH user input and programmatic changes
- [ ] This ensures consistent behavior across all controls

### Focus Event Callbacks via C++ setState Override
- [ ] Create `tf::View` base class that overrides `TView::setState()`
- [ ] Centralized detection of sfFocused changes for Enter/Leave events
- [ ] Centralized detection of sfVisible/sfDisabled changes
- [ ] Would enable callback-based events instead of polling-based detection
- [ ] Foundation for future keyboard/mouse events via `handleEvent()` override

### Data Binding
- [ ] `IBindableComponent` interface
- [ ] `DataBindings` collection
- [ ] `BindingSource` component
- [ ] `INotifyPropertyChanged` support

### Printing
- [ ] Print support (limited use in TUI)

### Drag and Drop
- [ ] `AllowDrop` property
- [ ] `DragEnter`, `DragDrop` events
- [ ] `DoDragDrop()` method

### ToolTip
- [ ] `ToolTip` component
- [ ] Control `ToolTipText` property

### Timer
- [ ] C# `Timer` component
- [ ] `Interval` property
- [ ] `Tick` event
- [ ] `Start()`, `Stop()` methods

### ListView (complex grid)
- [ ] Multi-column list with headers
- [ ] `Details`, `List`, `LargeIcon`, `SmallIcon` views (limited in TUI)
- [ ] Column sorting

### History/Autocomplete
- [ ] Input history support
- [ ] Autocomplete suggestions
- [ ] `AutoCompleteMode` property on TextBox

### Resources/Localization
- [ ] Resource file support
- [ ] String table support
- [ ] Culture-aware formatting

---

## Implementation Priority Notes

**High Priority (Core Usability):**
- Phases 1-3: Base properties, TextBox, RadioButton, ListBox
- Phase 8: MessageBox and DialogResult (essential for user interaction)

**Medium Priority (Full Application Support):**
- Phases 4-7: Containers, scrolling, menus, status bar
- Phase 9: Validation

**Lower Priority (Polish):**
- Phases 10-16: Layout, editors, trees, advanced events, theming
- Phase 17: Documentation and release

This ordering allows building useful applications early while progressively adding
more sophisticated features.
