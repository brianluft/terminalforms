#pragma once

#include "../common.h"
#include "../Objects/TPoint.h"

#define Uses_TEvent
#define Uses_TPoint
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<MouseEventType> {
    static void initialize(MouseEventType* self) {
        InitializePolicy<TPoint>::initialize(&self->where);
        self->eventFlags = {};
        self->controlKeyState = {};
        self->buttons = {};
        self->wheel = {};
    }
};

template <>
struct EqualsPolicy<MouseEventType> {
    static bool equals(const MouseEventType& self, const MouseEventType& other) {
        return EqualsPolicy<TPoint>::equals(self.where, other.where) && self.eventFlags == other.eventFlags &&
            self.controlKeyState == other.controlKeyState && self.buttons == other.buttons && self.wheel == other.wheel;
    }
};

template <>
struct HashPolicy<MouseEventType> {
    static void hash(const MouseEventType& self, int32_t* seed) {
        HashPolicy<TPoint>::hash(self.where, seed);
        tv::hash(self.eventFlags, seed);
        tv::hash(self.controlKeyState, seed);
        tv::hash(self.buttons, seed);
        tv::hash(self.wheel, seed);
    }
};

}  // namespace tv

TV_DECLARE_BOILERPLATE_FUNCTIONS(MouseEventType)
TV_DECLARE_GET_SET_COPYABLE_OBJECT(MouseEventType, TPoint, where)
TV_DECLARE_GET_SET_PRIMITIVE(MouseEventType, uint16_t, eventFlags)
TV_DECLARE_GET_SET_PRIMITIVE(MouseEventType, uint16_t, controlKeyState)
TV_DECLARE_GET_SET_PRIMITIVE(MouseEventType, uint8_t, buttons)
TV_DECLARE_GET_SET_PRIMITIVE(MouseEventType, uint8_t, wheel)
