#pragma once

#include "tvision4c.h"
#include "Point.h"

#define Uses_TRect
#include <tvision/tv.h>

EXPORT TRect* Tv_Rect_new0();
EXPORT TRect* Tv_Rect_new1(int32_t ax, int32_t ay, int32_t bx, int32_t by);
EXPORT TRect* Tv_Rect_new2(TPoint* p1, TPoint* p2);
EXPORT void Tv_Rect_delete(TRect* self);
EXPORT int32_t Tv_Rect_hash(TRect* self);
EXPORT void Tv_Rect_move(TRect* self, int32_t aDX, int32_t aDY);
EXPORT void Tv_Rect_grow(TRect* self, int32_t aDX, int32_t aDY);
EXPORT void Tv_Rect_intersect(TRect* self, TRect* r);
EXPORT void Tv_Rect_Union(TRect* self, TRect* r);
EXPORT BOOL Tv_Rect_contains(TRect* self, TPoint* p);
EXPORT BOOL Tv_Rect_operator_equals(TRect* self, TRect* r);
EXPORT BOOL Tv_Rect_isEmpty(TRect* self);
EXPORT TPoint* Tv_Rect_get_a(TRect* self);
EXPORT TPoint* Tv_Rect_get_b(TRect* self);
EXPORT void Tv_Rect_set_a(TRect* self, TPoint* p);
EXPORT void Tv_Rect_set_b(TRect* self, TPoint* p);
