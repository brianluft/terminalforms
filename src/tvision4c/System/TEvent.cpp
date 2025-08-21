#include "TEvent.h"
#include <cstring>

#define Uses_TEventQueue
#include <tvision/tv.h>

TV_IMPLEMENT_BOILERPLATE_FUNCTIONS(TEvent)
TV_IMPLEMENT_GET_SET_PRIMITIVE(TEvent, uint16_t, what)
TV_IMPLEMENT_GET_SET_COPYABLE_OBJECT(TEvent, MouseEventType, mouse)
TV_IMPLEMENT_GET_SET_COPYABLE_OBJECT(TEvent, KeyDownEvent, keyDown)
TV_IMPLEMENT_GET_SET_COPYABLE_OBJECT(TEvent, MessageEvent, message)

EXPORT tv::Error TV_Event_getMouseEvent(TEvent* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    TEventQueue::getMouseEvent(*self);
    return tv::Success;
}

EXPORT tv::Error TV_Event_getKeyEvent(TEvent* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->getKeyEvent();
    return tv::Success;
}
