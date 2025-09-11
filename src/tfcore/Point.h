#pragma once

#include "common.h"

namespace tf {

struct Point {
    int32_t X;
    int32_t Y;
};

template <>
struct equals<Point> {
    bool operator()(const Point& self, const Point& other) const { return self.X == other.X && self.Y == other.Y; }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::Point> {
    std::size_t operator()(const tf::Point& p) const noexcept {
        std::size_t x{};
        tf::combineHash(p.X, &x);
        tf::combineHash(p.Y, &x);
        return x;
    }
};
}  // namespace std