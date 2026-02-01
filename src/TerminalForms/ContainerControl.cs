namespace TerminalForms;

/// <summary>
/// Represents an abstract base class for controls that can contain child controls.
/// </summary>
/// <remarks>
/// Container controls manage a collection of child controls and provide focus
/// navigation between them. They are responsible for the layout and lifecycle
/// of their children.
/// </remarks>
public abstract unsafe partial class ContainerControl : Control
{
    private readonly ControlCollection _controls;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerControl"/> class with the specified meta object.
    /// </summary>
    /// <param name="metaObject">The meta object that defines the native functions for this container control type.</param>
    internal ContainerControl(MetaObject metaObject)
        : base(metaObject)
    {
        _controls = new ControlCollection(this);
    }

    /// <summary>
    /// Gets the collection of child controls contained in this container.
    /// </summary>
    /// <value>
    /// A <see cref="ControlCollection"/> containing all child controls of this container.
    /// </value>
    /// <remarks>
    /// Use this collection to add, remove, or enumerate the child controls. Controls
    /// added to this collection become children of the container and are displayed
    /// within the container's bounds.
    /// </remarks>
    public ControlCollection Controls => _controls;

    /// <summary>
    /// Gets the child control that currently has input focus.
    /// </summary>
    /// <value>
    /// The <see cref="Control"/> that has focus, or null if no child control has focus.
    /// </value>
    /// <remarks>
    /// The active control is the control that receives keyboard input. Only one control
    /// within a container can be active at any time. Use this property to determine
    /// which control is currently receiving user input.
    /// </remarks>
    public Control? ActiveControl
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfContainerControlGetActiveControl(Ptr, out var activePtr));

            if (activePtr == null)
                return null;

            if (!ObjectRegistry.TryGet(activePtr, out var activeObj))
                return null;

            return activeObj as Control;
        }
    }

    /// <summary>
    /// Activates the next control in the tab order.
    /// </summary>
    /// <param name="forward">
    /// true to move to the next control in tab order; false to move to the previous control.
    /// </param>
    /// <returns>
    /// true if a control was activated; otherwise, false.
    /// </returns>
    /// <remarks>
    /// This method moves focus to the next (or previous) control that can receive focus.
    /// Controls that are not visible, not enabled, or have <see cref="Control.CanFocus"/>
    /// set to false are skipped.
    /// </remarks>
    public bool SelectNextControl(bool forward)
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfContainerControlSelectNextControl(Ptr, forward, out var result));
        return result;
    }

    private static partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfContainerControlGetActiveControl(void* self, out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfContainerControlSelectNextControl(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool forward,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );
    }
}
