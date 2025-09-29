using TerminalForms;

namespace TerminalFormsDemo.Labels;

public class HotkeyLabelDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Hotkey Label Demo" };

        // Labels with hotkeys and buttons/controls to navigate to
        Label label1 = new() { Bounds = new(2, 2, 15, 1), Text = "~F~irst Button:" };
        Button button1 = new() { Bounds = new(18, 2, 12, 1), Text = "Button 1" };

        Label label2 = new() { Bounds = new(2, 4, 15, 1), Text = "~S~econd Button:" };
        Button button2 = new() { Bounds = new(18, 4, 12, 1), Text = "Button 2" };

        Label label3 = new() { Bounds = new(2, 6, 15, 1), Text = "~T~hird Button:" };
        Button button3 = new() { Bounds = new(18, 6, 12, 1), Text = "Button 3" };

        // Test label to show functionality is working
        Label statusLabel = new()
        {
            Bounds = new(2, 8, 40, 1),
            Text = "Press Alt+F, Alt+S, or Alt+T to test hotkeys",
        };

        // Add event handlers to show when buttons get focus
        button1.Click += (s, e) => statusLabel.Text = "HOTKEY_FIRST_SUCCESS";
        button2.Click += (s, e) => statusLabel.Text = "HOTKEY_SECOND_SUCCESS";
        button3.Click += (s, e) => statusLabel.Text = "HOTKEY_THIRD_SUCCESS";

        form.Controls.Add(label1);
        form.Controls.Add(button1);
        form.Controls.Add(label2);
        form.Controls.Add(button2);
        form.Controls.Add(label3);
        form.Controls.Add(button3);
        form.Controls.Add(statusLabel);

        form.Show();
    }
}
