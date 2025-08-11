#pragma once

#include "common.h"
#include "Error.h"

#define Uses_TEvent
#include <tvision/tv.h>

EXPORT tv::Error TV_MouseEventType_placementSize(int32_t* outSize, int32_t* outAlignment);
EXPORT tv::Error TV_MouseEventType_placementNew(MouseEventType* self);
EXPORT tv::Error TV_MouseEventType_placementDelete(MouseEventType* self);
EXPORT tv::Error TV_MouseEventType_new(MouseEventType** out);
EXPORT tv::Error TV_MouseEventType_delete(MouseEventType* self);
EXPORT tv::Error TV_MouseEventType_equals(MouseEventType* self, MouseEventType* other, BOOL* out);
EXPORT tv::Error TV_MouseEventType_hash(MouseEventType* self, int32_t* out);
EXPORT tv::Error TV_MouseEventType_get_where(MouseEventType* self, TPoint* dst);
EXPORT tv::Error TV_MouseEventType_set_where(MouseEventType* self, TPoint* src);
EXPORT tv::Error TV_MouseEventType_get_eventFlags(MouseEventType* self, uint16_t* out);
EXPORT tv::Error TV_MouseEventType_set_eventFlags(MouseEventType* self, uint16_t value);
EXPORT tv::Error TV_MouseEventType_get_controlKeyState(MouseEventType* self, uint16_t* out);
EXPORT tv::Error TV_MouseEventType_set_controlKeyState(MouseEventType* self, uint16_t value);
EXPORT tv::Error TV_MouseEventType_get_buttons(MouseEventType* self, uint8_t* out);
EXPORT tv::Error TV_MouseEventType_set_buttons(MouseEventType* self, uint8_t value);
EXPORT tv::Error TV_MouseEventType_get_wheel(MouseEventType* self, uint8_t* out);
EXPORT tv::Error TV_MouseEventType_set_wheel(MouseEventType* self, uint8_t value);
