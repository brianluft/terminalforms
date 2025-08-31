namespace TerminalForms;

// Matches `src\TerminalFormsNative\Error.h`
public enum Error
{
    Success = 0,
    Error_Unknown,
    Error_NativeInteropFailure,
    Error_OutOfMemory,
    Error_UnalignedObjectPlacement,
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
                "There was a problem connecting to the TerminalForms native library (TerminalFormsNative).",
            Error.Error_OutOfMemory => "There is not enough memory to complete the operation.",
            Error.Error_ArgumentNull => "The function argument must not be null.",
            _ => $"An unknown error occurred ({(int)error}).",
        };
}
