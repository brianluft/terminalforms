namespace TerminalForms;

/// <summary>
/// Represents a button control that can be clicked to perform an action.
/// </summary>
public unsafe partial class Button() : Control(_metaObject)
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfButtonNew,
        NativeMethods.TfButtonDelete,
        NativeMethods.TfButtonEquals,
        NativeMethods.TfButtonHash
    );

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonHash(void* self, out int @out);
    }
}
