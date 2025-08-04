#include "Rect.h"

EXPORT tv::Error TV_Rect_new(TRect** out) {
    return tv::checkedNew(out);
}

EXPORT tv::Error TV_Rect_new2(int32_t ax, int32_t ay, int32_t bx, int32_t by, TRect** out) {
    return tv::checkedNew(out);
}

EXPORT tv::Error TV_Rect_new3(TPoint* p1, TPoint* p2, TRect** out) {
    return tv::checkedNew(out);
}

EXPORT tv::Error TV_Rect_delete(TRect* self) {
    return tv::checkedDelete(self);
}

EXPORT tv::Error TV_Rect_equals(TRect* self, TRect* other, BOOL* out) {
    if (!out) {
        return tv::Error_ArgumentNull;
    }

    if (!self && !other) {
        *out = TRUE;
        return tv::Success;
    }

    if (!self || !other) {
        *out = FALSE;
        return tv::Success;
    }

    if (*self == *other) {
        *out = TRUE;
        return tv::Success;
    }

    *out = FALSE;
    return tv::Success;
}

EXPORT tv::Error TV_Rect_hash(TRect* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    int32_t result = 0;

    {
        int32_t pointHash = 0;
        auto error = TV_Point_hash(&self->a, &pointHash);
        if (error != tv::Success) {
            return error;
        }
    }

    {
        int32_t pointHash = 0;
        auto error = TV_Point_hash(&self->b, &pointHash);
        if (error != tv::Success) {
            return error;
        }
    }

    *out = result;
    return tv::Success;
}

EXPORT tv::Error TV_Rect_move(TRect* self, int32_t aDX, int32_t aDY) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->move(aDX, aDY);
    return tv::Success;
}

EXPORT tv::Error TV_Rect_grow(TRect* self, int32_t aDX, int32_t aDY) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->grow(aDX, aDY);
    return tv::Success;
}

EXPORT tv::Error TV_Rect_intersect(TRect* self, TRect* r) {
    if (!self || !r) {
        return tv::Error_ArgumentNull;
    }

    self->intersect(*r);
    return tv::Success;
}

EXPORT tv::Error TV_Rect_Union(TRect* self, TRect* r) {
    if (!self || !r) {
        return tv::Error_ArgumentNull;
    }

    self->Union(*r);
    return tv::Success;
}

EXPORT tv::Error TV_Rect_contains(TRect* self, TPoint* p, BOOL* out) {
    if (!self || !p || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->contains(*p);
    return tv::Success;
}

EXPORT tv::Error TV_Rect_isEmpty(TRect* self, BOOL* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->isEmpty();
    return tv::Success;
}

EXPORT tv::Error TV_Rect_get_a(TRect* self, TPoint** out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = new TPoint{ self->a };
    return tv::Success;
}

EXPORT tv::Error TV_Rect_get_b(TRect* self, TPoint** out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = new TPoint{ self->b };
    return tv::Success;
}

EXPORT tv::Error TV_Rect_set_a(TRect* self, TPoint* p) {
    if (!self || !p) {
        return tv::Error_ArgumentNull;
    }

    self->a = *p;
    return tv::Success;
}

EXPORT tv::Error TV_Rect_set_b(TRect* self, TPoint* p) {
    if (!self || !p) {
        return tv::Error_ArgumentNull;
    }

    self->b = *p;
    return tv::Success;
}
