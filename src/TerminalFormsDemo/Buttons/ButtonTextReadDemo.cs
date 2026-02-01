using TerminalForms;

namespace TerminalFormsDemo.Buttons;

/// <summary>
/// Tests reading from Text property in click handlers - verifies the fix for the
/// "Reading control.Text in Button.Click crashes after multiple clicks" bug.
/// </summary>
public class ButtonTextReadDemo : IDemo
{
    public void Setup()
    {
        var form = new Form { Bounds = new Rectangle(0, 0, 40, 8) };
        var label = new Label { Bounds = new Rectangle(1, 1, 35, 1), Text = "Click count: 0" };
        var button = new Button { Bounds = new Rectangle(1, 3, 15, 2), Text = "Click Me" };

        var clickCount = 0;
        button.Click += (sender, e) =>
        {
            clickCount++;
            // Read from label.Text (this used to crash after multiple clicks)
            var currentText = label.Text;
            // Write new text that includes reading from the control
            label.Text = $"Click count: {clickCount}";
        };

        form.Controls.Add(label);
        form.Controls.Add(button);
        form.Show();
    }
}
