#pragma once

#include "../common.h"

#define Uses_TResourceItem
#include <tvision/tv.h>

namespace tv {
template <>
struct initialize<TResourceItem> {
    void operator()(TResourceItem* self) const {
        self->pos = {};
        self->size = {};
        self->key = {};
    }
};

template <>
struct equals<TResourceItem> {
    bool operator()(const TResourceItem& self, const TResourceItem& other) const {
        return self.pos == other.pos && self.size == other.size &&
            ((self.key == nullptr && other.key == nullptr) ||
             (self.key != nullptr && other.key != nullptr && strcmp(self.key, other.key) == 0));
    }
};
}  // namespace tv

namespace std {
template <>
struct hash<TResourceItem> {
    std::size_t operator()(const TResourceItem& p) const noexcept {
        std::size_t x{};
        tv::combineHash(p.pos, &x);
        tv::combineHash(p.size, &x);
        if (p.key != nullptr) {
            tv::combineHash(std::string(p.key), &x);
        }
        return x;
    }
};
}  // namespace std
