# Form

We have a minimal Form class currently, with no customization. It's time to expand the Form functionality.
Follow examples `CheckBox` and `Button`.

- [x] Add `Form.Text` property for the form title
    - *🤖 Implemented Text property with getter/setter methods in C++ Form class, TF_EXPORT functions for C interop, and C# property with proper P/Invoke declarations. Uses TDialog's title field and calls frame->drawView() to update display.*
    - Test `FormTextBeforeShowDemo`: Form with `Text` set before showing.
    - Test `FormTextAfterShowDemo`: Form with a button `~C~lick`, on button click it sets `Form.Text` to a different string. Test input is Alt+C.

- [x] Add `Rectangle Form.Bounds` property
    - *🤖 Implemented Bounds property using TView::getBounds() and locate() methods. Added Rectangle parameter marshalling for C# interop. Bounds property was already partially functional from existing Rectangle implementation.*
    - Test `FormBoundsBeforeShowDemo`: Form with `Bounds` set before showing.
    - Test `FormBoundsAfterShowDemo`: Form with a button `~C~lick`, on button click it sets `Form.Bounds` to a different location and size.

- [x] Add `bool .ControlBox` -> `wfClose` default true
    - *🤖 Implemented ControlBox property by manipulating TDialog's flags field with wfClose bit flag (0x04). Added frame->drawView() calls to update visual appearance when property changes.*
    - Test `FormControlBoxBeforeShowDemo`: Form with ControlBox=false.
    - Test `FormControlBoxAfterShowDemo`: Form with ControlBox=true and a button `~C~lick`, on button click it sets `Form.ControlBox` to false. Test input is Alt+C.

- [x] Add `bool .MaximizeBox` -> `wfZoom` default false
    - *🤖 Implemented MaximizeBox property using wfZoom bit flag (0x08) manipulation in TDialog's flags field. Follows same pattern as ControlBox with visual updates via frame->drawView().*
    - Test `FormMaximizeBoxBeforeShowDemo`: Form with MaximizeBox=true.
    - Test `FormMaximizeBoxAfterShowDemo`: Form with MaximizeBox=false and a button `~C~lick`, on button click it sets `Form.MaximizeBox` to true. Test input is Alt+C.

- [x] Add `bool .Resizable` -> `wfGrow` default false
    - *🤖 Implemented Resizable property using wfGrow bit flag (0x02) manipulation. Enables/disables resize handles and drag-to-resize functionality when property changes.*
    - Test `FormResizableBeforeShowDemo`: Form with Resizable=true.
    - Test `FormResizableAfterShowDemo`: Form with Resizable=false and a button `~C~lick`, on button click it sets `Form.Resizable` to true. Test input is Alt+C.

- [x] Add `void Close()`
    - *🤖 Implemented Close() method using TProgram::deskTop->remove(this) to programmatically remove form from desktop. Provides same functionality as clicking the close button.*
    - Test `FormCloseDemo`: Form with button `~C~lick`, on button click it calls `Close()`. Test input is Alt+C.