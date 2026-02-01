using TerminalForms;

namespace TerminalFormsDemo.ListBoxes;

public class ListBoxBasicDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "List Box Demo" };
        Label label = new() { Bounds = new(1, 1, 20, 1), Text = "Select a fruit:" };
        ListBox listBox = new("Apple", "Banana", "Cherry", "Date", "Elderberry")
        {
            Bounds = new(1, 2, 20, 6),
        };

        form.Controls.Add(label);
        form.Controls.Add(listBox);
        form.Show();
    }
}
