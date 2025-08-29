#pragma once

#include "../common.h"

#define Uses_TView
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<write_args> {
    void operator()(write_args* self) const {
        self->self = {};
        self->target = {};
        self->buf = {};
        self->offset = {};
    }
};

template <>
struct equals<write_args> {
    bool operator()(const write_args& self, const write_args& other) const {
        return self.self == other.self && self.target == other.target && self.buf == other.buf &&
            self.offset == other.offset;
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<write_args> {
    std::size_t operator()(const write_args& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<uintptr_t>{}(reinterpret_cast<uintptr_t>(self.self)), &x);
        tv::combineHash(std::hash<uintptr_t>{}(reinterpret_cast<uintptr_t>(self.target)), &x);
        tv::combineHash(std::hash<uintptr_t>{}(reinterpret_cast<uintptr_t>(self.buf)), &x);
        tv::combineHash(std::hash<uint16_t>{}(self.offset), &x);
        return x;
    }
};
}  // namespace std
