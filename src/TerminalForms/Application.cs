namespace TerminalForms;

public class Application
{
    public Application()
    {
        // Test the P/Invoke connection.
        try
        {
            var result = NativeMethods.healthCheck();
            if (result != 123)
            {
                throw new TerminalFormsNativeInteropException(
                    $"""
                    The TerminalForms native library is malfunctioning.\n\n
                    healthCheck() returned {result} instead of 123.
                    """
                );
            }
        }
        catch (Exception ex) when (ex is not TerminalFormsException)
        {
            throw new TerminalFormsNativeInteropException(
                $"Failed to connect to native TerminalForms library.\n\n{ex.Message}"
            );
        }
    }
}
