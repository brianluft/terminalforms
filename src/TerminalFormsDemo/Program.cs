using TerminalForms;
using TerminalFormsDemo;

string? test = null;
string? screenshotFile = null;
string? logFile = null;
string? eventsFile = null;

void Log(string message)
{
    if (logFile != null)
    {
        File.AppendAllText(logFile, message + "\n");
    }
}

try
{
    if (args.Length % 2 != 0 || args.Length == 0)
    {
        throw new Exception(
            "Usage: TerminalFormsDemo --test \"name\" [--output <file-path>] [--log <file-path>] [--input <file-path>]"
        );
    }

    Log($"Args: {string.Join(" ", args.Select(x => $"\"{x}\""))}");

    for (var i = 0; i < args.Length; i += 2)
    {
        var value = args[i + 1];

        var key = args[i];
        switch (key)
        {
            case "--test":
                test = value;
                break;
            case "--output":
                screenshotFile = value;
                break;
            case "--log":
                logFile = value;
                break;
            case "--input":
                eventsFile = value;
                break;
            default:
                throw new Exception($"Invalid flag: {key}");
        }
    }

    if (test == null)
    {
        throw new Exception("Test is required.");
    }

    Log("Running health check.");
    Application.HealthCheck();

    // If an events file is provided, then we will enable debug events.
    if (eventsFile != null)
    {
        Log("Enabling events...");
        Application.EnableDebugEvents(eventsFile);
        Log("Events enabled.");
    }

    // If a screenshot file is provided, then we will take a screenshot and exit as soon as the UI is idle.
    if (screenshotFile != null)
    {
        Log("Enabling screenshot...");
        Application.EnableDebugScreenshot(screenshotFile);
        Log("Screenshot enabled.");
    }

    // Find the requested class by name.
    var assembly = typeof(IDemo).Assembly;
    Log($"Assembly: {assembly.FullName}");

    var className = "TerminalFormsDemo." + test;
    Log($"Class name: {className}");

    var type =
        assembly.GetType(className)
        ?? throw new Exception($"Demo implementation not found: {className}");
    Log($"Type: {type.FullName}");

    var demo = (IDemo)Activator.CreateInstance(type)!;
    Log("Created demo instance.");

    demo.Setup();
    Log("Setup completed.");

    Application.Run();
    Log("Run completed.");

    return 0;
}
catch (Exception ex)
{
    Log(ex.ToString());

    Console.Error.WriteLine(ex.Message);
    return -1;
}
