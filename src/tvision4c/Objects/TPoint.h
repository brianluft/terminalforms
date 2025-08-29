#pragma once

#include "../common.h"

#define Uses_TPoint
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TPoint> {
    void operator()(TPoint* self) const {
        self->x = {};
        self->y = {};
    }
};

template <>
struct equals<TPoint> {
    bool operator()(const TPoint& self, const TPoint& other) const { return self.x == other.x && self.y == other.y; }
};

}  // namespace tv

namespace std {
template <>
struct hash<TPoint> {
    std::size_t operator()(const TPoint& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<int32_t>{}(self.x), &x);
        tv::combineHash(std::hash<int32_t>{}(self.y), &x);
        return x;
    }
};
}  // namespace std
