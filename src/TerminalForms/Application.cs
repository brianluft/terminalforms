namespace TerminalForms;

public static partial class Application
{
    public static void HealthCheck()
    {
        Check(NativeMethods.TfHealthCheck(out var @out));
        if (@out != 123)
        {
            throw new Exception($"Health check failed: {@out}");
        }
    }

    public static void Run()
    {
        Check(NativeMethods.TfApplicationStaticRun());
    }

    public static void EnableDebugScreenshot(string outputFile)
    {
        Check(NativeMethods.TfApplicationStaticEnableDebugScreenshot(outputFile));
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfApplicationStaticRun();

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfApplicationStaticEnableDebugScreenshot(string outputFile);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfHealthCheck(out int @out);
    }
}
