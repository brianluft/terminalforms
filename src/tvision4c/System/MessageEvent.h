#pragma once

#include "../common.h"

#define Uses_TEvent
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<MessageEvent> {
    static void initialize(MessageEvent* self) {
        self->command = {};
        self->infoPtr = {};
    }
};

template <>
struct EqualsPolicy<MessageEvent> {
    static bool equals(const MessageEvent& self, const MessageEvent& other) {
        return self.command == other.command && self.infoPtr == other.infoPtr;
    }
};

template <>
struct HashPolicy<MessageEvent> {
    static void hash(const MessageEvent& self, int32_t* seed) {
        tv::hash(self.command, seed);
        tv::hash(reinterpret_cast<uintptr_t>(self.infoPtr), seed);
    }
};

}  // namespace tv
