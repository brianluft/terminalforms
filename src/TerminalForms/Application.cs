namespace TerminalForms;

public static partial class Application
{
    public static void Run()
    {
        Check(NativeMethods.TfApplicationStaticRun());
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfApplicationStaticRun();
    }
}
