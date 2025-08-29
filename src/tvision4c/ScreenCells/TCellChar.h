#pragma once

#include "../common.h"

#define Uses_TColorAttr
#include <tvision/tv.h>
#include <tvision/scrncell.h>

namespace tv {

// Initialize public members
template <>
struct initialize<TCellChar> {
    void operator()(TCellChar* self) const {
        // TCellChar has a default constructor that does the right thing
        // But we should zero-initialize for consistency
        memset(self, 0, sizeof(*self));
    }
};

// Use value semantics for TCellChar
template <>
struct equals<TCellChar> {
    bool operator()(const TCellChar& self, const TCellChar& other) const {
        // Compare individual members to avoid padding issues
        if (self._textLength != other._textLength)
            return false;
        if (self._flags != other._flags)
            return false;

        // Compare text content up to the length
        for (int i = 0; i < self._textLength; ++i) {
            if (self._text[i] != other._text[i])
                return false;
        }

        return true;
    }
};

}  // namespace tv

// Hash the same fields as equals
namespace std {
template <>
struct hash<TCellChar> {
    std::size_t operator()(const TCellChar& self) const noexcept {
        std::size_t x{};
        // Hash the text content and flags
        for (size_t i = 0; i < self._textLength; ++i) {
            tv::combineHash(std::hash<char>{}(self._text[i]), &x);
        }
        tv::combineHash(std::hash<uint8_t>{}(self._textLength), &x);
        tv::combineHash(std::hash<uint8_t>{}(self._flags), &x);
        return x;
    }
};
}  // namespace std
