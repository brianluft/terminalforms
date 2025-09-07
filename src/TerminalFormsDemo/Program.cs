using TerminalForms;

if (args.Length != 1 && args.Length != 2)
{
    Console.Error.WriteLine("Usage: TerminalFormsDemo <test-number> [screenshot-file]");
    return;
}

var number = int.Parse(args[0]);

if (args.Length > 1)
{
    Application.EnableDebugScreenshot(args[1]);
}

switch (number)
{
    case 1:
    {
        Form form = new();
        Button button1 = new() { Bounds = new(1, 1, 15, 2) };
        form.Controls.Add(button1);

        Button button2 = new() { Bounds = new(1, 4, 15, 2) };
        form.Controls.Add(button2);

        form.Show();
        break;
    }
}

Application.Run();
