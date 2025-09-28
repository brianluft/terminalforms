using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormCloseDemo : IDemo
{
    public void Setup()
    {
        Form form = new();

        Button button = new() { Text = "~C~lick", Bounds = new(2, 2, 10, 4) };

        button.Click += (s, e) =>
        {
            form.Close();
        };

        form.Controls.Add(button);
        form.Show();
    }
}
