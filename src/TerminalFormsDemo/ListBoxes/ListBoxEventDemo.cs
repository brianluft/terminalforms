using TerminalForms;

namespace TerminalFormsDemo.ListBoxes;

public class ListBoxEventDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Event Demo" };
        ListBox listBox = new("Apple", "Banana", "Cherry") { Bounds = new(1, 1, 20, 5) };
        Label selectionLabel = new() { Bounds = new(1, 6, 30, 1), Text = "Selected: Apple" };
        Label activationLabel = new() { Bounds = new(1, 7, 30, 1), Text = "Activated: (none)" };

        listBox.SelectedIndexChanged += (sender, e) =>
        {
            selectionLabel.Text = $"Selected: {listBox.SelectedItem ?? "(none)"}";
        };

        listBox.ItemActivated += (sender, e) =>
        {
            activationLabel.Text = $"Activated: {listBox.SelectedItem ?? "(none)"}";
        };

        form.Controls.Add(listBox);
        form.Controls.Add(selectionLabel);
        form.Controls.Add(activationLabel);
        form.Show();
    }
}
