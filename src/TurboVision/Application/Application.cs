using System.Runtime.InteropServices;

namespace TurboVision.Application;

public partial class Application
{
    static Application()
    {
        // Test the P/Invoke connection.
        int result;
        try
        {
            result = NativeMethods.TV_healthCheck();
        }
        catch (Exception ex)
        {
            throw new TurboVisionException(
                Error.Error_NativeInteropFailure,
                Error.Error_NativeInteropFailure.GetDefaultMessage() + "\n\n" + ex.Message
            );
        }

        if (result != 123)
        {
            throw new TurboVisionException(Error.Error_NativeInteropFailure);
        }
    }

    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial int TV_healthCheck();
    }
}
