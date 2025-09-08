using TerminalForms;
using TerminalFormsDemo;

if (args.Length != 1 && args.Length != 2)
{
    Console.Error.WriteLine("Usage: TerminalFormsDemo \"test-name\" [screenshot-file]");
    return;
}

var test = args[0];

// If a screenshot file is provided, then we will take a screenshot and exit as soon as the UI is idle.
if (args.Length > 1)
{
    Application.EnableDebugScreenshot(args[1]);
}

// Find the requested class by name.
var assembly = typeof(IDemo).Assembly;
var className = "TerminalFormsDemo." + test;
var type = assembly.GetType(className);
if (type == null)
{
    Console.Error.WriteLine($"Demo implementation not found: {className}");
    return;
}

// The implementation sets up the UI but doesn't run it.
var demo = (IDemo)Activator.CreateInstance(type)!;
demo.Setup();

// We run it.
Application.Run();
