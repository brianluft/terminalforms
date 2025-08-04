#include "KeyDownEvent.h"
#include "Error.h"
#include "Point.h"
#include <cstring>

EXPORT tv::Error TV_KeyDownEvent_new(KeyDownEvent** out) {
    return tv::checkedNew(out);
}

EXPORT tv::Error TV_KeyDownEvent_delete(KeyDownEvent* self) {
    return tv::checkedDelete(self);
}

EXPORT tv::Error TV_KeyDownEvent_equals(KeyDownEvent* self, KeyDownEvent* other, BOOL* out) {
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

    if (self->keyCode != other->keyCode || self->controlKeyState != other->controlKeyState ||
        self->textLength != other->textLength) {
        *out = FALSE;
        return tv::Success;
    }

    // Compare text content up to textLength
    if (self->textLength > 0) {
        *out = std::memcmp(self->text, other->text, self->textLength) == 0;
        return tv::Success;
    }

    *out = TRUE;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_hash(KeyDownEvent* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    // Hash the keyCode, controlKeyState, and textLength
    int32_t result = 0;
    tv::hash(self->keyCode, &result);
    tv::hash(self->controlKeyState, &result);
    tv::hash(self->textLength, &result);

    // Include text content in hash if present
    if (self->textLength > 0) {
        for (uint8_t i = 0; i < self->textLength; i++) {
            tv::hash((uint8_t)self->text[i], &result);
        }
    }

    *out = result;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_get_keyCode(KeyDownEvent* self, uint16_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->keyCode;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_set_keyCode(KeyDownEvent* self, uint16_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->keyCode = value;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_get_charCode(KeyDownEvent* self, uint8_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->charScan.charCode;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_set_charCode(KeyDownEvent* self, uint8_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->charScan.charCode = value;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_get_scanCode(KeyDownEvent* self, uint8_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->charScan.scanCode;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_set_scanCode(KeyDownEvent* self, uint8_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->charScan.scanCode = value;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_get_controlKeyState(KeyDownEvent* self, uint16_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->controlKeyState;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_set_controlKeyState(KeyDownEvent* self, uint16_t value) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->controlKeyState = value;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_get_text(KeyDownEvent* self, char** out, uint8_t* outTextLength) {
    if (!self || !out || !outTextLength) {
        return tv::Error_ArgumentNull;
    }

    *outTextLength = self->textLength;
    *out = self->text;
    return tv::Success;
}

EXPORT tv::Error TV_KeyDownEvent_set_text(KeyDownEvent* self, char* value, uint8_t textLength) {
    if (!self || (!value && textLength > 0)) {
        return tv::Error_ArgumentNull;
    }

    if (textLength > maxCharSize) {
        return tv::Error_BufferTooSmall;
    }

    self->textLength = textLength;
    memset(self->text, 0, maxCharSize);

    if (textLength > 0) {
        memcpy(self->text, value, textLength);
    }

    return tv::Success;
}
