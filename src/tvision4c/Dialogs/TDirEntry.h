#pragma once

#include "../common.h"

#define Uses_TDirEntry
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<TDirEntry> {
    static void initialize(TDirEntry* self) {
        // No public members to initialize
    }
};

template <>
struct EqualsPolicy<TDirEntry> {
    static bool equals(const TDirEntry& self, const TDirEntry& other) {
        // Compare the actual string contents
        // Need to cast away const since dir() and text() are not const methods, but we happen to know that this
        // doesn't mutate the object.
        const char* selfDir = const_cast<TDirEntry&>(self).dir();
        const char* otherDir = const_cast<TDirEntry&>(other).dir();
        const char* selfText = const_cast<TDirEntry&>(self).text();
        const char* otherText = const_cast<TDirEntry&>(other).text();

        bool dirEqual = (selfDir == otherDir) || (selfDir && otherDir && strcmp(selfDir, otherDir) == 0);
        bool textEqual = (selfText == otherText) || (selfText && otherText && strcmp(selfText, otherText) == 0);

        return dirEqual && textEqual;
    }
};

template <>
struct HashPolicy<TDirEntry> {
    static void hash(const TDirEntry& self, int32_t* seed) {
        // Need to cast away const since dir() and text() are not const methods
        const char* dir = const_cast<TDirEntry&>(self).dir();
        const char* text = const_cast<TDirEntry&>(self).text();
        if (dir) {
            tv::hash(TStringView(dir), seed);
        }
        if (text) {
            tv::hash(TStringView(text), seed);
        }
    }
};

}  // namespace tv
