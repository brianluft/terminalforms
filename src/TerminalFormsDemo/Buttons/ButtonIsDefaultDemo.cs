using TerminalForms;

namespace TerminalFormsDemo.Buttons;

public abstract class ButtonIsDefaultDemo(bool default1, bool default2) : IDemo
{
    public void Setup()
    {
        Form form = new();

        Button button1 = new()
        {
            Bounds = new(1, 1, 15, 2),
            Text = "1",
            IsDefault = default1,
        };
        button1.Click += delegate
        {
            button1.Text = "Clicked!";
        };
        form.Controls.Add(button1);

        Button button2 = new()
        {
            Bounds = new(1, 3, 15, 2),
            Text = "2",
            IsDefault = default2,
        };
        button2.Click += delegate
        {
            button2.Text = "Clicked!";
        };
        form.Controls.Add(button2);

        // The checkbox has initial focus, not either the buttons, while not accepting the Enter key itself.
        CheckBox checkBox = new() { Bounds = new(1, 5, 15, 1), Text = "CheckBox" };
        form.Controls.Add(checkBox);

        form.Show();
    }
}

public class ButtonIsDefaultDemo1() : ButtonIsDefaultDemo(true, false) { }

public class ButtonIsDefaultDemo2() : ButtonIsDefaultDemo(false, true) { }
