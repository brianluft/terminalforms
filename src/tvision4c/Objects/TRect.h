#pragma once

#include "../common.h"
#include "TPoint.h"

#define Uses_TRect
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<TRect> {
    static void initialize(TRect* self) {
        self->a = {};
        self->b = {};
    }
};

template <>
struct EqualsPolicy<TRect> {
    static bool equals(const TRect& self, const TRect& other) {
        return EqualsPolicy<TPoint>::equals(self.a, other.a) && EqualsPolicy<TPoint>::equals(self.b, other.b);
    }
};

template <>
struct HashPolicy<TRect> {
    static void hash(const TRect& self, int32_t* seed) {
        HashPolicy<TPoint>::hash(self.a, seed);
        HashPolicy<TPoint>::hash(self.b, seed);
    }
};

}  // namespace tv
