#pragma once

#include "common.h"

namespace tv {

// Matches `src\TerminalForms\VirtualMethod.cs`
enum VirtualMethod : int32_t {
    // Application
    VirtualMethod_Application_destructor = 0,
    VirtualMethod_Application_suspend,
    VirtualMethod_Application_resume,
    VirtualMethod_Application_getTileRect,
    VirtualMethod_Application_handleEvent,
    VirtualMethod_Application_writeShellMsg,

    VirtualMethod_Count,
};

}  // namespace tv
