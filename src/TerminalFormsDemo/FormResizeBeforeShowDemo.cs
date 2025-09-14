using TerminalForms;

namespace TerminalFormsDemo;

public class FormResizeBeforeShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Bounds = new(1, 1, 20, 8) };
        form.Show();
    }
}
