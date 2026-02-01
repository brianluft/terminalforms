using TerminalForms;

namespace TerminalFormsDemo.RadioButtons;

public class RadioButtonGroupEventDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Event Demo" };
        RadioButtonGroup radioGroup = new("First", "Second", "Third") { Bounds = new(1, 1, 15, 5) };
        Label statusLabel = new() { Bounds = new(1, 6, 30, 1), Text = "Selected: First" };

        radioGroup.SelectedIndexChanged += (sender, e) =>
        {
            statusLabel.Text = $"Selected: {radioGroup.SelectedItem}";
        };

        form.Controls.Add(radioGroup);
        form.Controls.Add(statusLabel);
        form.Show();
    }
}
