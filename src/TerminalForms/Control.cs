namespace TerminalForms;

/// <summary>
/// Represents an abstract base class for all controls in the Terminal Forms framework.
/// Controls are UI elements that can be displayed and interacted with.
/// </summary>
public abstract unsafe class Control : TerminalFormsObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Control"/> class with the specified meta object.
    /// </summary>
    /// <param name="metaObject">The meta object that defines the native functions for this control type.</param>
    protected Control(MetaObject metaObject)
        : base(metaObject) { }

    protected Control(MetaObject metaObject, void* ptr)
        : base(metaObject, ptr) { }
}
