using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormMaximizeBoxAfterShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { MaximizeBox = false };

        Button button = new() { Text = "~C~lick", Bounds = new(2, 2, 10, 4) };

        button.Click += (s, e) =>
        {
            form.MaximizeBox = true;
        };

        form.Controls.Add(button);
        form.Show();
    }
}
