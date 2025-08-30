#pragma once

#include "../common.h"

// Forward declare the types we need from helpbase.h to avoid including streaming operators
class TParagraph {
   public:
    TParagraph() noexcept {}
    TParagraph* next;
    bool wrap;
    unsigned short size;
    char* text;
};

namespace tv {

template <>
struct initialize<TParagraph> {
    void operator()(TParagraph* self) const {
        self->next = {};
        self->wrap = {};
        self->size = {};
        self->text = {};
    }
};

template <>
struct equals<TParagraph> {
    bool operator()(const TParagraph& self, const TParagraph& other) const {
        return self.next == other.next && self.wrap == other.wrap && self.size == other.size &&
            tv::stringEquals(self.text, other.text);
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<TParagraph> {
    std::size_t operator()(const TParagraph& self) const noexcept {
        std::size_t x{};
        tv::combineHash(reinterpret_cast<uintptr_t>(self.next), &x);
        tv::combineHash(static_cast<bool>(self.wrap), &x);
        tv::combineHash(static_cast<uint16_t>(self.size), &x);
        tv::combineHash(tv::stringHash(self.text), &x);
        return x;
    }
};
}  // namespace std
