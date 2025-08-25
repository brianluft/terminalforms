#include "TApplication.h"
#include "../VirtualMethodTable.h"

#define Uses_TApplication
#include <tvision/tv.h>

TApplicationImpl::TApplicationImpl()
    : TProgInit(TProgram::initStatusLine, TProgram::initMenuBar, TProgram::initDeskTop) {}

TApplicationImpl::~TApplicationImpl() {
    auto ptr = tv::getVirtualMethod<destructor_ptr>(tv::VirtualMethod_Application_destructor);
    if (ptr) {
        ptr(this);
    }
}

void TApplicationImpl::suspend() {
    auto ptr = tv::getVirtualMethod<suspend_ptr>(tv::VirtualMethod_Application_suspend);
    ptr ? ptr(this) : TApplication::suspend();
}

void TApplicationImpl::suspend_base() {
    TApplication::suspend();
}

void TApplicationImpl::resume() {
    auto ptr = tv::getVirtualMethod<resume_ptr>(tv::VirtualMethod_Application_resume);
    ptr ? ptr(this) : TApplication::resume();
}

void TApplicationImpl::resume_base() {
    TApplication::resume();
}

TRect TApplicationImpl::getTileRect() {
    auto ptr = tv::getVirtualMethod<getTileRect_ptr>(tv::VirtualMethod_Application_getTileRect);
    return ptr ? *ptr(this) : TApplication::getTileRect();
}

TRect TApplicationImpl::getTileRect_base() {
    return TApplication::getTileRect();
}

void TApplicationImpl::handleEvent(TEvent& event) {
    auto ptr = tv::getVirtualMethod<handleEvent_ptr>(tv::VirtualMethod_Application_handleEvent);
    ptr ? ptr(this, &event) : TApplication::handleEvent(event);
}

void TApplicationImpl::handleEvent_base(TEvent& event) {
    TApplication::handleEvent(event);
}

void TApplicationImpl::writeShellMsg() {
    auto ptr = tv::getVirtualMethod<writeShellMsg_ptr>(tv::VirtualMethod_Application_writeShellMsg);
    ptr ? ptr(this) : TApplication::writeShellMsg();
}

void TApplicationImpl::writeShellMsg_base() {
    TApplication::writeShellMsg();
}

TV_DEFAULT_CONSTRUCTOR(TApplicationImpl)
TV_BOILERPLATE_FUNCTIONS(TApplicationImpl)

EXPORT tv::Error TV_TApplicationImpl_suspend(TApplicationImpl* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->suspend();
    return tv::Success;
}

EXPORT tv::Error TV_TApplicationImpl_suspend_base(TApplicationImpl* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->suspend_base();
    return tv::Success;
}

EXPORT tv::Error TV_TApplicationImpl_resume(TApplicationImpl* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->resume();
    return tv::Success;
}

EXPORT tv::Error TV_TApplicationImpl_resume_base(TApplicationImpl* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->resume_base();
    return tv::Success;
}

EXPORT tv::Error TV_TApplicationImpl_getTileRect(TApplicationImpl* self, TRect** out) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    *out = new TRect(self->getTileRect());
    return tv::Success;
}

EXPORT tv::Error TV_TApplicationImpl_getTileRect_base(TApplicationImpl* self, TRect** out) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    *out = new TRect(self->getTileRect_base());
    return tv::Success;
}

EXPORT tv::Error TV_TApplicationImpl_handleEvent(TApplicationImpl* self, TEvent* event) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->handleEvent(*event);
    return tv::Success;
}

EXPORT tv::Error TV_TApplicationImpl_handleEvent_base(TApplicationImpl* self, TEvent* event) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->handleEvent_base(*event);
    return tv::Success;
}

EXPORT tv::Error TV_TApplicationImpl_writeShellMsg(TApplicationImpl* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->writeShellMsg();
    return tv::Success;
}

EXPORT tv::Error TV_TApplicationImpl_writeShellMsg_base(TApplicationImpl* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->writeShellMsg_base();
    return tv::Success;
}
