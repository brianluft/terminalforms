#pragma once

#include "../common.h"
#include "../Objects/TPoint.h"

#define Uses_TEvent
#define Uses_TPoint
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<MouseEventType> {
    void operator()(MouseEventType* self) const {
        tv::initialize<TPoint>{}(&self->where);
        self->eventFlags = {};
        self->controlKeyState = {};
        self->buttons = {};
        self->wheel = {};
    }
};

template <>
struct equals<MouseEventType> {
    bool operator()(const MouseEventType& self, const MouseEventType& other) const {
        return tv::equals<TPoint>{}(self.where, other.where) && self.eventFlags == other.eventFlags &&
            self.controlKeyState == other.controlKeyState && self.buttons == other.buttons && self.wheel == other.wheel;
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<MouseEventType> {
    std::size_t operator()(const MouseEventType& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<TPoint>{}(self.where), &x);
        tv::combineHash(std::hash<uint16_t>{}(self.eventFlags), &x);
        tv::combineHash(std::hash<uint16_t>{}(self.controlKeyState), &x);
        tv::combineHash(std::hash<uint8_t>{}(self.buttons), &x);
        tv::combineHash(std::hash<uint8_t>{}(self.wheel), &x);
        return x;
    }
};
}  // namespace std
