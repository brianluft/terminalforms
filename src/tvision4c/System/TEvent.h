#pragma once

#include "../common.h"
#include "MouseEventType.h"
#include "KeyDownEvent.h"
#include "MessageEvent.h"

#define Uses_TEvent
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<TEvent> {
    static void initialize(TEvent* self) {
        self->what = evKeyDown;
        InitializePolicy<KeyDownEvent>::initialize(&self->keyDown);
    }
};

template <>
struct EqualsPolicy<TEvent> {
    static bool equals(const TEvent& self, const TEvent& other) {
        if (self.what != other.what) {
            return false;
        }

        switch (self.what) {
            case evMouseDown:
            case evMouseUp:
            case evMouseMove:
            case evMouseAuto:
            case evMouseWheel:
                return EqualsPolicy<MouseEventType>::equals(self.mouse, other.mouse);
            case evKeyDown:
                return EqualsPolicy<KeyDownEvent>::equals(self.keyDown, other.keyDown);
            default:
                return EqualsPolicy<MessageEvent>::equals(self.message, other.message);
        }
    }
};

template <>
struct HashPolicy<TEvent> {
    static void hash(const TEvent& self, int32_t* seed) {
        switch (self.what) {
            case evMouseDown:
            case evMouseUp:
            case evMouseMove:
            case evMouseAuto:
            case evMouseWheel:
                HashPolicy<MouseEventType>::hash(self.mouse, seed);
                break;
            case evKeyDown:
                HashPolicy<KeyDownEvent>::hash(self.keyDown, seed);
                break;
            default:
                HashPolicy<MessageEvent>::hash(self.message, seed);
                break;
        }
    }
};

}  // namespace tv

TV_DECLARE_BOILERPLATE_FUNCTIONS(TEvent)
TV_DECLARE_GET_SET_PRIMITIVE(TEvent, uint16_t, what)
TV_DECLARE_GET_SET_COPYABLE_OBJECT(TEvent, MouseEventType, mouse)
TV_DECLARE_GET_SET_COPYABLE_OBJECT(TEvent, KeyDownEvent, keyDown)
TV_DECLARE_GET_SET_COPYABLE_OBJECT(TEvent, MessageEvent, message)
EXPORT tv::Error TV_Event_getMouseEvent(TEvent* self);
EXPORT tv::Error TV_Event_getKeyEvent(TEvent* self);
