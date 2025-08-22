#pragma once

#include "../common.h"

#define Uses_TText
#include <tvision/tv.h>

namespace tv {

template <>
struct InitializePolicy<TTextMetrics> {
    static void initialize(TTextMetrics* self) {
        self->width = {};
        self->characterCount = {};
        self->graphemeCount = {};
    }
};

template <>
struct EqualsPolicy<TTextMetrics> {
    static bool equals(const TTextMetrics& self, const TTextMetrics& other) {
        return self.width == other.width && self.characterCount == other.characterCount &&
            self.graphemeCount == other.graphemeCount;
    }
};

template <>
struct HashPolicy<TTextMetrics> {
    static void hash(const TTextMetrics& self, int32_t* seed) {
        tv::hash(self.width, seed);
        tv::hash(self.characterCount, seed);
        tv::hash(self.graphemeCount, seed);
    }
};

}  // namespace tv
