#pragma once

#include "../common.h"
#include "../Error.h"

#define Uses_TTimerQueue
#include <tvision/tv.h>

namespace tv {}  // namespace tv

EXPORT tv::Error TV_TTimerQueue_placementSize(int32_t* outSize, int32_t* outAlignment);
EXPORT tv::Error TV_TTimerQueue_placementNew(TTimerQueue* self);
EXPORT tv::Error TV_TTimerQueue_placementDelete(TTimerQueue* self);
EXPORT tv::Error TV_TTimerQueue_new(TTimerQueue** out);
EXPORT tv::Error TV_TTimerQueue_delete(TTimerQueue* self);
EXPORT tv::Error TV_TTimerQueue_equals(TTimerQueue* self, TTimerQueue* other, BOOL* out);
EXPORT tv::Error TV_TTimerQueue_hash(TTimerQueue* self, int32_t* out);
