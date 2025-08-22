#pragma once

#include "../common.h"

#define Uses_TPoint
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<TPoint> {
    static void initialize(TPoint* self) {
        self->x = {};
        self->y = {};
    }
};

template <>
struct EqualsPolicy<TPoint> {
    static bool equals(const TPoint& self, const TPoint& other) { return self.x == other.x && self.y == other.y; }
};

template <>
struct HashPolicy<TPoint> {
    static void hash(const TPoint& self, int32_t* seed) {
        tv::hash(self.x, seed);
        tv::hash(self.y, seed);
    }
};

}  // namespace tv
