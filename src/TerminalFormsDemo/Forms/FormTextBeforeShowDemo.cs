using TerminalForms;

namespace TerminalFormsDemo.Forms;

public class FormTextBeforeShowDemo : IDemo
{
    public void Setup()
    {
        Form form = new() { Text = "My Form Title" };
        form.Show();
    }
}
