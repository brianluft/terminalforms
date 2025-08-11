#pragma once

#include "common.h"
#include "Point.h"

#define Uses_TRect
#include <tvision/tv.h>

EXPORT tv::Error TV_Rect_placementSize(int32_t* outSize, int32_t* outAlignment);
EXPORT tv::Error TV_Rect_placementNew(TRect* self);
EXPORT tv::Error TV_Rect_placementDelete(TRect* self);
EXPORT tv::Error TV_Rect_new(TRect** out);
EXPORT tv::Error TV_Rect_delete(TRect* self);
EXPORT tv::Error TV_Rect_equals(TRect* self, TRect* other, BOOL* out);
EXPORT tv::Error TV_Rect_hash(TRect* self, int32_t* out);
EXPORT tv::Error TV_Rect_move(TRect* self, int32_t aDX, int32_t aDY);
EXPORT tv::Error TV_Rect_grow(TRect* self, int32_t aDX, int32_t aDY);
EXPORT tv::Error TV_Rect_intersect(TRect* self, TRect* r);
EXPORT tv::Error TV_Rect_Union(TRect* self, TRect* r);
EXPORT tv::Error TV_Rect_contains(TRect* self, TPoint* p, BOOL* out);
EXPORT tv::Error TV_Rect_isEmpty(TRect* self, BOOL* out);
EXPORT tv::Error TV_Rect_get_a(TRect* self, TPoint* dst);
EXPORT tv::Error TV_Rect_set_a(TRect* self, TPoint* src);
EXPORT tv::Error TV_Rect_get_b(TRect* self, TPoint* dst);
EXPORT tv::Error TV_Rect_set_b(TRect* self, TPoint* src);
