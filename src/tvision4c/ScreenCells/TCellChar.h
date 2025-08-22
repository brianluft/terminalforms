#pragma once

#include "../common.h"

#define Uses_TColorAttr
#include <tvision/tv.h>
#include <tvision/scrncell.h>

namespace tv {

// Initialize public members
template <>
struct InitializePolicy<TCellChar> {
    static void initialize(TCellChar* self) {
        // TCellChar has a default constructor that does the right thing
        // But we should zero-initialize for consistency
        memset(self, 0, sizeof(*self));
    }
};

// Use value semantics for TCellChar
template <>
struct EqualsPolicy<TCellChar> {
    static bool equals(const TCellChar& self, const TCellChar& other) {
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

// Hash the same fields as equals
template <>
struct HashPolicy<TCellChar> {
    static void hash(const TCellChar& self, int32_t* seed) {
        // Hash the text content and flags
        for (size_t i = 0; i < self._textLength; ++i) {
            tv::hash(self._text[i], seed);
        }
        tv::hash(self._textLength, seed);
        tv::hash(self._flags, seed);
    }
};

}  // namespace tv
