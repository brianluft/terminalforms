namespace TerminalForms;

public class Application
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
            throw new TerminalFormsException(
                Error.Error_NativeInteropFailure,
                Error.Error_NativeInteropFailure.GetDefaultMessage() + "\n\n" + ex.Message
            );
        }

        if (result != 123)
        {
            throw new TerminalFormsException(Error.Error_NativeInteropFailure);
        }
    }
}
