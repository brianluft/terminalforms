#pragma once

#include "common.h"
#include "MouseEventType.h"
#include "KeyDownEvent.h"
#include "MessageEvent.h"

#define Uses_TEvent
#include <tvision/tv.h>

EXPORT tv::Error TV_Event_new(TEvent** out);
EXPORT tv::Error TV_Event_delete(TEvent* self);
EXPORT tv::Error TV_Event_equals(TEvent* self, TEvent* other, BOOL* out);
EXPORT tv::Error TV_Event_hash(TEvent* self, int32_t* out);
EXPORT tv::Error TV_Event_get_what(TEvent* self, uint16_t* out);
EXPORT tv::Error TV_Event_set_what(TEvent* self, uint16_t value);
EXPORT tv::Error TV_Event_get_mouse(TEvent* self, MouseEventType** out);
EXPORT tv::Error TV_Event_set_mouse(TEvent* self, MouseEventType* value);
EXPORT tv::Error TV_Event_get_keyDown(TEvent* self, KeyDownEvent** out);
EXPORT tv::Error TV_Event_set_keyDown(TEvent* self, KeyDownEvent* value);
EXPORT tv::Error TV_Event_get_message(TEvent* self, MessageEvent** out);
EXPORT tv::Error TV_Event_set_message(TEvent* self, MessageEvent* value);
EXPORT tv::Error TV_Event_getMouseEvent(TEvent* self);
EXPORT tv::Error TV_Event_getKeyEvent(TEvent* self);
