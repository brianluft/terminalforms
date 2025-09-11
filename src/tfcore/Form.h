#pragma once

#include "common.h"

#define Uses_TDialog
#include <tvision/tv.h>

namespace tf {

class Form : public TDialog {
   public:
    Form();
};

template <>
struct equals<Form> {
    bool operator()(const Form& self, const Form& other) const { return &self == &other; }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::Form> {
    std::size_t operator()(const tf::Form& p) const noexcept {
        std::size_t x{};
        tf::combineHash(&p, &x);
        return x;
    }
};
}  // namespace std
