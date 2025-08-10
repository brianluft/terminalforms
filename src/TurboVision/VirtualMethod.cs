namespace TurboVision;

// Matches `src\tvision4c\VirtualMethod.h`
public enum VirtualMethod : int
{
    // Application
    VirtualMethod_Application_destructor = 0,
    VirtualMethod_Application_suspend,
    VirtualMethod_Application_resume,
    VirtualMethod_Application_getTileRect,
    VirtualMethod_Application_handleEvent,
    VirtualMethod_Application_writeShellMsg,

    VirtualMethod_Count,
}
