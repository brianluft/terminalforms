using TerminalForms;

namespace TerminalFormsDemo.Buttons;

public class ButtonIsDefaultDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        Button button1 = new() { Bounds = new(1, 1, 15, 2), Text = "1" };
        form.Controls.Add(button1);
        button1.Click += delegate
        {
            button1.Text = "Clicked!";
        };
        Button button2 = new()
        {
            Bounds = new(1, 3, 15, 2),
            Text = "2",
            IsDefault = true,
        };
        button2.Click += delegate
        {
            button2.Text = "Clicked!";
        };
        form.Controls.Add(button2);
        form.Show();
    }
}
