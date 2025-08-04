using System.Runtime.InteropServices;

namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(DLL_NAME)]
    public static partial Error TV_getLastErrorMessageLength(out int @out);

    [LibraryImport(DLL_NAME)]
    public static partial Error TV_getLastErrorMessage(Span<byte> buffer, int bufferSize);
}
