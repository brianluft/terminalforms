#include "Point.h"

EXPORT TPoint* TPoint_new0() {
    return new TPoint;
}

EXPORT TPoint* TPoint_new1(int32_t x, int32_t y) {
    TPoint* self = new TPoint{ x, y };
    return self;
}

EXPORT void TPoint_delete(TPoint* self) {
    delete self;
}

EXPORT int32_t TPoint_hash(TPoint* self) {
    return tv::hash(self->x, self->y);
}

EXPORT void TPoint_operator_add_in_place(TPoint* self, TPoint* adder) {
    *self += *adder;
}

EXPORT void TPoint_operator_subtract_in_place(TPoint* self, TPoint* subber) {
    *self -= *subber;
}

EXPORT TPoint* TPoint_operator_add(TPoint* one, TPoint* two) {
    return new TPoint{ *one + *two };
}

EXPORT TPoint* TPoint_operator_subtract(TPoint* one, TPoint* two) {
    return new TPoint{ *one - *two };
}

EXPORT BOOL TPoint_operator_equals(TPoint* one, TPoint* two) {
    return *one == *two;
}

EXPORT int32_t TPoint_get_x(TPoint* self) {
    return self->x;
}

EXPORT void TPoint_set_x(TPoint* self, int32_t x) {
    self->x = x;
}

EXPORT int32_t TPoint_get_y(TPoint* self) {
    return self->y;
}

EXPORT void TPoint_set_y(TPoint* self, int32_t y) {
    self->y = y;
}
