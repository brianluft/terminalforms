using TerminalForms;

namespace TerminalFormsDemo.CheckBoxes;

public class CheckBoxClickDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        CheckBox checkBox = new() { Bounds = new(1, 1, 15, 1), Text = "~C~lick me" };
        Button button = new() { Bounds = new(1, 3, 15, 2), Text = "Not clicked" };

        checkBox.CheckedChanged += (sender, e) =>
        {
            button.Text = "Clicked!";
        };

        form.Controls.Add(checkBox);
        form.Controls.Add(button);
        form.Show();
    }
}
