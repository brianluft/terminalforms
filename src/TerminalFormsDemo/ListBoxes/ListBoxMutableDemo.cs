using TerminalForms;

namespace TerminalFormsDemo.ListBoxes;

public class ListBoxMutableDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Mutable List Demo" };
        ListBox listBox = new() { Bounds = new(1, 1, 25, 6) };

        // Add items programmatically
        listBox.Items.Add("First Item");
        listBox.Items.Add("Second Item");
        listBox.Items.Add("Third Item");

        // Insert at beginning
        listBox.Items.Insert(0, "Inserted at Start");

        // Modify an item
        listBox.Items[2] = "Modified Second";

        Label countLabel = new()
        {
            Bounds = new(1, 7, 30, 1),
            Text = $"Count: {listBox.Items.Count}",
        };

        form.Controls.Add(listBox);
        form.Controls.Add(countLabel);
        form.Show();
    }
}
