#pragma once

#include "../common.h"
#include "../Error.h"

#define Uses_TEvent
#include <tvision/tv.h>

EXPORT tv::Error TV_KeyDownEvent_placementSize(int32_t* outSize, int32_t* outAlignment);
EXPORT tv::Error TV_KeyDownEvent_placementNew(KeyDownEvent* self);
EXPORT tv::Error TV_KeyDownEvent_placementDelete(KeyDownEvent* self);
EXPORT tv::Error TV_KeyDownEvent_new(KeyDownEvent** out);
EXPORT tv::Error TV_KeyDownEvent_delete(KeyDownEvent* self);
EXPORT tv::Error TV_KeyDownEvent_equals(KeyDownEvent* self, KeyDownEvent* other, BOOL* out);
EXPORT tv::Error TV_KeyDownEvent_hash(KeyDownEvent* self, int32_t* out);
EXPORT tv::Error TV_KeyDownEvent_get_keyCode(KeyDownEvent* self, uint16_t* out);
EXPORT tv::Error TV_KeyDownEvent_set_keyCode(KeyDownEvent* self, uint16_t value);
EXPORT tv::Error TV_KeyDownEvent_get_charCode(KeyDownEvent* self, uint8_t* out);
EXPORT tv::Error TV_KeyDownEvent_set_charCode(KeyDownEvent* self, uint8_t value);
EXPORT tv::Error TV_KeyDownEvent_get_scanCode(KeyDownEvent* self, uint8_t* out);
EXPORT tv::Error TV_KeyDownEvent_set_scanCode(KeyDownEvent* self, uint8_t value);
EXPORT tv::Error TV_KeyDownEvent_get_controlKeyState(KeyDownEvent* self, uint16_t* out);
EXPORT tv::Error TV_KeyDownEvent_set_controlKeyState(KeyDownEvent* self, uint16_t value);
EXPORT tv::Error TV_KeyDownEvent_get_text(KeyDownEvent* self, char** out, uint8_t* outTextLength);
EXPORT tv::Error TV_KeyDownEvent_set_text(KeyDownEvent* self, char* value, uint8_t textLength);
