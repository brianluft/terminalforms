#pragma once

#include "../common.h"
#include <cstring>

#define Uses_TEvent
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<KeyDownEvent> {
    static void initialize(KeyDownEvent* self) {
        self->keyCode = {};
        self->controlKeyState = {};
        std::memset(self->text, 0, sizeof(self->text));
        self->textLength = {};
    }
};

template <>
struct EqualsPolicy<KeyDownEvent> {
    static bool equals(const KeyDownEvent& self, const KeyDownEvent& other) {
        if (self.keyCode != other.keyCode || self.controlKeyState != other.controlKeyState ||
            self.textLength != other.textLength) {
            return false;
        }

        // Compare text content up to textLength
        if (self.textLength > 0) {
            return std::memcmp(self.text, other.text, self.textLength) == 0;
        }

        return true;
    }
};

template <>
struct HashPolicy<KeyDownEvent> {
    static void hash(const KeyDownEvent& self, int32_t* seed) {
        tv::hash(self.keyCode, seed);
        tv::hash(self.controlKeyState, seed);
        tv::hash(self.textLength, seed);

        // Include text content in hash if present
        if (self.textLength > 0) {
            for (uint8_t i = 0; i < self.textLength; i++) {
                tv::hash((uint8_t)self.text[i], seed);
            }
        }
    }
};

}  // namespace tv
