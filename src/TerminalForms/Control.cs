namespace TerminalForms;

/// <summary>
/// Represents an abstract base class for all controls in the Terminal Forms framework.
/// Controls are UI elements that can be displayed and interacted with.
/// </summary>
public abstract unsafe partial class Control : TerminalFormsObject
{
    internal Control(MetaObject metaObject)
        : base(metaObject) { }

    /// <summary>
    /// Gets or sets the rectangular bounds of this control, which defines both its position and size
    /// within its parent container. The bounds specify the complete rectangular area that this
    /// control occupies on the screen or within its parent view.
    /// </summary>
    /// <value>
    /// A <see cref="Rectangle"/> that represents the position and size of the control.
    /// The X and Y coordinates are relative to the parent container's coordinate system.
    /// </value>
    /// <remarks>
    /// All views manage rectangular regions of the screen. The bounds define the complete area owned by this control,
    /// including any borders or decorative elements. Changing the bounds will trigger a redraw of the control at its
    /// new position and size.
    /// </remarks>
    public virtual Rectangle Bounds
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

    /// <summary>
    /// Gets or sets the position of the control's upper-left corner relative to its parent container.
    /// This property provides a convenient way to change only the position while preserving the size.
    /// </summary>
    /// <value>
    /// A <see cref="Point"/> representing the X and Y coordinates of the control's upper-left corner.
    /// </value>
    /// <remarks>
    /// This property is a convenience wrapper around the <see cref="Bounds"/> property. Setting the
    /// location will update the control's bounds while preserving its current size.
    /// </remarks>
    public Point Location
    {
        get { return Bounds.Location; }
        set { Bounds = new(value, Bounds.Size); }
    }

    /// <summary>
    /// Gets or sets the size of the control in character cells. In Terminal Forms, all measurements
    /// are in character units rather than pixels, reflecting the text-mode nature of the interface.
    /// </summary>
    /// <value>
    /// A <see cref="Size"/> representing the width and height of the control in character cells.
    /// </value>
    /// <remarks>
    /// This property is a convenience wrapper around the <see cref="Bounds"/> property. Setting the
    /// size will update the control's bounds while preserving its current location. The size
    /// represents the total area owned by the control, including any borders or decorative elements.
    /// </remarks>
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
