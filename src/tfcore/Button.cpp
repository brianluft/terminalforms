#include "Button.h"

#define Uses_TRect
#define Uses_TButton
#include <tvision/tv.h>
#include <tvision/dialogs.h>

namespace tf {

Button::Button() : TButton(TRect(2, 2, 12, 4), "Button", 0, bfNormal) {}

void Button::press() {
    TButton::press();
    clickEventHandler();
}

void Button::setClickEventHandler(EventHandlerFunction function, void* userData) {
    clickEventHandler = EventHandler(function, userData);
}

BOOL Button::getIsDefault() const {
    return (flags & bfDefault) != 0;
}

void Button::setIsDefault(BOOL value) {
    flags &= ~bfDefault;
    makeDefault(value);
    drawView();
}

int32_t Button::getTextAlign() const {
    return (flags & bfLeftJust) != 0 ? 1 : 0;
}

void Button::setTextAlign(int32_t value) {
    if (value == 1) {
        flags |= bfLeftJust;
    } else {
        flags &= ~bfLeftJust;
    }
    drawView();
}

BOOL Button::getGrabsFocus() const {
    return (flags & bfGrabFocus) != 0;
}

void Button::setGrabsFocus(BOOL value) {
    if (value) {
        flags |= bfGrabFocus;
    } else {
        flags &= ~bfGrabFocus;
    }
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

TF_EXPORT tf::Error TfButtonGetIsDefault(tf::Button* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getIsDefault();
    return tf::Success;
}

TF_EXPORT tf::Error TfButtonSetIsDefault(tf::Button* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setIsDefault(value);
    return tf::Success;
}

TF_EXPORT tf::Error TfButtonGetTextAlign(tf::Button* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getTextAlign();
    return tf::Success;
}

TF_EXPORT tf::Error TfButtonSetTextAlign(tf::Button* self, int32_t value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setTextAlign(value);
    return tf::Success;
}

TF_EXPORT tf::Error TfButtonGetGrabsFocus(tf::Button* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getGrabsFocus();
    return tf::Success;
}

TF_EXPORT tf::Error TfButtonSetGrabsFocus(tf::Button* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setGrabsFocus(value);
    return tf::Success;
}
