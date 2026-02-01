using TerminalForms;

namespace TerminalFormsDemo.RadioButtons;

public class RadioButtonGroupBasicDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Radio Buttons" };
        Label label = new() { Bounds = new(1, 1, 20, 1), Text = "Pick a color:" };
        RadioButtonGroup radioGroup = new("~R~ed", "~G~reen", "~B~lue")
        {
            Bounds = new(1, 2, 15, 5),
        };

        form.Controls.Add(label);
        form.Controls.Add(radioGroup);
        form.Show();
    }
}
