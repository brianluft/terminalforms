using TerminalForms;

namespace TerminalFormsDemo.Labels;

public class ComplexLayoutDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Complex Label Layout Demo" };

        // Section 1: Form fields with labels
        Label nameLabel = new() { Bounds = new(2, 2, 15, 1), Text = "~N~ame:" };
        Button nameInput = new() { Bounds = new(18, 2, 20, 1), Text = "Name Input" };

        Label emailLabel = new() { Bounds = new(2, 4, 15, 1), Text = "~E~mail:" };
        Button emailInput = new() { Bounds = new(18, 4, 20, 1), Text = "Email Input" };

        // Section 2: Options with labels
        Label optionsHeader = new() { Bounds = new(2, 6, 20, 1), Text = "Options:" };

        Label option1Label = new() { Bounds = new(4, 7, 15, 1), Text = "~S~ave Settings" };
        CheckBox option1Check = new() { Bounds = new(20, 7, 15, 1), Text = "Enabled" };

        Label option2Label = new() { Bounds = new(4, 8, 15, 1), Text = "~A~uto Save" };
        CheckBox option2Check = new() { Bounds = new(20, 8, 15, 1), Text = "Disabled" };

        // Section 3: Action buttons with labels
        Label actionsHeader = new() { Bounds = new(2, 10, 20, 1), Text = "Actions:" };

        Button saveButton = new() { Bounds = new(4, 11, 12, 1), Text = "~O~K" };
        Button cancelButton = new() { Bounds = new(18, 11, 12, 1), Text = "~C~ancel" };

        // Status label
        Label statusLabel = new()
        {
            Bounds = new(2, 13, 50, 1),
            Text = "Use Alt+N, Alt+E, Alt+S, Alt+A, Alt+O, Alt+C to navigate",
        };

        // Event handlers to show navigation working
        nameInput.Click += (s, e) => statusLabel.Text = "COMPLEX_NAME_SUCCESS";
        emailInput.Click += (s, e) => statusLabel.Text = "COMPLEX_EMAIL_SUCCESS";
        option1Check.CheckedChanged += (s, e) => statusLabel.Text = "COMPLEX_OPTION1_SUCCESS";
        option2Check.CheckedChanged += (s, e) => statusLabel.Text = "COMPLEX_OPTION2_SUCCESS";
        saveButton.Click += (s, e) => statusLabel.Text = "COMPLEX_OK_SUCCESS";
        cancelButton.Click += (s, e) => statusLabel.Text = "COMPLEX_CANCEL_SUCCESS";

        // Add all controls
        form.Controls.Add(nameLabel);
        form.Controls.Add(nameInput);
        form.Controls.Add(emailLabel);
        form.Controls.Add(emailInput);
        form.Controls.Add(optionsHeader);
        form.Controls.Add(option1Label);
        form.Controls.Add(option1Check);
        form.Controls.Add(option2Label);
        form.Controls.Add(option2Check);
        form.Controls.Add(actionsHeader);
        form.Controls.Add(saveButton);
        form.Controls.Add(cancelButton);
        form.Controls.Add(statusLabel);

        form.Show();
    }
}
