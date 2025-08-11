#include "WriteArgs.h"

EXPORT tv::Error TV_WriteArgs_placementSize(int32_t* outSize, int32_t* outAlignment) {
    return tv::checkedSize<write_args>(outSize, outAlignment);
}

EXPORT tv::Error TV_WriteArgs_placementNew(write_args* self) {
    return tv::checkedPlacementNew(self);
}

EXPORT tv::Error TV_WriteArgs_placementDelete(write_args* self) {
    return tv::checkedPlacementDelete(self);
}

EXPORT tv::Error TV_WriteArgs_new(write_args** out) {
    return tv::checkedNew<write_args>(out);
}

EXPORT tv::Error TV_WriteArgs_delete(write_args* self) {
    return tv::checkedDelete(self);
}

EXPORT tv::Error TV_WriteArgs_equals(write_args* self, write_args* other, BOOL* out) {
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

    if (self->self == other->self && self->target == other->target && self->buf == other->buf &&
        self->offset == other->offset) {
        *out = TRUE;
        return tv::Success;
    }

    *out = FALSE;
    return tv::Success;
}

EXPORT tv::Error TV_WriteArgs_hash(write_args* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    int32_t result = 0;
    tv::hash(reinterpret_cast<uintptr_t>(self->self), &result);
    tv::hash(reinterpret_cast<uintptr_t>(self->target), &result);
    tv::hash(reinterpret_cast<uintptr_t>(self->buf), &result);
    tv::hash(self->offset, &result);

    *out = result;
    return tv::Success;
}

EXPORT tv::Error TV_WriteArgs_get_self(write_args* self, void** out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->self;
    return tv::Success;
}

EXPORT tv::Error TV_WriteArgs_set_self(write_args* self, void* value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->self = value;
    return tv::Success;
}

EXPORT tv::Error TV_WriteArgs_get_target(write_args* self, void** out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->target;
    return tv::Success;
}

EXPORT tv::Error TV_WriteArgs_set_target(write_args* self, void* value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->target = value;
    return tv::Success;
}

EXPORT tv::Error TV_WriteArgs_get_buf(write_args* self, void** out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->buf;
    return tv::Success;
}

EXPORT tv::Error TV_WriteArgs_set_buf(write_args* self, void* value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->buf = value;
    return tv::Success;
}

EXPORT tv::Error TV_WriteArgs_get_offset(write_args* self, uint16_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->offset;
    return tv::Success;
}

EXPORT tv::Error TV_WriteArgs_set_offset(write_args* self, uint16_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->offset = value;
    return tv::Success;
}
