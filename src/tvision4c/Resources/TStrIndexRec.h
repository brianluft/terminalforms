#pragma once

#include "../common.h"

#define Uses_TStrIndexRec
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<TStrIndexRec> {
    static void initialize(TStrIndexRec* self) {
        self->key = {};
        self->count = {};
        self->offset = {};
    }
};

template <>
struct EqualsPolicy<TStrIndexRec> {
    static bool equals(const TStrIndexRec& self, const TStrIndexRec& other) {
        return self.key == other.key && self.count == other.count && self.offset == other.offset;
    }
};

template <>
struct HashPolicy<TStrIndexRec> {
    static void hash(const TStrIndexRec& self, int32_t* seed) {
        tv::hash(self.key, seed);
        tv::hash(self.count, seed);
        tv::hash(self.offset, seed);
    }
};

}  // namespace tv
