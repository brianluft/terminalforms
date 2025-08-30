#pragma once

#include "../common.h"

#define Uses_TSystemError
#include <tvision/tv.h>

namespace tv {

template <>
struct initialize<TPMRegs> {
    void operator()(TPMRegs* self) const {
        self->di = {};
        self->si = {};
        self->bp = {};
        self->dummy = {};
        self->bx = {};
        self->dx = {};
        self->cx = {};
        self->ax = {};
        self->flags = {};
        self->es = {};
        self->ds = {};
        self->fs = {};
        self->gs = {};
        self->ip = {};
        self->cs = {};
        self->sp = {};
        self->ss = {};
    }
};

template <>
struct equals<TPMRegs> {
    bool operator()(const TPMRegs& self, const TPMRegs& other) const {
        return self.di == other.di && self.si == other.si && self.bp == other.bp && self.dummy == other.dummy &&
            self.bx == other.bx && self.dx == other.dx && self.cx == other.cx && self.ax == other.ax &&
            self.flags == other.flags && self.es == other.es && self.ds == other.ds && self.fs == other.fs &&
            self.gs == other.gs && self.ip == other.ip && self.cs == other.cs && self.sp == other.sp &&
            self.ss == other.ss;
    }
};

}  // namespace tv

namespace std {
template <>
struct hash<TPMRegs> {
    std::size_t operator()(const TPMRegs& self) const noexcept {
        std::size_t x{};
        tv::combineHash(self.di, &x);
        tv::combineHash(self.si, &x);
        tv::combineHash(self.bp, &x);
        tv::combineHash(self.dummy, &x);
        tv::combineHash(self.bx, &x);
        tv::combineHash(self.dx, &x);
        tv::combineHash(self.cx, &x);
        tv::combineHash(self.ax, &x);
        tv::combineHash(self.flags, &x);
        tv::combineHash(self.es, &x);
        tv::combineHash(self.ds, &x);
        tv::combineHash(self.fs, &x);
        tv::combineHash(self.gs, &x);
        tv::combineHash(self.ip, &x);
        tv::combineHash(self.cs, &x);
        tv::combineHash(self.sp, &x);
        tv::combineHash(self.ss, &x);
        return x;
    }
};
}  // namespace std
