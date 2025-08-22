#pragma once

#include "../common.h"

#define Uses_TView
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<write_args> {
    static void initialize(write_args* self) {
        self->self = {};
        self->target = {};
        self->buf = {};
        self->offset = {};
    }
};

template <>
struct EqualsPolicy<write_args> {
    static bool equals(const write_args& self, const write_args& other) {
        return self.self == other.self && self.target == other.target && self.buf == other.buf &&
            self.offset == other.offset;
    }
};

template <>
struct HashPolicy<write_args> {
    static void hash(const write_args& self, int32_t* seed) {
        tv::hash(reinterpret_cast<uintptr_t>(self.self), seed);
        tv::hash(reinterpret_cast<uintptr_t>(self.target), seed);
        tv::hash(reinterpret_cast<uintptr_t>(self.buf), seed);
        tv::hash(self.offset, seed);
    }
};

}  // namespace tv
