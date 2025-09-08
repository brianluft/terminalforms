using System.Diagnostics;
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
        Assert.AreEqual(File.ReadAllText(expectedFilePath), File.ReadAllText(actualFilePath));
    }
}
