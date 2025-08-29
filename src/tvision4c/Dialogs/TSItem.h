#pragma once
#include "../common.h"

#define Uses_TSItem
#include <tvision/tv.h>

namespace tv {
// Initialize public members
template <>
struct initialize<TSItem> {
    void operator()(TSItem* self) const {
        self->value = nullptr;
        self->next = nullptr;
    }
};

// TSItem should use value semantics for equality comparison
template <>
struct equals<TSItem> {
    bool operator()(const TSItem& self, const TSItem& other) const {
        // Compare the string values and next pointers
        bool valueEqual = (self.value == nullptr && other.value == nullptr) ||
            (self.value != nullptr && other.value != nullptr && strcmp(self.value, other.value) == 0);
        return valueEqual && self.next == other.next;
    }
};
}  // namespace tv

// Hash specialization matching the equals semantics
namespace std {
template <>
struct hash<TSItem> {
    std::size_t operator()(const TSItem& p) const noexcept {
        std::size_t x{};
        if (p.value != nullptr) {
            tv::combineHash(std::string_view(p.value), &x);
        }
        tv::combineHash(p.next, &x);
        return x;
    }
};
}  // namespace std
