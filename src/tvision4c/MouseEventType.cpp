#include "MouseEventType.h"
#include "Point.h"

EXPORT tv::Error TV_MouseEventType_new(MouseEventType** out) {
    return tv::checkedNew(out);
}

EXPORT tv::Error TV_MouseEventType_delete(MouseEventType* self) {
    return tv::checkedDelete(self);
}

EXPORT tv::Error TV_MouseEventType_equals(MouseEventType* self, MouseEventType* other, BOOL* out) {
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

    if (self->where == other->where && self->eventFlags == other->eventFlags &&
        self->controlKeyState == other->controlKeyState && self->buttons == other->buttons &&
        self->wheel == other->wheel) {
        *out = TRUE;
        return tv::Success;
    }

    *out = FALSE;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_hash(MouseEventType* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    int32_t result = 0;

    {
        int32_t pointHash = 0;
        auto error = TV_Point_hash(&self->where, &pointHash);
        if (error != tv::Success) {
            return error;
        }
    }

    tv::hash(self->eventFlags, &result);
    tv::hash(self->controlKeyState, &result);
    tv::hash(self->buttons, &result);
    tv::hash(self->wheel, &result);

    *out = result;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_get_where(MouseEventType* self, TPoint** out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = new TPoint{ self->where };
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_set_where(MouseEventType* self, TPoint* value) {
    if (!self || !value) {
        return tv::Error_ArgumentNull;
    }

    self->where = *value;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_get_eventFlags(MouseEventType* self, uint16_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->eventFlags;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_set_eventFlags(MouseEventType* self, uint16_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->eventFlags = value;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_get_controlKeyState(MouseEventType* self, uint16_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->controlKeyState;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_set_controlKeyState(MouseEventType* self, uint16_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->controlKeyState = value;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_get_buttons(MouseEventType* self, uint8_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->buttons;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_set_buttons(MouseEventType* self, uint8_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->buttons = value;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_get_wheel(MouseEventType* self, uint8_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->wheel;
    return tv::Success;
}

EXPORT tv::Error TV_MouseEventType_set_wheel(MouseEventType* self, uint8_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->wheel = value;
    return tv::Success;
}
