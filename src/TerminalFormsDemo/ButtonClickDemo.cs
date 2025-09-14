using TerminalForms;

namespace TerminalFormsDemo;

[TestInclude]
public class ButtonClickDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        Button button1 = new() { Bounds = new(1, 1, 15, 2), Text = "Click Me" };
        button1.Click += (sender, e) =>
        {
            button1.Text = "Clicked!";
        };
        form.Controls.Add(button1);
        form.Show();
    }
}
