#include "Button.h"

#define Uses_TRect
#include <tvision/tv.h>

namespace tf {

Button::Button() : TButton(TRect(2, 2, 12, 4), "Button", 0, bfNormal) {}

void Button::press() {
    TButton::press();
    clickEventHandler();
}

void Button::setClickEventHandler(EventHandlerFunction function, void* userData) {
    clickEventHandler = EventHandler(function, userData);
}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(Button)

TF_BOILERPLATE_FUNCTIONS(Button)

TF_EXPORT tf::Error TfButtonSetText(tf::Button* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }

    delete[] self->title;
    self->title = newStr(text);
    self->drawView();
    return tf::Success;
}

TF_EXPORT tf::Error TfButtonGetText(tf::Button* self, const char** out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->title;
    return tf::Success;
}

TF_EXPORT tf::Error TfButtonSetClickEventHandler(tf::Button* self, tf::EventHandlerFunction function, void* userData) {
    if (self == nullptr || function == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setClickEventHandler(function, userData);
    return tf::Success;
}
