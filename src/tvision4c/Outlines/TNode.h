#pragma once

#include "../common.h"

#define Uses_TOutlineViewer
#include <tvision/tv.h>

namespace tv {
template <>
struct initialize<TNode> {
    void operator()(TNode* self) const {
        self->next = nullptr;
        self->text = nullptr;
        self->childList = nullptr;
        self->expanded = True;
    }
};

template <>
struct equals<TNode> {
    bool operator()(const TNode& self, const TNode& other) const {
        return self.next == other.next &&
            (self.text == other.text || (self.text && other.text && strcmp(self.text, other.text) == 0)) &&
            self.childList == other.childList && self.expanded == other.expanded;
    }
};
}  // namespace tv

namespace std {
template <>
struct hash<TNode> {
    std::size_t operator()(const TNode& p) const noexcept {
        std::size_t x{};
        tv::combineHash(p.next, &x);
        if (p.text)
            tv::combineHash(std::string(p.text), &x);
        tv::combineHash(p.childList, &x);
        tv::combineHash(p.expanded, &x);
        return x;
    }
};
}  // namespace std
