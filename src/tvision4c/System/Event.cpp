#include "Event.h"
#include "../Error.h"
#include <cstring>

#define Uses_TEventQueue
#include <tvision/tv.h>

EXPORT tv::Error TV_Event_placementSize(int32_t* outSize, int32_t* outAlignment) {
    return tv::checkedSize<TEvent>(outSize, outAlignment);
}

EXPORT tv::Error TV_Event_placementNew(TEvent* self) {
    return tv::checkedPlacementNew(self);
}

EXPORT tv::Error TV_Event_placementDelete(TEvent* self) {
    return tv::checkedPlacementDelete(self);
}

EXPORT tv::Error TV_Event_new(TEvent** out) {
    return tv::checkedNew(out);
}

EXPORT tv::Error TV_Event_delete(TEvent* self) {
    return tv::checkedDelete(self);
}

EXPORT tv::Error TV_Event_equals(TEvent* self, TEvent* other, BOOL* out) {
    if (!out) {
        return tv::Error_ArgumentNull;
    }

    if (!self && !other) {
        *out = TRUE;
        return tv::Success;
    }

    if (!self || !other) {
        *out = FALSE;
        return tv::Success;
    }

    *out = memcmp(self, other, sizeof(TEvent)) == 0;
    return tv::Success;
}

EXPORT tv::Error TV_Event_hash(TEvent* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    // Hash the raw bytes
    uint8_t bytes[sizeof(TEvent)];
    memcpy(bytes, self, sizeof(TEvent));

    int32_t result = 0;
    for (size_t i = 0; i < sizeof(TEvent); i++) {
        tv::hash(bytes[i], &result);
    }

    *out = result;
    return tv::Success;
}

EXPORT tv::Error TV_Event_get_what(TEvent* self, uint16_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->what;
    return tv::Success;
}

EXPORT tv::Error TV_Event_set_what(TEvent* self, uint16_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->what = value;
    return tv::Success;
}

EXPORT tv::Error TV_Event_get_mouse(TEvent* self, MouseEventType* dst) {
    if (!self || !dst) {
        return tv::Error_ArgumentNull;
    }

    *dst = self->mouse;
    return tv::Success;
}

EXPORT tv::Error TV_Event_set_mouse(TEvent* self, MouseEventType* src) {
    if (!self || !src) {
        return tv::Error_ArgumentNull;
    }

    self->mouse = *src;
    return tv::Success;
}

EXPORT tv::Error TV_Event_get_keyDown(TEvent* self, KeyDownEvent* dst) {
    if (!self || !dst) {
        return tv::Error_ArgumentNull;
    }

    *dst = self->keyDown;
    return tv::Success;
}

EXPORT tv::Error TV_Event_set_keyDown(TEvent* self, KeyDownEvent* src) {
    if (!self || !src) {
        return tv::Error_ArgumentNull;
    }

    self->keyDown = *src;
    return tv::Success;
}

EXPORT tv::Error TV_Event_get_message(TEvent* self, MessageEvent* dst) {
    if (!self || !dst) {
        return tv::Error_ArgumentNull;
    }

    *dst = self->message;
    return tv::Success;
}

EXPORT tv::Error TV_Event_set_message(TEvent* self, MessageEvent* src) {
    if (!self || !src) {
        return tv::Error_ArgumentNull;
    }

    self->message = *src;
    return tv::Success;
}

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
