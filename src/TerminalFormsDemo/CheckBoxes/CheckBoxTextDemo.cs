using TerminalForms;

namespace TerminalFormsDemo.CheckBoxes;

public class CheckBoxTextDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        CheckBox checkBox = new() { Bounds = new(1, 1, 15, 1), Text = "Original Text" };
        Button button = new() { Bounds = new(1, 3, 15, 2), Text = "~C~hange it" };

        button.Click += (sender, e) =>
        {
            checkBox.Text = "Clicked!";
        };

        form.Controls.Add(checkBox);
        form.Controls.Add(button);
        form.Show();
    }
}
