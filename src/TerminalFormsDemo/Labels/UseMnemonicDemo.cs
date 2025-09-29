using TerminalForms;

namespace TerminalFormsDemo.Labels;

public class UseMnemonicDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "UseMnemonic Demo" };

        // Label with mnemonic enabled (default)
        Label label1 = new() { Bounds = new(2, 2, 25, 1), Text = "~E~nabled Mnemonic (Alt+E)" };
        Button button1 = new() { Bounds = new(28, 2, 10, 1), Text = "Target 1" };

        // Label with mnemonic disabled
        Label label2 = new()
        {
            Bounds = new(2, 4, 25, 1),
            Text = "~D~isabled Mnemonic",
            UseMnemonic = false,
        };
        Button button2 = new() { Bounds = new(28, 4, 10, 1), Text = "Target 2" };

        // Status label to show results
        Label statusLabel = new()
        {
            Bounds = new(2, 6, 45, 1),
            Text = "Try Alt+E (should work) and Alt+D (should not work)",
        };

        // Toggle button to demonstrate changing UseMnemonic at runtime
        Button toggleButton = new() { Bounds = new(2, 8, 20, 1), Text = "Toggle Mnemonic" };

        // Event handlers
        button1.Click += (s, e) => statusLabel.Text = "MNEMONIC_ENABLED_SUCCESS";
        button2.Click += (s, e) => statusLabel.Text = "MNEMONIC_DISABLED_SUCCESS";

        toggleButton.Click += (s, e) =>
        {
            label2.UseMnemonic = !label2.UseMnemonic;
            statusLabel.Text = $"UseMnemonic now: {label2.UseMnemonic} - TOGGLE_SUCCESS";
        };

        form.Controls.Add(label1);
        form.Controls.Add(button1);
        form.Controls.Add(label2);
        form.Controls.Add(button2);
        form.Controls.Add(statusLabel);
        form.Controls.Add(toggleButton);

        form.Show();
    }
}
