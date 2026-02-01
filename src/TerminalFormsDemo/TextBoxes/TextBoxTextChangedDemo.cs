using TerminalForms;

namespace TerminalFormsDemo.TextBoxes;

public class TextBoxTextChangedDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        TextBox textBox = new() { Bounds = new(1, 1, 20, 1), Text = "Edit me" };
        Label statusLabel = new() { Bounds = new(1, 3, 30, 1), Text = "Changes: 0" };
        int changeCount = 0;

        textBox.TextChanged += (sender, e) =>
        {
            changeCount++;
            statusLabel.Text = $"Changes: {changeCount}";
        };

        form.Controls.Add(textBox);
        form.Controls.Add(statusLabel);
        form.Show();
    }
}
