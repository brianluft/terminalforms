#pragma once

#include "../common.h"
#include <cstring>

#define Uses_TEvent
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<KeyDownEvent> {
    void operator()(KeyDownEvent* self) const {
        self->keyCode = {};
        self->controlKeyState = {};
        std::memset(self->text, 0, sizeof(self->text));
        self->textLength = {};
    }
};

template <>
struct equals<KeyDownEvent> {
    bool operator()(const KeyDownEvent& self, const KeyDownEvent& other) const {
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

}  // namespace tv

namespace std {
template <>
struct hash<KeyDownEvent> {
    std::size_t operator()(const KeyDownEvent& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<uint16_t>{}(self.keyCode), &x);
        tv::combineHash(std::hash<uint16_t>{}(self.controlKeyState), &x);
        tv::combineHash(std::hash<uint8_t>{}(self.textLength), &x);

        // Include text content in hash if present
        if (self.textLength > 0) {
            for (uint8_t i = 0; i < self.textLength; i++) {
                tv::combineHash(std::hash<uint8_t>{}((uint8_t)self.text[i]), &x);
            }
        }
        return x;
    }
};
}  // namespace std
