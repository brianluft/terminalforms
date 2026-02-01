using TerminalForms;

namespace TerminalFormsDemo.RadioButtons;

public class RadioButtonGroupSelectionDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Selection Demo" };
        RadioButtonGroup radioGroup = new("Option A", "Option B", "Option C")
        {
            Bounds = new(1, 1, 15, 5),
        };
        Button button = new() { Bounds = new(1, 6, 20, 2), Text = "~S~elect Option C" };

        button.Click += (sender, e) =>
        {
            radioGroup.SelectedIndex = 2;
        };

        form.Controls.Add(radioGroup);
        form.Controls.Add(button);
        form.Show();
    }
}
