#pragma once

#include "../common.h"

#define Uses_TReplaceDialogRec
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TReplaceDialogRec> {
    void operator()(TReplaceDialogRec* self) const {
        memset(self->find, 0, sizeof(self->find));
        memset(self->replace, 0, sizeof(self->replace));
        self->options = {};
    }
};

template <>
struct equals<TReplaceDialogRec> {
    bool operator()(const TReplaceDialogRec& self, const TReplaceDialogRec& other) const {
        return strcmp(self.find, other.find) == 0 && strcmp(self.replace, other.replace) == 0 &&
            self.options == other.options;
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<TReplaceDialogRec> {
    std::size_t operator()(const TReplaceDialogRec& self) const noexcept {
        std::size_t x{};
        tv::combineHash(std::hash<std::string>{}(std::string(self.find)), &x);
        tv::combineHash(std::hash<std::string>{}(std::string(self.replace)), &x);
        tv::combineHash(std::hash<uint16_t>{}(self.options), &x);
        return x;
    }
};
}  // namespace std
