#pragma once

#include "../common.h"

#define Uses_TStrIndexRec
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TStrIndexRec> {
    void operator()(TStrIndexRec* self) const {
        self->key = {};
        self->count = {};
        self->offset = {};
    }
};

template <>
struct equals<TStrIndexRec> {
    bool operator()(const TStrIndexRec& self, const TStrIndexRec& other) const {
        return self.key == other.key && self.count == other.count && self.offset == other.offset;
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<TStrIndexRec> {
    std::size_t operator()(const TStrIndexRec& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<uint16_t>{}(self.key), &x);
        tv::combineHash(std::hash<uint16_t>{}(self.count), &x);
        tv::combineHash(std::hash<uint16_t>{}(self.offset), &x);
        return x;
    }
};
}  // namespace std
