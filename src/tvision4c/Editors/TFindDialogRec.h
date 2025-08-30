#pragma once

#include "../common.h"

#define Uses_TFindDialogRec
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TFindDialogRec> {
    void operator()(TFindDialogRec* self) const {
        memset(self->find, 0, sizeof(self->find));
        self->options = {};
    }
};

template <>
struct equals<TFindDialogRec> {
    bool operator()(const TFindDialogRec& self, const TFindDialogRec& other) const {
        return strcmp(self.find, other.find) == 0 && self.options == other.options;
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<TFindDialogRec> {
    std::size_t operator()(const TFindDialogRec& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<std::string>{}(std::string(self.find)), &x);
        tv::combineHash(std::hash<uint16_t>{}(self.options), &x);
        return x;
    }
};
}  // namespace std
