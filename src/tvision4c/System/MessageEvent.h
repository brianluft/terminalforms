#pragma once

#include "../common.h"

#define Uses_TEvent
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<MessageEvent> {
    void operator()(MessageEvent* self) const {
        self->command = {};
        self->infoPtr = {};
    }
};

template <>
struct equals<MessageEvent> {
    bool operator()(const MessageEvent& self, const MessageEvent& other) const {
        return self.command == other.command && self.infoPtr == other.infoPtr;
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<MessageEvent> {
    std::size_t operator()(const MessageEvent& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<uint16_t>{}(self.command), &x);
        tv::combineHash(std::hash<uintptr_t>{}(reinterpret_cast<uintptr_t>(self.infoPtr)), &x);
        return x;
    }
};
}  // namespace std
