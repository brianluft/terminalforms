using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class SimpleFormDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        Button button1 = new() { Bounds = new(1, 1, 15, 2) };
        form.Controls.Add(button1);
        form.Show();
    }
}
