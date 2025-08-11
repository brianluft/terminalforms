#pragma once

#include "../common.h"
#include "../Error.h"

#define Uses_TPoint
#include <tvision/tv.h>

EXPORT tv::Error TV_Point_placementSize(int32_t* outSize, int32_t* outAlignment);
EXPORT tv::Error TV_Point_placementNew(TPoint* self);
EXPORT tv::Error TV_Point_placementDelete(TPoint* self);
EXPORT tv::Error TV_Point_new(TPoint** out);
EXPORT tv::Error TV_Point_delete(TPoint* self);
EXPORT tv::Error TV_Point_equals(TPoint* self, TPoint* other, BOOL* out);
EXPORT tv::Error TV_Point_hash(TPoint* self, int32_t* out);
EXPORT tv::Error TV_Point_operator_add_in_place(TPoint* self, TPoint* adder);
EXPORT tv::Error TV_Point_operator_subtract_in_place(TPoint* self, TPoint* subber);
EXPORT tv::Error TV_Point_operator_add(TPoint* one, TPoint* two, TPoint* dst);
EXPORT tv::Error TV_Point_operator_subtract(TPoint* one, TPoint* two, TPoint* dst);
EXPORT tv::Error TV_Point_get_x(TPoint* self, int32_t* out);
EXPORT tv::Error TV_Point_set_x(TPoint* self, int32_t x);
EXPORT tv::Error TV_Point_get_y(TPoint* self, int32_t* out);
EXPORT tv::Error TV_Point_set_y(TPoint* self, int32_t y);
