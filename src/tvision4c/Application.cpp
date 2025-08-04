#include "Application.h"
#include "VirtualMethodTable.h"

#define Uses_TApplication
#include <tvision/tv.h>

namespace tv {

Application::Application() : TProgInit(TProgram::initStatusLine, TProgram::initMenuBar, TProgram::initDeskTop) {}

EXPORT tv::Error TV_Application_new(tv::Application** out) {
    return tv::checkedNew(out);
}

Application::~Application() {
    auto ptr = getVirtualMethod<destructor_ptr>(VirtualMethod_Application_destructor);
    if (ptr) {
        ptr(this);
    }
}

EXPORT tv::Error TV_Application_delete(tv::Application* self) {
    return tv::checkedDelete(self);
}

void Application::suspend() {
    auto ptr = getVirtualMethod<suspend_ptr>(VirtualMethod_Application_suspend);
    ptr ? ptr(this) : TApplication::suspend();
}

EXPORT tv::Error TV_Application_suspend(tv::Application* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->suspend();
    return tv::Success;
}

void Application::suspend_base() {
    TApplication::suspend();
}

EXPORT tv::Error TV_Application_suspend_base(tv::Application* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->suspend_base();
    return tv::Success;
}

void Application::resume() {
    auto ptr = getVirtualMethod<resume_ptr>(VirtualMethod_Application_resume);
    ptr ? ptr(this) : TApplication::resume();
}

EXPORT tv::Error TV_Application_resume(tv::Application* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->resume();
    return tv::Success;
}

void Application::resume_base() {
    TApplication::resume();
}

EXPORT tv::Error TV_Application_resume_base(tv::Application* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->resume_base();
    return tv::Success;
}

TRect Application::getTileRect() {
    auto ptr = getVirtualMethod<getTileRect_ptr>(VirtualMethod_Application_getTileRect);
    return ptr ? *ptr(this) : TApplication::getTileRect();
}

EXPORT tv::Error TV_Application_getTileRect(tv::Application* self, TRect** out) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    *out = new TRect(self->getTileRect());
    return tv::Success;
}

TRect Application::getTileRect_base() {
    return TApplication::getTileRect();
}

EXPORT tv::Error TV_Application_getTileRect_base(tv::Application* self, TRect** out) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    *out = new TRect(self->getTileRect_base());
    return tv::Success;
}

void Application::handleEvent(TEvent& event) {
    auto ptr = getVirtualMethod<handleEvent_ptr>(VirtualMethod_Application_handleEvent);
    ptr ? ptr(this, &event) : TApplication::handleEvent(event);
}

EXPORT tv::Error TV_Application_handleEvent(tv::Application* self, TEvent* event) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->handleEvent(*event);
    return tv::Success;
}

void Application::handleEvent_base(TEvent& event) {
    TApplication::handleEvent(event);
}

EXPORT tv::Error TV_Application_handleEvent_base(tv::Application* self, TEvent* event) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->handleEvent_base(*event);
    return tv::Success;
}

void Application::writeShellMsg() {
    auto ptr = getVirtualMethod<writeShellMsg_ptr>(VirtualMethod_Application_writeShellMsg);
    ptr ? ptr(this) : TApplication::writeShellMsg();
}

EXPORT tv::Error TV_Application_writeShellMsg(tv::Application* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->writeShellMsg();
    return tv::Success;
}

void Application::writeShellMsg_base() {
    TApplication::writeShellMsg();
}

EXPORT tv::Error TV_Application_writeShellMsg_base(tv::Application* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->writeShellMsg_base();
    return tv::Success;
}

}  // namespace tv
