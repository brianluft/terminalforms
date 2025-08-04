namespace TerminalForms;

// Matches `src\tvision4c\Error.h`
public enum Error
{
    Success = 0,
    Error_Unknown,
    Error_NativeInteropFailure,
    Error_OutOfMemory,
    Error_ArgumentNull,
    Error_ArgumentOutOfRange,
    Error_BufferTooSmall,

    Error_HasMessage = 0x8000,
}

public static class ErrorExtensions
{
    public static string GetDefaultMessage(this Error error) =>
        error switch
        {
            Error.Success => "The operation completed successfully.",
            Error.Error_Unknown => "An unknown error occurred.",
            Error.Error_NativeInteropFailure =>
                "There was a problem connecting to the TerminalForms native library (tvision4c).",
            Error.Error_OutOfMemory => "There is not enough memory to complete the operation.",
            Error.Error_ArgumentNull => "The function argument must not be null.",
            Error.Error_ArgumentOutOfRange => "The function argument is out of range.",
            Error.Error_BufferTooSmall => "The text is too long to fit in the destination.",
            _ => $"An unknown error occurred ({(int)error}).",
        };
}
