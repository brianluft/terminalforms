#include "TTimerQueue.h"

EXPORT tv::Error TV_TTimerQueue_placementSize(int32_t* outSize, int32_t* outAlignment) {
    return tv::checkedSize<TTimerQueue>(outSize, outAlignment);
}

EXPORT tv::Error TV_TTimerQueue_placementNew(TTimerQueue* self) {
    return tv::checkedPlacementNew(self);
}

EXPORT tv::Error TV_TTimerQueue_placementDelete(TTimerQueue* self) {
    return tv::checkedPlacementDelete(self);
}

EXPORT tv::Error TV_TTimerQueue_new(TTimerQueue** out) {
    return tv::checkedNew(out);
}

EXPORT tv::Error TV_TTimerQueue_delete(TTimerQueue* self) {
    return tv::checkedDelete(self);
}

EXPORT tv::Error TV_TTimerQueue_equals(TTimerQueue* self, TTimerQueue* other, BOOL* out) {
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

    // For stateful objects like TTimerQueue, we use reference equality
    *out = (self == other) ? TRUE : FALSE;
    return tv::Success;
}

EXPORT tv::Error TV_TTimerQueue_hash(TTimerQueue* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    // For stateful objects like TTimerQueue, we use the memory address for hash
    int32_t result = 0;
    tv::hash(reinterpret_cast<uintptr_t>(self), &result);

    *out = result;
    return tv::Success;
}
