using TerminalForms;

namespace TerminalFormsDemo.CheckBoxes;

public class CheckBoxLayoutDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        CheckBox checkBox = new() { Bounds = new(1, 1, 10, 1), Text = "~T~est" };
        form.Controls.Add(checkBox);
        form.Show();
    }
}
