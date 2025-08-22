#pragma once

#include "../common.h"

#define Uses_TApplication
#include <tvision/tv.h>

class TApplicationImpl : public TApplication {
   public:
    TApplicationImpl();

    typedef void (*destructor_ptr)(TApplicationImpl* self);
    virtual ~TApplicationImpl();

    typedef void (*suspend_ptr)(TApplicationImpl* self);
    void suspend() override;
    void suspend_base();

    typedef void (*resume_ptr)(TApplicationImpl* self);
    void resume() override;
    void resume_base();

    typedef TRect* (*getTileRect_ptr)(TApplicationImpl* self);
    TRect getTileRect() override;
    TRect getTileRect_base();

    typedef void (*handleEvent_ptr)(TApplicationImpl* self, TEvent* event);
    void handleEvent(TEvent& event) override;
    void handleEvent_base(TEvent& event);

    typedef void (*writeShellMsg_ptr)(TApplicationImpl* self);
    void writeShellMsg() override;
    void writeShellMsg_base();

    // TODO: TProgram functions
};
