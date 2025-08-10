using System.Text;

namespace TurboVision;

public class TurboVisionException(Error error, string? message = null)
    : Exception(message ?? error.GetDefaultMessage())
{
    public Error Error => error;

    public static void Check(Error error)
    {
        var hasMessage = error.HasFlag(Error.Error_HasMessage);
        var code = error & ~Error.Error_HasMessage;

        if (code == Error.Success)
            return;

        if (hasMessage)
        {
            var messageError = NativeMethods.TV_getLastErrorMessageLength(out var messageLength);
            if (messageError == Error.Success)
            {
                var utf8Message = new byte[messageLength];
                NativeMethods.TV_getLastErrorMessage(utf8Message, utf8Message.Length);
                var message = Encoding.UTF8.GetString(utf8Message);
                throw new TurboVisionException(code, message);
            }
        }

        throw new TurboVisionException(code);
    }
}
