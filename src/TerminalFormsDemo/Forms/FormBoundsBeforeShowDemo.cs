using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormBoundsBeforeShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Bounds = new(5, 3, 25, 10) };
        form.Show();
    }
}
