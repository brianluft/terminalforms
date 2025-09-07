#pragma once

#include "common.h"

#define Uses_TRect
#include <tvision/tv.h>

namespace tf {

struct Rectangle {
    int32_t x;
    int32_t y;
    int32_t width;
    int32_t height;

    Rectangle(const TRect& rect);
    TRect toTRect() const;
};

template <>
struct equals<Rectangle> {
    bool operator()(const Rectangle& self, const Rectangle& other) const {
        return self.x == other.x && self.y == other.y && self.width == other.width && self.height == other.height;
    }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::Rectangle> {
    std::size_t operator()(const tf::Rectangle& p) const noexcept {
        std::size_t x{};
        tf::combineHash(p.x, &x);
        tf::combineHash(p.y, &x);
        tf::combineHash(p.width, &x);
        tf::combineHash(p.height, &x);
        return x;
    }
};
}  // namespace std
