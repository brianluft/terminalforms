#include "TTimerQueue.h"

TV_DEFAULT_CONSTRUCTOR(TTimerQueue)
TV_BOILERPLATE_FUNCTIONS(TTimerQueue)

// Not bound: TTimerQueue(TTimePoint (&getTimeMs)()) noexcept;
// Not bound: void collectExpiredTimers(void (&func)(TTimerId, void *), void *args);
// These seem to be used internally in the tvision library and not intended for public use, so we skip them.

EXPORT tv::Error TV_TTimerQueue_setTimer(TTimerQueue* self, uint32_t timeoutMs, int32_t periodMs, void** out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->setTimer(timeoutMs, periodMs);
    return tv::Success;
}

EXPORT tv::Error TV_TTimerQueue_killTimer(TTimerQueue* self, void* id) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    self->killTimer(id);
    return tv::Success;
}

EXPORT tv::Error TV_TTimerQueue_timeUntilNextTimeout(TTimerQueue* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->timeUntilNextTimeout();
    return tv::Success;
}
