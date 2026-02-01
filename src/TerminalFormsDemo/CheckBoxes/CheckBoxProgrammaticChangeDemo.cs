using TerminalForms;

namespace TerminalFormsDemo.CheckBoxes;

/// <summary>
/// Demonstrates that CheckedChanged fires on programmatic changes to the Checked property,
/// following Windows Forms semantics where events fire for both user input and code changes.
/// </summary>
public class CheckBoxProgrammaticChangeDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        CheckBox checkBox = new() { Bounds = new(1, 1, 20, 1), Text = "Feature" };
        Label countLabel = new() { Bounds = new(1, 3, 25, 1), Text = "Event count: 0" };
        Button setTrueBtn = new() { Bounds = new(1, 5, 18, 2), Text = "Set ~T~rue" };
        Button setFalseBtn = new() { Bounds = new(1, 7, 18, 2), Text = "Set ~F~alse" };

        var eventCount = 0;

        checkBox.CheckedChanged += (sender, e) =>
        {
            eventCount++;
            countLabel.Text = $"Event count: {eventCount}";
        };

        setTrueBtn.Click += (sender, e) =>
        {
            checkBox.Checked = true;
        };

        setFalseBtn.Click += (sender, e) =>
        {
            checkBox.Checked = false;
        };

        form.Controls.Add(checkBox);
        form.Controls.Add(countLabel);
        form.Controls.Add(setTrueBtn);
        form.Controls.Add(setFalseBtn);
        form.Show();
    }
}
