using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormControlBoxBeforeShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { ControlBox = false };
        form.Show();
    }
}
