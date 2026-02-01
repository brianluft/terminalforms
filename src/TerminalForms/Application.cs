using System.Collections.ObjectModel;

namespace TerminalForms;

/// <summary>
/// Provides static methods for managing the Terminal Forms application lifecycle.
/// </summary>
public static partial class Application
{
    private static readonly List<Form> _openForms = [];
    private static ReadOnlyCollection<Form>? _openFormsReadOnly;

    /// <summary>
    /// Gets a collection of open forms owned by the application.
    /// </summary>
    /// <value>A <see cref="ReadOnlyCollection{T}"/> containing all currently open forms.</value>
    /// <remarks>
    /// <para>
    /// Forms are automatically added to this collection when <see cref="Form.Show"/> is called
    /// and removed when the form is closed. This collection maintains strong references to
    /// open forms, preventing them from being garbage collected while they are visible.
    /// </para>
    /// <para>
    /// This property is similar to <c>System.Windows.Forms.Application.OpenForms</c> and
    /// provides a way to enumerate and access all currently displayed forms in the application.
    /// </para>
    /// </remarks>
    public static ReadOnlyCollection<Form> OpenForms =>
        _openFormsReadOnly ??= _openForms.AsReadOnly();

    internal static void RegisterOpenForm(Form form)
    {
        if (!_openForms.Contains(form))
        {
            _openForms.Add(form);
        }
    }

    internal static void UnregisterOpenForm(Form form)
    {
        _openForms.Remove(form);
    }

    /// <summary>
    /// Performs a basic health check to verify that the native Terminal Forms library (tfcore)
    /// is properly loaded and functioning. This method may be optionally called before using other
    /// Terminal Forms functionality to ensure the interop layer is working correctly.
    /// </summary>
    /// <exception cref="Exception">Thrown if the health check fails, indicating a problem with the native library.</exception>
    public static void HealthCheck()
    {
        Check(NativeMethods.TfHealthCheck(out var @out));
        if (@out != 123)
        {
            throw new Exception($"Health check failed: {@out}");
        }
    }

    /// <summary>
    /// Starts the main application event loop. This method initializes the Terminal Forms subsystems,
    /// then enters the main event processing loop that continues until a quit command is received.
    /// The event loop repeatedly fetches events from the mouse, keyboard, and other sources,
    /// then dispatches them to the appropriate views for handling.
    /// </summary>
    /// <remarks>
    /// This method blocks until the application is instructed to quit. It should typically be
    /// called after creating and showing your main forms but before any cleanup code.
    /// </remarks>
    public static void Run()
    {
        Check(NativeMethods.TfApplicationStaticRun());
    }

    /// <summary>
    /// Enables debug screenshot functionality, which captures the terminal screen contents
    /// to the specified file when the application exits. This is primarily used for testing
    /// and debugging purposes to verify the visual appearance of the application.
    /// </summary>
    /// <param name="outputFile">The path where the screenshot will be saved when the application exits.</param>
    /// <remarks>
    /// The screenshot is saved in text format, capturing both the character content and
    /// visual layout of the terminal interface. This must be called before <see cref="Run"/>.
    /// </remarks>
    public static void EnableDebugScreenshot(string outputFile)
    {
        Check(NativeMethods.TfApplicationStaticEnableDebugScreenshot(outputFile));
    }

    /// <summary>
    /// Provides a series of keyboard and mouse input events to the application for automated testing.
    /// </summary>
    /// <param name="inputFile">The path to a text file containing the events to be provided to the application.</param>
    public static void EnableDebugEvents(string inputFile)
    {
        Check(NativeMethods.TfApplicationStaticEnableDebugEvents(inputFile));
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfApplicationStaticRun();

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfApplicationStaticEnableDebugScreenshot(string outputFile);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfApplicationStaticEnableDebugEvents(string inputFile);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfHealthCheck(out int @out);
    }
}
