#pragma once

#include "common.h"

#define Uses_TButton
#include <tvision/tv.h>

namespace tf {

class Button : public TButton {
   public:
    Button();
};

template <>
struct equals<Button> {
    bool operator()(const Button& self, const Button& other) const { return &self == &other; }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::Button> {
    std::size_t operator()(const tf::Button& p) const noexcept {
        std::size_t x{};
        tf::combineHash(&p, &x);
        return x;
    }
};
}  // namespace std
