using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormResizableAfterShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Resizable = false };

        Button button = new() { Text = "~C~lick", Bounds = new(2, 2, 10, 4) };

        button.Click += (s, e) =>
        {
            form.Resizable = true;
        };

        form.Controls.Add(button);
        form.Show();
    }
}
