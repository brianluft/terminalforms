#include "Rect.h"

EXPORT TRect* Tv_Rect_new0() {
    return new TRect;
}

EXPORT TRect* Tv_Rect_new1(int32_t ax, int32_t ay, int32_t bx, int32_t by) {
    return new TRect(ax, ay, bx, by);
}

EXPORT TRect* Tv_Rect_new2(TPoint* p1, TPoint* p2) {
    return new TRect(*p1, *p2);
}

EXPORT void Tv_Rect_delete(TRect* self) {
    delete self;
}

EXPORT int32_t Tv_Rect_hash(TRect* self) {
    return hash(Tv_Point_hash(&self->a), Tv_Point_hash(&self->b));
}

EXPORT void Tv_Rect_move(TRect* self, int32_t aDX, int32_t aDY) {
    self->move(aDX, aDY);
}

EXPORT void Tv_Rect_grow(TRect* self, int32_t aDX, int32_t aDY) {
    self->grow(aDX, aDY);
}

EXPORT void Tv_Rect_intersect(TRect* self, TRect* r) {
    self->intersect(*r);
}

EXPORT void Tv_Rect_Union(TRect* self, TRect* r) {
    self->Union(*r);
}

EXPORT BOOL Tv_Rect_contains(TRect* self, TPoint* p) {
    return self->contains(*p);
}

EXPORT BOOL Tv_Rect_operator_equals(TRect* self, TRect* r) {
    return *self == *r;
}

EXPORT BOOL Tv_Rect_isEmpty(TRect* self) {
    return self->isEmpty();
}

EXPORT TPoint* Tv_Rect_get_a(TRect* self) {
    return new TPoint{ self->a };
}

EXPORT TPoint* Tv_Rect_get_b(TRect* self) {
    return new TPoint{ self->b };
}

EXPORT void Tv_Rect_set_a(TRect* self, TPoint* p) {
    self->a = *p;
}

EXPORT void Tv_Rect_set_b(TRect* self, TPoint* p) {
    self->b = *p;
}
