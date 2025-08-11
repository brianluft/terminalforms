#include "Point.h"

EXPORT tv::Error TV_Point_placementSize(int32_t* outSize, int32_t* outAlignment) {
    return tv::checkedSize<TPoint>(outSize, outAlignment);
}

EXPORT tv::Error TV_Point_placementNew(TPoint* self) {
    return tv::checkedPlacementNew(self);
}

EXPORT tv::Error TV_Point_placementDelete(TPoint* self) {
    return tv::checkedPlacementDelete(self);
}

EXPORT tv::Error TV_Point_new(TPoint** out) {
    return tv::checkedNew(out);
}

EXPORT tv::Error TV_Point_delete(TPoint* self) {
    return tv::checkedDelete(self);
}

EXPORT tv::Error TV_Point_equals(TPoint* self, TPoint* other, BOOL* out) {
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

EXPORT tv::Error TV_Point_hash(TPoint* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    int32_t result = 0;
    tv::hash(self->x, &result);
    tv::hash(self->y, &result);

    *out = result;
    return tv::Success;
}

EXPORT tv::Error TV_Point_operator_add_in_place(TPoint* self, TPoint* adder) {
    if (!self || !adder) {
        return tv::Error_ArgumentNull;
    }

    *self += *adder;
    return tv::Success;
}

EXPORT tv::Error TV_Point_operator_subtract_in_place(TPoint* self, TPoint* subber) {
    if (!self || !subber) {
        return tv::Error_ArgumentNull;
    }

    *self -= *subber;
    return tv::Success;
}

EXPORT tv::Error TV_Point_operator_add(TPoint* one, TPoint* two, TPoint* dst) {
    if (!one || !two || !dst) {
        return tv::Error_ArgumentNull;
    }

    *dst = *one + *two;
    return tv::Success;
}

EXPORT tv::Error TV_Point_operator_subtract(TPoint* one, TPoint* two, TPoint* dst) {
    if (!one || !two || !dst) {
        return tv::Error_ArgumentNull;
    }

    *dst = *one - *two;
    return tv::Success;
}

EXPORT tv::Error TV_Point_get_x(TPoint* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->x;
    return tv::Success;
}

EXPORT tv::Error TV_Point_set_x(TPoint* self, int32_t x) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->x = x;
    return tv::Success;
}

EXPORT tv::Error TV_Point_get_y(TPoint* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->y;
    return tv::Success;
}

EXPORT tv::Error TV_Point_set_y(TPoint* self, int32_t y) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->y = y;
    return tv::Success;
}
