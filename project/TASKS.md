# Label class

## Overview

The Label control provides both static text display and hotkey functionality for associated controls. Unlike tvision's approach which distinguishes between TStaticText (static text) and TLabel (paired with controls), we follow the Windows Forms model where a single Label class handles both use cases transparently.

### Architecture Decision

**Inherit from TLabel with nullptr link + override handleEvent for Windows Forms behavior**

- **TLabel base**: Provides hotkey parsing (~A~ syntax), hotkey coloring, and drawing logic
- **nullptr link**: TLabel safely handles null links, disabling explicit association
- **Override handleEvent**: Replace `link->focus()` with `selectNext(false)` for Windows Forms semantics
- **UseMnemonic property**: Controls whether hotkey processing is enabled (like Windows Forms)

This approach gives us all TLabel functionality (parsing, coloring, events) while providing Windows Forms-compatible behavior where hotkeys move focus to the next control in tab order.

## Tasks

### C++ Implementation (tfcore)

- [x] **Create Label.h header**
  - Inherit from TLabel with nullptr link constructor
  - Add setUseMnemonic() method for enabling/disabling hotkeys
  - Implement proper tf::equals and std::hash specializations
  - *🤖 Created Label.h with TLabel inheritance, proper specializations for tf::equals and std::hash, and public methods for text and mnemonic access.*

- [x] **Create Label.cpp implementation** 
  - Constructor: `Label(bounds, text)` → `TLabel(bounds, text, nullptr)`
  - Override handleEvent() for Windows Forms behavior:
    - Call TStaticText::handleEvent() first
    - On hotkey/mouse: use `owner->selectNext(false)` instead of `link->focus()`
    - Skip TLabel's broadcast event handling (no link feedback)
  - Implement setUseMnemonic() to toggle ofPreProcess/ofPostProcess flags
  - Add TF_DEFAULT_CONSTRUCTOR and TF_BOILERPLATE_FUNCTIONS macros
  - *🤖 Implemented Label.cpp with Windows Forms navigation behavior using owner->selectNext(false), proper UseMnemonic flag handling, and public text access methods to avoid protected member access issues.*

- [x] **Export C interface functions**
  - TfLabelNew, TfLabelDelete, TfLabelEquals, TfLabelHash
  - TfLabelGetText, TfLabelSetText (inherit from TStaticText)
  - TfLabelGetUseMnemonic, TfLabelSetUseMnemonic
  - *🤖 Exported all required C interface functions with proper error handling and parameter validation.*

- [x] **Update CMakeLists.txt**
  - Add Label.cpp to the tfcore library source list
  - *🤖 Added Label.cpp to the CMakeLists.txt source list in alphabetical order.*

### C# Implementation (TerminalForms)

- [x] **Create Label.cs**
  - Inherit from Control (not ContainerControl)
  - Standard MetaObject pattern with NativeMethods
  - Text property with UTF8 string marshalling
  - UseMnemonic property (bool, default true)
  - No explicit AssociatedControl property needed
  - Full XML documentation explaining Windows Forms compatibility
  - *🤖 Created Label.cs following established patterns with comprehensive XML documentation explaining Windows Forms compatibility and hotkey navigation behavior.*

- [x] **Follow established patterns**
  - Dispose pattern with ObjectDisposedException.ThrowIf
  - Check() error handling wrapper
  - Standard NativeMethods inner class with LibraryImport
  - Proper bool marshalling with [MarshalAs(UnmanagedType.I4)]
  - *🤖 Implemented all established patterns consistently with other controls, including proper error handling and marshalling.*

### Demo and Testing

- [x] **Create demo classes in TerminalFormsDemo/Labels/**
  - BasicLabelDemo: Simple text display
  - HotkeyLabelDemo: Labels with Alt+key shortcuts
  - UseMnemonicDemo: Toggle hotkey processing on/off
  - ComplexLayoutDemo: Labels mixed with various controls
  - For all demos: when "the right thing" happens, make a unique string appear on-screen somehow. Then you can use `grep` later to see whether the string appears in the output file. Don't trust your "eyes"--ASCII art is difficult for you to read.
  - *🤖 Created all four demo classes with unique success strings for grep testing. Fixed CheckBox event handling to use CheckedChanged instead of Click.*

- [ ] **Add unit tests in Tests project**
  - Text property get/set
  - UseMnemonic property behavior
  - Basic construction and disposal
  - Integration with forms and other controls

- [ ] **Generate expected output files**
  - Run `NO_TESTS=1 scripts/build.sh` 
  - Run `scripts/run-demo.sh --test LabelDemo --output <file>`
  - Use `grep` to verify that the expected string appears in the output file, as you cannot directly interpret the ASCII screenshot.

### User Documentation

- [ ] Write `src\website\guide\labels.md` and update `src\website\guide\toc.yml`. This will be a short article explaining how to use labels in their various modes. You can paste demo output files into code blocks as screenshots.
