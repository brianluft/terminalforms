#pragma once

#include "../common.h"

#define Uses_TTimerQueue
#include <tvision/tv.h>

namespace std {
template <>
struct hash<TTimerQueue> {
    std::size_t operator()(const TTimerQueue& self) const noexcept {
        // Use pointer hash as default (reference equality)
        return std::hash<const void*>{}(&self);
    }
};
}  // namespace std