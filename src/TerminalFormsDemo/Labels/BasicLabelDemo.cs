using TerminalForms;

namespace TerminalFormsDemo.Labels;

public class BasicLabelDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "Basic Label Demo" };

        // Simple text labels
        Label label1 = new() { Bounds = new(2, 2, 20, 1), Text = "Simple Label" };
        Label label2 = new() { Bounds = new(2, 4, 25, 1), Text = "Another Label Text" };
        Label label3 = new() { Bounds = new(2, 6, 30, 1), Text = "Third Label for Testing" };

        // Label with different text after creation
        Label label4 = new() { Bounds = new(2, 8, 25, 1), Text = "Original Text" };
        form.Controls.Add(label4);
        label4.Text = "BASIC_LABEL_DEMO_SUCCESS"; // Unique marker for grep testing

        form.Controls.Add(label1);
        form.Controls.Add(label2);
        form.Controls.Add(label3);

        form.Show();
    }
}
