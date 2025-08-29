#pragma once

#include "../common.h"
#include "TPoint.h"

#define Uses_TRect
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TRect> {
    void operator()(TRect* self) const {
        self->a = {};
        self->b = {};
    }
};

template <>
struct equals<TRect> {
    bool operator()(const TRect& self, const TRect& other) const {
        return tv::equals<TPoint>{}(self.a, other.a) && tv::equals<TPoint>{}(self.b, other.b);
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<TRect> {
    std::size_t operator()(const TRect& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<TPoint>{}(self.a), &x);
        tv::combineHash(std::hash<TPoint>{}(self.b), &x);
        return x;
    }
};
}  // namespace std
