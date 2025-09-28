using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormMaximizeBoxBeforeShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { MaximizeBox = true };
        form.Show();
    }
}
