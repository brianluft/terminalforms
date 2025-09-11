#include "common.h"
#include "Rectangle.h"

#define Uses_TView
#include <tvision/tv.h>

EXPORT tf::Error TfControlGetBounds(TView* view, tf::Rectangle* out) {
    if (view == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = tf::Rectangle(view->getBounds());
    return tf::Success;
}

EXPORT tf::Error TfControlSetBounds(TView* view, const tf::Rectangle* value) {
    if (view == nullptr || value == nullptr) {
        return tf::Error_ArgumentNull;
    }

    TRect bounds = value->toTRect();
    view->locate(bounds);
    return tf::Success;
}
