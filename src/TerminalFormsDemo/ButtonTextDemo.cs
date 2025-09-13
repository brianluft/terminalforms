using TerminalForms;

namespace TerminalFormsDemo;

[TestInclude]
public class ButtonTextDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        Button button1 = new() { Bounds = new(1, 1, 15, 2), Text = "Original" };
        form.Controls.Add(button1);
        form.Show();
        button1.Text = "Updated";
    }
}
