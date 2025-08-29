#pragma once

#include "../common.h"

#define Uses_TDirEntry
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TDirEntry> {
    void operator()(TDirEntry* self) const {
        // No public members to initialize
    }
};

template <>
struct equals<TDirEntry> {
    bool operator()(const TDirEntry& self, const TDirEntry& other) const {
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

}  // namespace tv

namespace std {
template <>
struct hash<TDirEntry> {
    std::size_t operator()(const TDirEntry& self) const noexcept {
        std::size_t x{};
        // Need to cast away const since dir() and text() are not const methods
        const char* dir = const_cast<TDirEntry&>(self).dir();
        const char* text = const_cast<TDirEntry&>(self).text();
        if (dir) {
            tv::combineHash(std::hash<TStringView>{}(TStringView(dir)), &x);
        }
        if (text) {
            tv::combineHash(std::hash<TStringView>{}(TStringView(text)), &x);
        }
        return x;
    }
};
}  // namespace std
