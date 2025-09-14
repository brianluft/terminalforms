using TerminalForms;

namespace TerminalFormsDemo.CheckBoxes;

public class CheckBoxDisabledClickDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        CheckBox checkBox = new()
        {
            Bounds = new(1, 1, 15, 1),
            Text = "Disabled",
            Enabled = false,
        };
        form.Controls.Add(checkBox);
        form.Show();
    }
}
