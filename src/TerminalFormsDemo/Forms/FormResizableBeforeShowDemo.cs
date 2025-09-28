using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormResizableBeforeShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Resizable = true };
        form.Show();
    }
}
