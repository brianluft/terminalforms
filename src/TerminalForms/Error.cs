namespace TerminalForms;

/// <summary>
/// An error code associated with a <see cref="TerminalFormsException"/>.
/// </summary>
public enum Error
{
    // These error codes correspond to the Error enum defined in src\tfcore\common.h.
    // In this C# enum, we drop the "Error_" prefix from the error codes in the C++ enum.

    /// <summary>
    /// Indicates that the operation completed successfully.
    /// </summary>
    Success = 0,

    /// <summary>
    /// An unknown error occurred that doesn't fit into any specific category.
    /// </summary>
    Unknown,

    /// <summary>
    /// There was a failure in the interoperation between managed C# code and the native library.
    /// This typically indicates a marshalling or calling convention problem.
    /// </summary>
    NativeInteropFailure,

    /// <summary>
    /// The operation failed because there is not enough memory available.
    /// </summary>
    OutOfMemory,

    /// <summary>
    /// A required function argument was null when a non-null value was expected.
    /// </summary>
    ArgumentNull,

    /// <summary>
    /// A function argument was invalid or out of the expected range.
    /// </summary>
    InvalidArgument,

    /// <summary>
    /// A flag bit indicating that the error has an associated message.
    /// This can be combined with other error codes to indicate additional error information is available.
    /// </summary>
    HasMessage = 0x8000,
}

/// <summary>
/// Provides extension methods for the <see cref="Error"/> enum to get human-readable error messages.
/// </summary>
internal static class ErrorExtensions
{
    /// <summary>
    /// Gets a human-readable error message for the specified error code.
    /// This provides default error messages for all known error types.
    /// </summary>
    /// <param name="error">The error code to get a message for.</param>
    /// <returns>A descriptive error message explaining the error condition.</returns>
    public static string GetDefaultMessage(this Error error) =>
        error switch
        {
            Error.Success => "The operation completed successfully.",
            Error.Unknown => "An unknown error occurred.",
            Error.NativeInteropFailure =>
                "There was a problem connecting to the TerminalForms native library (tfcore).",
            Error.OutOfMemory => "There is not enough memory to complete the operation.",
            Error.ArgumentNull => "The function argument must not be null.",
            Error.InvalidArgument => "The function argument is invalid.",
            _ => $"An unknown error occurred ({(int)error}).",
        };
}
