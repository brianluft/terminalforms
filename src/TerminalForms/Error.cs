namespace TerminalForms;

/// <summary>
/// Represents error codes returned by the native Terminal Forms library (tfcore).
/// These error codes correspond to the Error enum defined in src\tfcore\common.h.
/// </summary>
public enum Error
{
    /// <summary>
    /// Indicates that the operation completed successfully.
    /// </summary>
    Success = 0,

    /// <summary>
    /// An unknown error occurred that doesn't fit into any specific category.
    /// </summary>
    Error_Unknown,

    /// <summary>
    /// There was a failure in the interoperation between managed C# code and the native library.
    /// This typically indicates a marshalling or calling convention problem.
    /// </summary>
    Error_NativeInteropFailure,

    /// <summary>
    /// The operation failed because there is not enough memory available.
    /// </summary>
    Error_OutOfMemory,

    /// <summary>
    /// A required function argument was null when a non-null value was expected.
    /// </summary>
    Error_ArgumentNull,

    /// <summary>
    /// A function argument was invalid or out of the expected range.
    /// </summary>
    Error_InvalidArgument,

    /// <summary>
    /// A flag bit indicating that the error has an associated message.
    /// This can be combined with other error codes to indicate additional error information is available.
    /// </summary>
    Error_HasMessage = 0x8000,
}

/// <summary>
/// Provides extension methods for the <see cref="Error"/> enum to get human-readable error messages.
/// </summary>
public static class ErrorExtensions
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
            Error.Error_Unknown => "An unknown error occurred.",
            Error.Error_NativeInteropFailure =>
                "There was a problem connecting to the TerminalForms native library (tfcore).",
            Error.Error_OutOfMemory => "There is not enough memory to complete the operation.",
            Error.Error_ArgumentNull => "The function argument must not be null.",
            Error.Error_InvalidArgument => "The function argument is invalid.",
            _ => $"An unknown error occurred ({(int)error}).",
        };
}
