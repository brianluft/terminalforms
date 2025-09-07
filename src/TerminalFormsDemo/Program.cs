using TerminalForms;

Form form = new();
Button button1 = new() { Bounds = new(1, 1, 15, 2) };
form.Controls.Add(button1);

Button button2 = new() { Bounds = new(1, 4, 15, 2) };
form.Controls.Add(button2);

form.Show();
Application.Run();
