using TerminalForms;

namespace TerminalFormsDemo;

[TestInclude]
public class FormResizeAfterShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new();
        form.Show();
        form.Bounds = new(1, 1, 20, 8);
    }
}
