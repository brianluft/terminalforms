using TerminalForms;

namespace TerminalFormsDemo.RadioButtons;

public class RadioButtonGroupMutableDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Mutable Items" };
        RadioButtonGroup radioGroup = new("Item 1", "Item 2") { Bounds = new(1, 1, 20, 5) };
        Button addButton = new() { Bounds = new(1, 6, 15, 2), Text = "~A~dd Item" };

        var itemCount = 2;
        addButton.Click += (sender, e) =>
        {
            itemCount++;
            radioGroup.Items.Add($"Item {itemCount}");
        };

        form.Controls.Add(radioGroup);
        form.Controls.Add(addButton);
        form.Show();
    }
}
