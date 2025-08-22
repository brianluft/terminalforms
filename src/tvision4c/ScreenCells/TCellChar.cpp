#include "TCellChar.h"

TV_BOILERPLATE_FUNCTIONS(TCellChar)

// Constants
EXPORT tv::Error TV_TCellChar_get_fWide(uint8_t* out) {
    try {
        *out = TCellChar::fWide;
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}

EXPORT tv::Error TV_TCellChar_get_fTrail(uint8_t* out) {
    try {
        *out = TCellChar::fTrail;
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}

// Member accessors
TV_GET_SET_PRIMITIVE(TCellChar, uint8_t, _textLength)
TV_GET_SET_PRIMITIVE(TCellChar, uint8_t, _flags)

// Text buffer access (read-only for safety)
EXPORT tv::Error TV_TCellChar_getText(void* self, char* buffer, int32_t bufferSize, int32_t* actualLength) {
    try {
        if (!self || !buffer || !actualLength)
            return tv::Error_ArgumentNull;
        if (bufferSize <= 0)
            return tv::Error_ArgumentOutOfRange;

        auto* obj = static_cast<TCellChar*>(self);
        auto text = obj->getText();
        *actualLength = static_cast<int32_t>(text.size());

        if (bufferSize < *actualLength) {
            return tv::Error_BufferTooSmall;
        }

        memcpy(buffer, text.data(), text.size());
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}

EXPORT tv::Error TV_TCellChar_size(void* self, int32_t* out) {
    try {
        if (!self || !out)
            return tv::Error_ArgumentNull;
        auto* obj = static_cast<TCellChar*>(self);
        *out = static_cast<int32_t>(obj->size());
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}

// Methods
EXPORT tv::Error TV_TCellChar_moveChar(void* self, char ch) {
    try {
        if (!self)
            return tv::Error_ArgumentNull;
        auto* obj = static_cast<TCellChar*>(self);
        obj->moveChar(ch);
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}

EXPORT tv::Error TV_TCellChar_moveWideCharTrail(void* self) {
    try {
        if (!self)
            return tv::Error_ArgumentNull;
        auto* obj = static_cast<TCellChar*>(self);
        obj->moveWideCharTrail();
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}

EXPORT tv::Error TV_TCellChar_isWide(void* self, BOOL* out) {
    try {
        if (!self || !out)
            return tv::Error_ArgumentNull;
        auto* obj = static_cast<TCellChar*>(self);
        *out = obj->isWide() ? TRUE : FALSE;
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}

EXPORT tv::Error TV_TCellChar_isWideCharTrail(void* self, BOOL* out) {
    try {
        if (!self || !out)
            return tv::Error_ArgumentNull;
        auto* obj = static_cast<TCellChar*>(self);
        *out = obj->isWideCharTrail() ? TRUE : FALSE;
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}

// Array indexer access
EXPORT tv::Error TV_TCellChar_getAt(void* self, int32_t index, char* out) {
    try {
        if (!self || !out)
            return tv::Error_ArgumentNull;
        if (index < 0 || index >= 15)
            return tv::Error_ArgumentOutOfRange;

        auto* obj = static_cast<TCellChar*>(self);
        *out = (*obj)[index];
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}

EXPORT tv::Error TV_TCellChar_setAt(void* self, int32_t index, char value) {
    try {
        if (!self)
            return tv::Error_ArgumentNull;
        if (index < 0 || index >= 15)
            return tv::Error_ArgumentOutOfRange;

        auto* obj = static_cast<TCellChar*>(self);
        (*obj)[index] = value;
        return tv::Success;
    } catch (...) {
        return tv::Error_Unknown;
    }
}
