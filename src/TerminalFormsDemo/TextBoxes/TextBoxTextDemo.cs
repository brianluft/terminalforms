using TerminalForms;

namespace TerminalFormsDemo.TextBoxes;

public class TextBoxTextDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        TextBox textBox = new() { Bounds = new(1, 1, 20, 1), Text = "Hello World" };
        Label label = new() { Bounds = new(1, 3, 20, 1), Text = "Initial text above" };

        form.Controls.Add(textBox);
        form.Controls.Add(label);
        form.Show();
    }
}
