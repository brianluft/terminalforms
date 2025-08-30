#pragma once

#include "../common.h"

#define Uses_TSearchRec
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TSearchRec> {
    void operator()(TSearchRec* self) const {
        self->attr = {};
        self->time = {};
        self->size = {};
        memset(self->name, 0, sizeof(self->name));
    }
};

template <>
struct equals<TSearchRec> {
    bool operator()(const TSearchRec& self, const TSearchRec& other) const {
        return self.attr == other.attr && self.time == other.time && self.size == other.size &&
            strcmp(self.name, other.name) == 0;
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<TSearchRec> {
    std::size_t operator()(const TSearchRec& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<uint8_t>{}(self.attr), &x);
        tv::combineHash(std::hash<int32_t>{}(self.time), &x);
        tv::combineHash(std::hash<int32_t>{}(self.size), &x);
        tv::combineHash(std::hash<std::string>{}(std::string(self.name)), &x);
        return x;
    }
};
}  // namespace std
