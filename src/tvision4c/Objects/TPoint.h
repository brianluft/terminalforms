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

TV_DECLARE_BOILERPLATE_FUNCTIONS(TPoint)
TV_DECLARE_BINARY_OPERATOR_IN_PLACE(TPoint, TPoint, +, add)
TV_DECLARE_BINARY_OPERATOR_IN_PLACE(TPoint, TPoint, -, subtract)
TV_DECLARE_BINARY_OPERATOR_COPYABLE_OBJECT(TPoint, TPoint, +, add, TPoint)
TV_DECLARE_BINARY_OPERATOR_COPYABLE_OBJECT(TPoint, TPoint, -, subtract, TPoint)
TV_DECLARE_GET_SET_PRIMITIVE(TPoint, int32_t, x)
TV_DECLARE_GET_SET_PRIMITIVE(TPoint, int32_t, y)
