namespace TerminalForms;

/// <summary>
/// Represents an abstract base class for all controls in the Terminal Forms framework.
/// Controls are UI elements that can be displayed and interacted with.
/// </summary>
public abstract unsafe partial class Control : TerminalFormsObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Control"/> class with the specified meta object.
    /// </summary>
    /// <param name="metaObject">The meta object that defines the native functions for this control type.</param>
    protected Control(MetaObject metaObject)
        : base(metaObject) { }

    protected Control(MetaObject metaObject, void* ptr)
        : base(metaObject, ptr) { }

    public Rectangle Bounds
    {
        get
        {
            Check(NativeMethods.TfControlGetBounds(Ptr, out var bounds));
            return bounds;
        }
        set { Check(NativeMethods.TfControlSetBounds(Ptr, &value)); }
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
