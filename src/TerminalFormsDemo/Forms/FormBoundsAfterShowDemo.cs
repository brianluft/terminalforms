using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormBoundsAfterShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new();

        Button button = new() { Text = "~C~lick", Bounds = new(2, 2, 10, 4) };

        button.Click += (s, e) =>
        {
            form.Bounds = new(10, 5, 30, 12);
        };

        form.Controls.Add(button);
        form.Show();
    }
}
