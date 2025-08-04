#include "MessageEvent.h"

EXPORT tv::Error TV_MessageEvent_new(MessageEvent** out) {
    return tv::checkedNew(out);
}

EXPORT tv::Error TV_MessageEvent_delete(MessageEvent* self) {
    return tv::checkedDelete(self);
}

EXPORT tv::Error TV_MessageEvent_equals(MessageEvent* self, MessageEvent* other, BOOL* out) {
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

    if (self->command != other->command || self->infoPtr != other->infoPtr) {
        *out = FALSE;
        return tv::Success;
    }

    *out = TRUE;
    return tv::Success;
}

EXPORT tv::Error TV_MessageEvent_hash(MessageEvent* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    int32_t result = 0;
    tv::hash(self->command, &result);
    tv::hash(self->infoPtr, &result);

    *out = result;
    return tv::Success;
}

EXPORT tv::Error TV_MessageEvent_get_command(MessageEvent* self, uint16_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->command;
    return tv::Success;
}

EXPORT tv::Error TV_MessageEvent_set_command(MessageEvent* self, uint16_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->command = value;
    return tv::Success;
}

EXPORT tv::Error TV_MessageEvent_get_infoPtr(MessageEvent* self, void** out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->infoPtr;
    return tv::Success;
}

EXPORT tv::Error TV_MessageEvent_set_infoPtr(MessageEvent* self, void* value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->infoPtr = value;
    return tv::Success;
}
