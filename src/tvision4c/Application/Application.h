#pragma once

#include "../common.h"
#include "../Error.h"

#define Uses_TApplication
#include <tvision/tv.h>

namespace tv {

class Application : public TApplication {
   public:
    Application();

    typedef void (*destructor_ptr)(Application* self);
    virtual ~Application();

    typedef void (*suspend_ptr)(Application* self);
    void suspend() override;
    void suspend_base();

    typedef void (*resume_ptr)(Application* self);
    void resume() override;
    void resume_base();

    typedef TRect* (*getTileRect_ptr)(Application* self);
    TRect getTileRect() override;
    TRect getTileRect_base();

    typedef void (*handleEvent_ptr)(Application* self, TEvent* event);
    void handleEvent(TEvent& event) override;
    void handleEvent_base(TEvent& event);

    typedef void (*writeShellMsg_ptr)(Application* self);
    void writeShellMsg() override;
    void writeShellMsg_base();

    // TODO: TProgram functions
};

}  // namespace tv

EXPORT tv::Error TV_Application_new(tv::Application** out);
EXPORT tv::Error TV_Application_delete(tv::Application* self);
EXPORT tv::Error TV_Application_suspend(tv::Application* self);
EXPORT tv::Error TV_Application_suspend_base(tv::Application* self);
EXPORT tv::Error TV_Application_resume(tv::Application* self);
EXPORT tv::Error TV_Application_resume_base(tv::Application* self);
EXPORT tv::Error TV_Application_getTileRect(tv::Application* self, TRect** out);
EXPORT tv::Error TV_Application_getTileRect_base(tv::Application* self, TRect** out);
EXPORT tv::Error TV_Application_handleEvent(tv::Application* self, TEvent* event);
EXPORT tv::Error TV_Application_handleEvent_base(tv::Application* self, TEvent* event);
EXPORT tv::Error TV_Application_writeShellMsg(tv::Application* self);
EXPORT tv::Error TV_Application_writeShellMsg_base(tv::Application* self);
