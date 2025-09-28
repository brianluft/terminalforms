using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormTextAfterShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new();

        Button button = new() { Text = "~C~lick", Bounds = new(2, 2, 10, 4) };

        button.Click += (s, e) =>
        {
            form.Text = "New Title";
        };

        form.Controls.Add(button);
        form.Show();
    }
}
