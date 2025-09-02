namespace TerminalForms;

public unsafe partial class Form() : ContainerControl(_metaObject)
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfFormNew,
        NativeMethods.TfFormDelete,
        NativeMethods.TfFormEquals,
        NativeMethods.TfFormHash
    );

    public void Show()
    {
        Check(NativeMethods.TfFormShow(Ptr));

        // TProgram::deskTop takes ownership.
        IsOwned = false;
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormNew(out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormDelete(void* self);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormEquals(
            void* self,
            void* other,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormHash(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfFormShow(void* self);
    }
}
