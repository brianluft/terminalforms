using System.Text;

namespace TerminalForms;

internal static class Global
{
    public const string DLL_NAME = "tfcore";

    /// <summary>
    /// This UTF8 encoding omits the BOM as tvision expects.
    /// </summary>
    public static UTF8Encoding UTF8Encoding { get; } = new UTF8Encoding(false);
}
