#include "Rectangle.h"

namespace tf {

Rectangle::Rectangle(const TRect& rect)
    : x(rect.a.x), y(rect.a.y), width(rect.b.x - rect.a.x), height(rect.b.y - rect.a.y) {}

TRect Rectangle::toTRect() const {
    return TRect(x, y, x + width, y + height);
}

}  // namespace tf

TF_BOILERPLATE_FUNCTIONS(Rectangle)
