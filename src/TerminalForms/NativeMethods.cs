using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    public const string DLL_NAME = "tvision4c";

    [LibraryImport(DLL_NAME)]
    public static partial int healthCheck();
}
