using TerminalForms;

namespace TerminalFormsDemo.TextBoxes;

public class TextBoxSelectionDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        TextBox textBox = new() { Bounds = new(1, 1, 25, 1), Text = "Select some text here" };
        Label infoLabel = new() { Bounds = new(1, 3, 35, 1), Text = "Click button to select all" };
        Button selectAllButton = new() { Bounds = new(1, 5, 15, 2), Text = "Select ~A~ll" };
        Button clearButton = new() { Bounds = new(17, 5, 10, 2), Text = "~C~lear" };

        selectAllButton.Click += (sender, e) =>
        {
            textBox.SelectAll();
            infoLabel.Text = $"Selected: '{textBox.SelectedText}'";
        };

        clearButton.Click += (sender, e) =>
        {
            textBox.Clear();
            infoLabel.Text = "Text cleared";
        };

        form.Controls.Add(textBox);
        form.Controls.Add(infoLabel);
        form.Controls.Add(selectAllButton);
        form.Controls.Add(clearButton);
        form.Show();
    }
}
