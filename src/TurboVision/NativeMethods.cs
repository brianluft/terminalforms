using System.Runtime.InteropServices;

namespace TurboVision;

internal static partial class NativeMethods
{
    [LibraryImport(Global.DLL_NAME)]
    public static partial Error TV_getLastErrorMessageLength(out int @out);

    [LibraryImport(Global.DLL_NAME)]
    public static partial Error TV_getLastErrorMessage(Span<byte> buffer, int bufferSize);

    [LibraryImport(Global.DLL_NAME)]
    public static partial void TV_overrideMethod(
        VirtualMethod virtualMethod,
        IntPtr functionPointer
    );
}
