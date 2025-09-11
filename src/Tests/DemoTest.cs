using System.Diagnostics;
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
            var interfaceType = typeof(IDemo);
            var assembly = interfaceType.Assembly;
            var demoTypes = assembly
                .GetTypes()
                .Where(t =>
                    interfaceType.IsAssignableFrom(t)
                    && !t.IsAbstract
                    && !t.IsInterface
                    && t.Name.EndsWith("Demo")
                )
                .ToList();

            foreach (var demoType in demoTypes)
            {
                yield return new object[] { demoType.Name };
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
        var expectedFilePath = Path.Combine(
            exeDir,
            "..",
            "..",
            "..",
            "..",
            "..",
            "src",
            "Tests",
            "screenshots",
            $"{name}.txt"
        );

        // Run `TerminalFormsDemo` in the same directory as the IDemo assembly.
        var demoDll = Path.Combine(exeDir, "TerminalFormsDemo.dll");
        ProcessStartInfo psi = new(
            "dotnet",
            $"\"{demoDll}\" --test \"{name}\" --screenshot \"{actualFilePath}\" --log \"{logFilePath}\""
        )
        {
            CreateNoWindow = true,
        };

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
                sb.AppendLine("Expected:");
                for (var j = 0; j <= i + 1 && j < expectedLines.Length; j++)
                {
                    sb.AppendLine($"Line {j}: \"{expectedLines[j].TrimEnd()}\"");
                }
                sb.AppendLine();
                sb.AppendLine("Actual:");
                for (var j = 0; j <= i + 1 && j < actualLines.Length; j++)
                {
                    sb.AppendLine($"Line {j}: \"{actualLines[j].TrimEnd()}\"");
                }
                Assert.Fail(sb.ToString());
            }
        }

        Assert.AreEqual(expectedLines.Length, actualLines.Length);
    }
}
