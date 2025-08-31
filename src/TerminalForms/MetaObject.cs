namespace TerminalForms;

public unsafe record class MetaObject(
    MetaObject.NewDelegate NativeNew,
    MetaObject.DeleteDelegate NativeDelete,
    MetaObject.EqualsDelegate NativeEquals,
    MetaObject.HashDelegate NativeHash
)
{
    public delegate Error NewDelegate(out void* @out);
    public delegate Error DeleteDelegate(void* self);
    public delegate Error EqualsDelegate(
        void* self,
        void* other,
        [MarshalAs(UnmanagedType.I4)] out bool @out
    );
    public delegate Error HashDelegate(void* self, out int @out);
}
