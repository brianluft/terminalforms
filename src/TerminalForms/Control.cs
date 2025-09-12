namespace TerminalForms;

/// <summary>
/// Represents an abstract base class for all controls in the Terminal Forms framework.
/// Controls are UI elements that can be displayed and interacted with.
/// </summary>
public abstract unsafe partial class Control(MetaObject metaObject)
    : TerminalFormsObject(metaObject)
{
    public Rectangle Bounds
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlGetBounds(Ptr, out var bounds));
            return bounds;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlSetBounds(Ptr, &value));
        }
    }

    public Point Location
    {
        get { return Bounds.Location; }
        set { Bounds = new(value, Bounds.Size); }
    }

    public Size Size
    {
        get { return Bounds.Size; }
        set { Bounds = new(Bounds.Location, value); }
    }

    private static unsafe partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlGetBounds(void* view, out Rectangle @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlSetBounds(void* view, Rectangle* value);
    }
}
