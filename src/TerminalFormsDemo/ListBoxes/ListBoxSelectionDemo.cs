using TerminalForms;

namespace TerminalFormsDemo.ListBoxes;

public class ListBoxSelectionDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Selection Demo" };
        ListBox listBox = new("Apple", "Banana", "Cherry", "Date") { Bounds = new(1, 1, 20, 5) };

        // Programmatically set selection to "Cherry" (index 2)
        listBox.SelectedIndex = 2;

        Label indexLabel = new()
        {
            Bounds = new(1, 6, 30, 1),
            Text = $"SelectedIndex: {listBox.SelectedIndex}",
        };

        Label itemLabel = new()
        {
            Bounds = new(1, 7, 30, 1),
            Text = $"SelectedItem: {listBox.SelectedItem}",
        };

        form.Controls.Add(listBox);
        form.Controls.Add(indexLabel);
        form.Controls.Add(itemLabel);
        form.Show();
    }
}
