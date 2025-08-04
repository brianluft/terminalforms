#include "Application.h"

#define Uses_TApplication
#include <tvision/tv.h>

namespace tv {

Application::Application() : TProgInit(TProgram::initStatusLine, TProgram::initMenuBar, TProgram::initDeskTop) {}

EXPORT tv::Application* Tv_Application_new() {
    return new tv::Application();
}

Application::~Application() {
    auto ptr = getVirtualMethod<destructor_ptr>(VirtualMethod_Application_destructor);
    if (ptr) {
        ptr(this);
    }
}

EXPORT void Tv_Application_delete(tv::Application* self) {
    delete self;
}

void Application::suspend() {
    auto ptr = getVirtualMethod<suspend_ptr>(VirtualMethod_Application_suspend);
    ptr ? ptr(this) : TApplication::suspend();
}

EXPORT void Tv_Application_suspend(tv::Application* self) {
    self->suspend();
}

void Application::suspend_base() {
    TApplication::suspend();
}

EXPORT void Tv_Application_suspend_base(tv::Application* self) {
    self->suspend_base();
}

}  // namespace tv
