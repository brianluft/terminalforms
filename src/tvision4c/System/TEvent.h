#pragma once

#include "../common.h"
#include "MouseEventType.h"
#include "KeyDownEvent.h"
#include "MessageEvent.h"

#define Uses_TEvent
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TEvent> {
    void operator()(TEvent* self) const {
        self->what = evKeyDown;
        tv::initialize<KeyDownEvent>{}(&self->keyDown);
    }
};

template <>
struct equals<TEvent> {
    bool operator()(const TEvent& self, const TEvent& other) const {
        if (self.what != other.what) {
            return false;
        }

        switch (self.what) {
            case evMouseDown:
            case evMouseUp:
            case evMouseMove:
            case evMouseAuto:
            case evMouseWheel:
                return tv::equals<MouseEventType>{}(self.mouse, other.mouse);
            case evKeyDown:
                return tv::equals<KeyDownEvent>{}(self.keyDown, other.keyDown);
            default:
                return tv::equals<MessageEvent>{}(self.message, other.message);
        }
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<TEvent> {
    std::size_t operator()(const TEvent& self) const noexcept {
        std::size_t x{};
        switch (self.what) {
            case evMouseDown:
            case evMouseUp:
            case evMouseMove:
            case evMouseAuto:
            case evMouseWheel:
                tv::combineHash(std::hash<MouseEventType>{}(self.mouse), &x);
                break;
            case evKeyDown:
                tv::combineHash(std::hash<KeyDownEvent>{}(self.keyDown), &x);
                break;
            default:
                tv::combineHash(std::hash<MessageEvent>{}(self.message), &x);
                break;
        }
        return x;
    }
};
}  // namespace std
