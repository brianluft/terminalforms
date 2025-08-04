#pragma once

#include "tvision4c.h"

#define Uses_TPoint
#include <tvision/tv.h>

EXPORT TPoint* Tv_Point_new0();
EXPORT TPoint* Tv_Point_new1(int32_t x, int32_t y);
EXPORT void Tv_Point_delete(TPoint* self);
EXPORT int32_t Tv_Point_hash(TPoint* self);
EXPORT void Tv_Point_operator_add_in_place(TPoint* self, TPoint* adder);
EXPORT void Tv_Point_operator_subtract_in_place(TPoint* self, TPoint* subber);
EXPORT TPoint* Tv_Point_operator_add(TPoint* one, TPoint* two);
EXPORT TPoint* Tv_Point_operator_subtract(TPoint* one, TPoint* two);
EXPORT BOOL Tv_Point_operator_equals(TPoint* one, TPoint* two);
EXPORT int32_t Tv_Point_get_x(TPoint* self);
EXPORT void Tv_Point_set_x(TPoint* self, int32_t x);
EXPORT int32_t Tv_Point_get_y(TPoint* self);
EXPORT void Tv_Point_set_y(TPoint* self, int32_t y);
