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

TV_DECLARE_BOILERPLATE_FUNCTIONS(TRect)
TV_DECLARE_GET_SET_COPYABLE_OBJECT(TRect, TPoint, a)
TV_DECLARE_GET_SET_COPYABLE_OBJECT(TRect, TPoint, b)
EXPORT tv::Error TV_TRect_move(TRect* self, int32_t aDX, int32_t aDY);
EXPORT tv::Error TV_TRect_grow(TRect* self, int32_t aDX, int32_t aDY);
EXPORT tv::Error TV_TRect_intersect(TRect* self, TRect* r);
EXPORT tv::Error TV_TRect_Union(TRect* self, TRect* r);
EXPORT tv::Error TV_TRect_contains(TRect* self, TPoint* p, BOOL* out);
EXPORT tv::Error TV_TRect_isEmpty(TRect* self, BOOL* out);
