#pragma once

#include "common.h"
#include "Error.h"

#define Uses_TEvent
#include <tvision/tv.h>

EXPORT tv::Error TV_MessageEvent_new(MessageEvent** out);
EXPORT tv::Error TV_MessageEvent_delete(MessageEvent* self);
EXPORT tv::Error TV_MessageEvent_equals(MessageEvent* self, MessageEvent* other, BOOL* out);
EXPORT tv::Error TV_MessageEvent_hash(MessageEvent* self, int32_t* out);
EXPORT tv::Error TV_MessageEvent_get_command(MessageEvent* self, uint16_t* out);
EXPORT tv::Error TV_MessageEvent_set_command(MessageEvent* self, uint16_t value);
EXPORT tv::Error TV_MessageEvent_get_infoPtr(MessageEvent* self, void** out);
EXPORT tv::Error TV_MessageEvent_set_infoPtr(MessageEvent* self, void* value);
