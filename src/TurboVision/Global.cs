using System.Text;

namespace TurboVision;

internal static class Global
{
    public const string DLL_NAME = "tvision4c";

    public static UTF8Encoding UTF8Encoding { get; } = new UTF8Encoding(false);
}
