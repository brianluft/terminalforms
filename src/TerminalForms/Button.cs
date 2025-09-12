using System.Runtime.CompilerServices;

namespace TerminalForms;

/// <summary>
/// Represents a button control that can be clicked to perform an action.
/// </summary>
public unsafe partial class Button : Control
{
    private static readonly MetaObject _metaObject = new(
        NativeMethods.TfButtonNew,
        NativeMethods.TfButtonDelete,
        NativeMethods.TfButtonEquals,
        NativeMethods.TfButtonHash
    );

    public Button()
        : base(_metaObject)
    {
        Check(NativeMethods.TfButtonSetClickEventHandler(Ptr, &NativeClickEventHandler, Ptr));
    }

    public string Text
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonGetText(Ptr, out var text));
            return text;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfButtonSetText(Ptr, value));
        }
    }

    #region Click
    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void NativeClickEventHandler(void* userData)
    {
        try
        {
            if (!ObjectRegistry.TryGet(userData, out var obj))
                return;

            var button = (Button)obj!;
            button.PerformClick();
        }
        catch { }
    }

    public event EventHandler? Click;

    public void PerformClick()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        OnClick();
    }

    protected virtual void OnClick()
    {
        Click?.Invoke(this, EventArgs.Empty);
    }
    #endregion

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

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfButtonSetText(void* self, string text);

        [LibraryImport(Global.DLL_NAME, StringMarshalling = StringMarshalling.Utf8)]
        public static partial Error TfButtonGetText(void* self, out string @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfButtonSetClickEventHandler(
            void* self,
            delegate* unmanaged[Cdecl]<void*, void> function,
            void* userData
        );
    }
}
