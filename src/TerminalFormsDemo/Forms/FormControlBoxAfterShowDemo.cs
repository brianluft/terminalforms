using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormControlBoxAfterShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { ControlBox = true };

        Button button = new() { Text = "~C~lick", Bounds = new(2, 2, 10, 4) };

        button.Click += (s, e) =>
        {
            form.ControlBox = false;
        };

        form.Controls.Add(button);
        form.Show();
    }
}
