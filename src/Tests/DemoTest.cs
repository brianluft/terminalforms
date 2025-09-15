using System.Diagnostics;
using System.Reflection;
using System.Text;
using TerminalFormsDemo;

namespace Tests;

[TestClass]
public class DemoTest
{
    /// <summary>
    /// Dynamically discovers all demo types that implement IDemo interface.
    /// </summary>
    public static IEnumerable<object[]> DemoTypes
    {
        get
        {
            const string namespacePrefix = "TerminalFormsDemo.";
            var interfaceType = typeof(IDemo);
            var assembly = interfaceType.Assembly;
            var demoTypes = assembly
                .GetTypes()
                .Where(t => interfaceType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .ToList();

            foreach (var demoType in demoTypes)
            {
                yield return new object[] { demoType.FullName![namespacePrefix.Length..] };
            }
        }
    }

    [DynamicData(nameof(DemoTypes))]
    [DataTestMethod]
    public void TestDemo(string name)
    {
        Test(name);
    }

    private static void Test(string name)
    {
        var actualFilePath = Path.Combine(Path.GetTempPath(), "TerminalFormsDemo.txt");
        File.Delete(actualFilePath);

        var logFilePath = Path.Combine(Path.GetTempPath(), "TerminalFormsDemo.log");
        File.Delete(logFilePath);

        try
        {
            TestCore(name, actualFilePath, logFilePath);
        }
        finally
        {
            File.Delete(actualFilePath);
            File.Delete(logFilePath);
        }
    }

    private static void TestCore(string name, string actualFilePath, string logFilePath)
    {
        var exeDir = Path.GetDirectoryName(typeof(IDemo).Assembly.Location)!;
        var filesDir = Path.Combine(
            exeDir,
            "..",
            "..",
            "..",
            "..",
            "..",
            "src",
            "TerminalFormsDemo"
        );
        var eventsFilePath = Path.Combine(
            filesDir,
            $"{name.Replace('.', Path.DirectorySeparatorChar)}-input.txt"
        );
        var expectedFilePath = Path.Combine(
            filesDir,
            $"{name.Replace('.', Path.DirectorySeparatorChar)}-output.txt"
        );

        // Run `TerminalFormsDemo` in the same directory as the IDemo assembly.
        var demoDll = Path.Combine(exeDir, "TerminalFormsDemo.dll");
        var args =
            $"\"{demoDll}\" --test \"{name}\" --output \"{actualFilePath}\" --log \"{logFilePath}\"";
        if (File.Exists(eventsFilePath))
        {
            args += $" --input \"{eventsFilePath}\"";
        }

        ProcessStartInfo psi = new("dotnet", args) { CreateNoWindow = true };

        using var process = Process.Start(psi)!;

        if (!process.WaitForExit(5000))
        {
            process.Kill();
            throw new Exception($"Process {demoDll} timed out after 5 seconds");
        }

        if (process.ExitCode != 0)
        {
            if (File.Exists(logFilePath))
            {
                var log = File.ReadAllText(logFilePath);
                Assert.Fail($"Exit code {process.ExitCode}. Log output:\n" + log);
            }
            else
            {
                Assert.Fail($"Exit code {process.ExitCode}. No log output.");
            }
        }

        // This should have written a screenshot to the file.
        var expectedLines = File.ReadAllLines(expectedFilePath);
        var actualLines = File.ReadAllLines(actualFilePath);

        for (int i = 0; i < Math.Min(expectedLines.Length, actualLines.Length); i++)
        {
            if (expectedLines[i].TrimEnd() != actualLines[i].TrimEnd())
            {
                StringBuilder sb = new("Mismatch at line index ");
                sb.Append(i);
                sb.AppendLine(".");
                sb.AppendLine("---Expected---");
                for (var j = 0; j < expectedLines.Length; j++)
                {
                    sb.AppendLine(expectedLines[j]);
                }
                sb.AppendLine("---Actual---");
                for (var j = 0; j < actualLines.Length; j++)
                {
                    sb.AppendLine(actualLines[j]);
                }
                sb.AppendLine("------");
                Assert.Fail(sb.ToString());
            }
        }

        Assert.AreEqual(expectedLines.Length, actualLines.Length);
    }
}
