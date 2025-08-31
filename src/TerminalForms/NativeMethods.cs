namespace TerminalForms;

internal static partial class NativeMethods
{
    [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
    public static partial Error TfGetLastErrorMessage(out string @out);
}
