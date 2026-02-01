using TerminalForms;

namespace TerminalFormsDemo.RadioButtons;

public class RadioButtonGroupProgrammaticChangeDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Programmatic Change" };
        RadioButtonGroup radioGroup = new("Apple", "Banana", "Cherry")
        {
            Bounds = new(1, 1, 15, 5),
        };
        Label statusLabel = new() { Bounds = new(1, 6, 30, 1), Text = "Events: 0" };
        Button changeButton = new() { Bounds = new(1, 7, 20, 2), Text = "~C~hange Selection" };

        var eventCount = 0;
        radioGroup.SelectedIndexChanged += (sender, e) =>
        {
            eventCount++;
            statusLabel.Text = $"Events: {eventCount} ({radioGroup.SelectedItem})";
        };

        changeButton.Click += (sender, e) =>
        {
            // Cycle through selections programmatically
            radioGroup.SelectedIndex = (radioGroup.SelectedIndex + 1) % radioGroup.Items.Count;
        };

        form.Controls.Add(radioGroup);
        form.Controls.Add(statusLabel);
        form.Controls.Add(changeButton);
        form.Show();
    }
}
