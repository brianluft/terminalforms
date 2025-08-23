using System.Runtime.InteropServices;

namespace TurboVision.System;

public static unsafe partial class TClipboard
{
    internal static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TV_TClipboard_setText(string text);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TV_TClipboard_requestText();
    }
}
