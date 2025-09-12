namespace TerminalForms;

/// <summary>
/// Represents an abstract base class for controls that can contain child controls.
/// </summary>
public abstract unsafe class ContainerControl : Control
{
    private readonly ControlCollection _controls;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerControl"/> class with the specified meta object.
    /// </summary>
    /// <param name="metaObject">The meta object that defines the native functions for this container control type.</param>
    protected ContainerControl(MetaObject metaObject)
        : base(metaObject)
    {
        _controls = new ControlCollection(this);
    }

    /// <summary>
    /// Gets the collection of child controls contained in this container.
    /// </summary>
    public ControlCollection Controls => _controls;
}
