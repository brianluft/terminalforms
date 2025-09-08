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
        var demoExe = Path.Combine(exeDir, "TerminalFormsDemo");
        ProcessStartInfo psi = new(demoExe, $"\"{name}\" \"{actualFilePath}\"")
        {
            CreateNoWindow = true,
        };

        using var process = Process.Start(psi)!;

        if (!process.WaitForExit(5000))
        {
            process.Kill();
            throw new Exception($"Process {demoExe} timed out after 5 seconds");
        }

        Assert.AreEqual(0, process.ExitCode);

        // This should have written a screenshot to the file.
        var expectedLines = File.ReadAllLines(expectedFilePath);
        var actualLines = File.ReadAllLines(actualFilePath);

        Assert.AreEqual(expectedLines.Length, actualLines.Length);

        for (int i = 0; i < expectedLines.Length; i++)
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
    }
}
