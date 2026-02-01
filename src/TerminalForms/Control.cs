namespace TerminalForms;

/// <summary>
/// Represents an abstract base class for all controls in the Terminal Forms framework.
/// Controls are UI elements that can be displayed and interacted with.
/// </summary>
public abstract unsafe partial class Control : TerminalFormsObject
{
    private string? _name;
    private object? _tag;
    private bool _previousFocused;

    internal Control(MetaObject metaObject)
        : base(metaObject) { }

    #region Bounds Properties

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
    /// Controls manage rectangular regions of the screen. The bounds define the complete area owned by this control,
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

    #endregion

    #region Visibility and State Properties

    /// <summary>
    /// Gets or sets a value indicating whether the control is displayed.
    /// </summary>
    /// <value>true if the control is visible; otherwise, false. The default is true.</value>
    /// <remarks>
    /// When a control is not visible, it is hidden from the user and does not participate
    /// in the user interface. Setting this property to false hides the control without
    /// removing it from its parent's control collection. The control still exists and
    /// maintains its position in the tab order.
    /// </remarks>
    public bool Visible
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlGetVisible(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlSetVisible(Ptr, value));
            OnVisibleChanged(EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control can respond to user interaction.
    /// </summary>
    /// <value>true if the control is enabled; otherwise, false. The default is true.</value>
    /// <remarks>
    /// Disabled controls appear dimmed and do not respond to keyboard or mouse input.
    /// The control remains visible unless the <see cref="Visible"/> property is also set to false.
    /// Use this property to temporarily prevent user interaction with a control without
    /// hiding it from view.
    /// </remarks>
    public bool Enabled
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlGetEnabled(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlSetEnabled(Ptr, value));
            OnEnabledChanged(EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the control has input focus.
    /// </summary>
    /// <value>true if the control has focus; otherwise, false.</value>
    /// <remarks>
    /// A control that has focus is the active control and receives keyboard input.
    /// Only one control in an application can have focus at any given time.
    /// Use the <see cref="Focus"/> method to programmatically set focus to a control.
    /// </remarks>
    public bool Focused
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlGetFocused(Ptr, out var value));
            return value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control can receive focus.
    /// </summary>
    /// <value>true if the control can receive focus; otherwise, false. The default depends on the control type.</value>
    /// <remarks>
    /// Controls that cannot receive focus are skipped during tab navigation and cannot
    /// be selected by the user. Some controls, such as labels, typically have this
    /// property set to false because they are informational and do not accept input.
    /// </remarks>
    public bool CanFocus
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlGetCanFocus(Ptr, out var value));
            return value;
        }
        set
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlSetCanFocus(Ptr, value));
        }
    }

    #endregion

    #region Tab Navigation Properties

    /// <summary>
    /// Gets the tab order of the control within its container.
    /// </summary>
    /// <value>
    /// The zero-based index indicating the position of the control in the tab order,
    /// or -1 if the control has no parent.
    /// </value>
    /// <remarks>
    /// The tab order determines the sequence in which controls receive focus when
    /// the user presses the Tab key. Controls are focused in ascending order of
    /// their TabIndex values. The TabIndex reflects the order in which controls
    /// were added to their parent container.
    /// </remarks>
    public int TabIndex
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlGetTabIndex(Ptr, out var value));
            return value;
        }
    }

    #endregion

    #region Parent and Identification Properties

    /// <summary>
    /// Gets the parent container of this control.
    /// </summary>
    /// <value>
    /// A <see cref="ContainerControl"/> that represents the parent of this control,
    /// or null if the control has not been added to a container.
    /// </value>
    /// <remarks>
    /// The parent container owns this control and is responsible for managing its
    /// lifetime and layout. A control can only belong to one parent at a time.
    /// </remarks>
    public ContainerControl? Parent
    {
        get
        {
            ObjectDisposedException.ThrowIf(IsDisposed, this);
            Check(NativeMethods.TfControlGetParent(Ptr, out var parentPtr));

            if (parentPtr == null)
                return null;

            if (!ObjectRegistry.TryGet(parentPtr, out var parentObj))
                return null;

            return parentObj as ContainerControl;
        }
    }

    /// <summary>
    /// Gets or sets the name of the control.
    /// </summary>
    /// <value>The name of the control, or null if no name has been assigned.</value>
    /// <remarks>
    /// The name provides a way to identify controls programmatically. You can use
    /// the name to look up controls in a parent's <see cref="ControlCollection"/>
    /// using the string indexer. Names do not need to be unique within a container;
    /// when duplicates exist, the first matching control is returned.
    /// </remarks>
    public string? Name
    {
        get => _name;
        set => _name = value;
    }

    /// <summary>
    /// Gets or sets the object that contains data about the control.
    /// </summary>
    /// <value>An object that contains data about the control. The default is null.</value>
    /// <remarks>
    /// The Tag property can store any object that you want to associate with the control.
    /// This is useful for storing application-specific data without needing to create
    /// a derived class. The framework does not use this property.
    /// </remarks>
    public object? Tag
    {
        get => _tag;
        set => _tag = value;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets input focus to the control.
    /// </summary>
    /// <returns>true if the control received focus; otherwise, false.</returns>
    /// <remarks>
    /// The Focus method attempts to give the control input focus. A control can only
    /// receive focus if it is visible, enabled, and has <see cref="CanFocus"/> set to true.
    /// When a control receives focus, it raises the <see cref="Enter"/> event.
    /// </remarks>
    public bool Focus()
    {
        ObjectDisposedException.ThrowIf(IsDisposed, this);
        Check(NativeMethods.TfControlFocus(Ptr, out var result));
        return result;
    }

    /// <summary>
    /// Called by the framework to check for focus changes and raise Enter/Leave events.
    /// This method should be called periodically or after operations that may change focus.
    /// </summary>
    internal void CheckFocusChanged()
    {
        if (IsDisposed)
            return;

        var currentFocused = Focused;
        if (currentFocused != _previousFocused)
        {
            _previousFocused = currentFocused;
            if (currentFocused)
            {
                OnEnter(EventArgs.Empty);
            }
            else
            {
                OnLeave(EventArgs.Empty);
            }
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when the control receives focus.
    /// </summary>
    /// <remarks>
    /// The Enter event is raised when the control becomes the active control and begins
    /// receiving keyboard input. Use this event to perform any actions that should occur
    /// when the user moves focus to this control.
    /// </remarks>
    public event EventHandler? Enter;

    /// <summary>
    /// Raises the <see cref="Enter"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected virtual void OnEnter(EventArgs e)
    {
        Enter?.Invoke(this, e);
    }

    /// <summary>
    /// Occurs when the control loses focus.
    /// </summary>
    /// <remarks>
    /// The Leave event is raised when the control is no longer the active control
    /// and keyboard input is directed elsewhere. Use this event to perform validation
    /// or save data when the user moves focus away from this control.
    /// </remarks>
    public event EventHandler? Leave;

    /// <summary>
    /// Raises the <see cref="Leave"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected virtual void OnLeave(EventArgs e)
    {
        Leave?.Invoke(this, e);
    }

    /// <summary>
    /// Occurs when the <see cref="Enabled"/> property value changes.
    /// </summary>
    /// <remarks>
    /// Use this event to respond to changes in the control's enabled state,
    /// such as updating related controls or refreshing the display.
    /// </remarks>
    public event EventHandler? EnabledChanged;

    /// <summary>
    /// Raises the <see cref="EnabledChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected virtual void OnEnabledChanged(EventArgs e)
    {
        EnabledChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Occurs when the <see cref="Visible"/> property value changes.
    /// </summary>
    /// <remarks>
    /// Use this event to respond to changes in the control's visibility,
    /// such as pausing animations or releasing resources when hidden.
    /// </remarks>
    public event EventHandler? VisibleChanged;

    /// <summary>
    /// Raises the <see cref="VisibleChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected virtual void OnVisibleChanged(EventArgs e)
    {
        VisibleChanged?.Invoke(this, e);
    }

    #endregion

    #region NativeMethods

    private static unsafe partial class NativeMethods
    {
        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlGetBounds(void* self, out Rectangle @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlSetBounds(void* self, Rectangle* value);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlGetVisible(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlSetVisible(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlGetEnabled(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlSetEnabled(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlGetFocused(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlGetCanFocus(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlSetCanFocus(
            void* self,
            [MarshalAs(UnmanagedType.I4)] bool value
        );

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlGetTabIndex(void* self, out int @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlGetParent(void* self, out void* @out);

        [LibraryImport(Global.DLL_NAME)]
        public static partial Error TfControlFocus(
            void* self,
            [MarshalAs(UnmanagedType.I4)] out bool @out
        );
    }

    #endregion
}
