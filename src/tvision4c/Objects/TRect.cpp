#include "TRect.h"
#include "../common.h"

TV_BOILERPLATE_FUNCTIONS(TRect)
TV_GET_SET_COPYABLE_OBJECT(TRect, TPoint, a)
TV_GET_SET_COPYABLE_OBJECT(TRect, TPoint, b)

EXPORT tv::Error TV_TRect_move(TRect* self, int32_t aDX, int32_t aDY) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->move(aDX, aDY);
    return tv::Success;
}

EXPORT tv::Error TV_TRect_grow(TRect* self, int32_t aDX, int32_t aDY) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->grow(aDX, aDY);
    return tv::Success;
}

EXPORT tv::Error TV_TRect_intersect(TRect* self, TRect* r) {
    if (!self || !r) {
        return tv::Error_ArgumentNull;
    }

    self->intersect(*r);
    return tv::Success;
}

EXPORT tv::Error TV_TRect_Union(TRect* self, TRect* r) {
    if (!self || !r) {
        return tv::Error_ArgumentNull;
    }

    self->Union(*r);
    return tv::Success;
}

EXPORT tv::Error TV_TRect_contains(TRect* self, TPoint* p, BOOL* out) {
    if (!self || !p || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->contains(*p);
    return tv::Success;
}

EXPORT tv::Error TV_TRect_isEmpty(TRect* self, BOOL* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->isEmpty();
    return tv::Success;
}
