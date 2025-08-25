#include "TEvent.h"
#include <cstring>

#define Uses_TEventQueue
#include <tvision/tv.h>

TV_DEFAULT_CONSTRUCTOR(TEvent)
TV_BOILERPLATE_FUNCTIONS(TEvent)
TV_GET_SET_PRIMITIVE(TEvent, uint16_t, what)
TV_GET_SET_COPYABLE_OBJECT(TEvent, MouseEventType, mouse)
TV_GET_SET_COPYABLE_OBJECT(TEvent, KeyDownEvent, keyDown)
TV_GET_SET_COPYABLE_OBJECT(TEvent, MessageEvent, message)

EXPORT tv::Error TV_TEvent_getMouseEvent(TEvent* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    TEventQueue::getMouseEvent(*self);
    return tv::Success;
}

EXPORT tv::Error TV_TEvent_getKeyEvent(TEvent* self) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->getKeyEvent();
    return tv::Success;
}
