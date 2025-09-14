using TerminalForms;

namespace TerminalFormsDemo.CheckBoxes;

public class CheckBoxCheckedDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        CheckBox checkBox = new() { Bounds = new(1, 1, 15, 1), Text = "Feature" };
        Button button = new() { Bounds = new(1, 3, 15, 2), Text = "~C~heck it" };

        button.Click += (sender, e) =>
        {
            checkBox.Checked = true;
        };

        form.Controls.Add(checkBox);
        form.Controls.Add(button);
        form.Show();
    }
}
