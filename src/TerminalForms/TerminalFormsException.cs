namespace TerminalForms;

public class TerminalFormsException(Error error, string? message = null)
    : Exception(message ?? error.GetDefaultMessage())
{
    public Error Error => error;

    public static void Check(Error error)
    {
        if (error == Error.Success)
            return;

        var hasMessage = error.HasFlag(Error.Error_HasMessage);
        error &= ~Error.Error_HasMessage;

        if (hasMessage)
        {
            var error2 = NativeMethods.TfGetLastErrorMessage(out var message);
            if (error2 == Error.Success)
                throw new TerminalFormsException(error, message);
            throw new TerminalFormsException(error);
        }

        throw new TerminalFormsException(error);
    }
}
