#pragma once

#include "../common.h"

#define Uses_TText
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TTextMetrics> {
    void operator()(TTextMetrics* self) const {
        self->width = {};
        self->characterCount = {};
        self->graphemeCount = {};
    }
};

template <>
struct equals<TTextMetrics> {
    bool operator()(const TTextMetrics& self, const TTextMetrics& other) const {
        return self.width == other.width && self.characterCount == other.characterCount &&
            self.graphemeCount == other.graphemeCount;
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<TTextMetrics> {
    std::size_t operator()(const TTextMetrics& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<uint32_t>{}(self.width), &x);
        tv::combineHash(std::hash<uint32_t>{}(self.characterCount), &x);
        tv::combineHash(std::hash<uint32_t>{}(self.graphemeCount), &x);
        return x;
    }
};
}  // namespace std
