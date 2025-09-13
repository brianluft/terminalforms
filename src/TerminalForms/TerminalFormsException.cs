namespace TerminalForms;

/// <summary>
/// Exception thrown when a Terminal Forms operation fails.
/// </summary>
/// <param name="error">Error code.</param>
/// <param name="message">Error message.</param>
public class TerminalFormsException(Error error, string? message = null)
    : Exception(message ?? error.GetDefaultMessage())
{
    /// <summary>
    /// Error code.
    /// </summary>
    public Error Error => error;

    /// <summary>
    /// Checks the error code and throws a <see cref="TerminalFormsException"/> if it is not <see cref="Error.Success"/>.
    /// </summary>
    /// <param name="error">Error code.</param>
    /// <exception cref="TerminalFormsException">Thrown if the error code is not <see cref="Error.Success"/>.</exception>
    public static void Check(Error error)
    {
        if (error == Error.Success)
            return;

        var hasMessage = error.HasFlag(Error.HasMessage);
        error &= ~Error.HasMessage;

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
