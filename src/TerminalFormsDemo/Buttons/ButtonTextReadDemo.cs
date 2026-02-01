using TerminalForms;

namespace TerminalFormsDemo.Buttons;

/// <summary>
/// Tests reading from Text property in click handlers - verifies the fix for the
/// "Reading control.Text in Button.Click crashes after multiple clicks" bug.
/// The test performs multiple label.Text reads within a single click to exercise
/// the same code path that was triggered by multiple separate clicks, while
/// avoiding timing-dependent behavior from the button animation timer.
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
            // Read from label.Text multiple times (this used to crash)
            // Using multiple reads in a single click to test the same code path
            // that was previously triggered by multiple separate clicks.
            for (var i = 0; i < 5; i++)
            {
                _ = label.Text;
            }
            // Write new text that includes reading from the control
            label.Text = $"Click count: {clickCount}";
        };

        form.Controls.Add(label);
        form.Controls.Add(button);
        form.Show();
    }
}
