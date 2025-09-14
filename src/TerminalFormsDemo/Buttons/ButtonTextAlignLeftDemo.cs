using TerminalForms;

namespace TerminalFormsDemo.Buttons;

public class ButtonTextAlignLeftDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        Button button1 = new() { Bounds = new(1, 1, 15, 2), TextAlign = ButtonTextAlignment.Left };
        form.Controls.Add(button1);
        form.Show();
    }
}
