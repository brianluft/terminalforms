using TerminalForms;

namespace TerminalFormsDemo.Buttons;

// TODO: Currently disabled. We need a non-button control implemented that _doesn't_ accept the Enter key when focused.
// When such a control is focused, pressing Enter should trigger the default button.
// Right now the only control we have is Button.
public abstract class ButtonIsDefaultDemo(bool default1, bool default2)
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

        form.Show();
    }
}

public class ButtonIsDefault1Demo() : ButtonIsDefaultDemo(true, false) { }

public class ButtonIsDefault2Demo() : ButtonIsDefaultDemo(false, true) { }
