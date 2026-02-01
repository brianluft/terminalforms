using TerminalForms;

namespace TerminalFormsDemo.Forms;

/// <summary>
/// Demonstrates that forms and their event handlers remain stable when using instance fields.
/// This demo tests the fix for the GC bug where demo objects with instance fields/methods
/// in event handlers could be garbage collected during Application.Run().
/// </summary>
public class FormInstanceFieldDemo : IDemo
{
    // Instance fields - these would cause crashes before the fix if the demo object was GC'd
    private Form _form = null!;
    private Label _label = null!;
    private Button _button = null!;
    private int _clickCount;

    public void Setup()
    {
        _form = new Form { Text = "Instance Field Demo", Bounds = new Rectangle(0, 0, 40, 10) };
        _label = new Label { Bounds = new Rectangle(1, 1, 30, 1), Text = "Clicks: 0" };
        _button = new Button { Bounds = new Rectangle(1, 3, 15, 2), Text = "~C~lick Me" };

        // Use instance method as event handler - captures 'this'
        _button.Click += OnButtonClick;

        _form.Controls.Add(_label);
        _form.Controls.Add(_button);
        _form.Show();

        // After Setup() returns, this demo object is only kept alive through
        // the delegate chain: Form -> Controls -> Button -> Click delegate -> this
        // The form is kept alive by Application.OpenForms.
    }

    private void OnButtonClick(object? sender, EventArgs e)
    {
        // Access instance fields - would crash if 'this' was GC'd
        _clickCount++;
        _label.Text = $"Clicks: {_clickCount}";
    }
}
