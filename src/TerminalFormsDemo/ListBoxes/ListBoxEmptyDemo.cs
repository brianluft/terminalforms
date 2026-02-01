using TerminalForms;

namespace TerminalFormsDemo.ListBoxes;

public class ListBoxEmptyDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Empty List Demo" };

        // Create empty list box
        ListBox listBox = new() { Bounds = new(1, 1, 20, 5) };

        Label indexLabel = new()
        {
            Bounds = new(1, 6, 30, 1),
            Text = $"SelectedIndex: {listBox.SelectedIndex}",
        };

        Label countLabel = new()
        {
            Bounds = new(1, 7, 30, 1),
            Text = $"Count: {listBox.Items.Count}",
        };

        form.Controls.Add(listBox);
        form.Controls.Add(indexLabel);
        form.Controls.Add(countLabel);
        form.Show();
    }
}
