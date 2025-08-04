namespace TerminalForms;

public class Application
{
    public Application()
    {
        TestNativeInterop();
    }

    private static void TestNativeInterop()
    {
        // Test the P/Invoke connection.
        try
        {
            var result = NativeMethods.TvHealthCheck();
            if (result != 123)
            {
                throw new TerminalFormsNativeInteropException(
                    $"""
                    The TerminalForms native library (tvision4c) is malfunctioning.\n\n
                    HealthCheck returned {result} instead of 123.
                    """
                );
            }
        }
        catch (Exception ex) when (ex is not TerminalFormsException)
        {
            throw new TerminalFormsNativeInteropException(
                $"There was a problem connecting to the TerminalForms native library (tvision4c).\n\n{ex.Message}"
            );
        }
    }
}
