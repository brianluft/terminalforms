using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(DLL_NAME)]
    public static partial void TV_overrideMethod(
        VirtualMethod virtualMethod,
        IntPtr functionPointer
    );
}
