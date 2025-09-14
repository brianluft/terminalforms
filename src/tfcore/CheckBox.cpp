#include "CheckBox.h"

#define Uses_TRect
#define Uses_TCheckBoxes
#define Uses_TSItem
#define Uses_TStringCollection
#include <tvision/tv.h>
#include <tvision/dialogs.h>

namespace tf {

CheckBox::CheckBox() : TCheckBoxes(TRect(2, 2, 12, 4), new TSItem("CheckBox", nullptr)) {}

void CheckBox::press(int32_t item) {
    TCheckBoxes::press(item);
    stateChangedEventHandler();
}

void CheckBox::setStateChangedEventHandler(EventHandlerFunction function, void* userData) {
    stateChangedEventHandler = EventHandler(function, userData);
}

BOOL CheckBox::getChecked() const {
    return (value & 1) != 0;
}

void CheckBox::setChecked(BOOL value) {
    if (value) {
        this->value |= 1;
    } else {
        this->value &= ~1;
    }
    drawView();
}

BOOL CheckBox::getEnabled() const {
    return (state & sfDisabled) == 0;
}

void CheckBox::setEnabled(BOOL value) {
    if (value) {
        state &= ~sfDisabled;
    } else {
        state |= sfDisabled;
    }
    drawView();
}

const char* CheckBox::getText() const {
    if (strings && strings->getCount() > 0) {
        return static_cast<const char*>(strings->at(0));
    }
    return "";
}

void CheckBox::setText(const char* text) {
    if (strings) {
        if (strings->getCount() > 0) {
            // Replace the existing string
            strings->atFree(0);
            strings->atInsert(0, newStr(text));
        } else {
            // Insert new string
            strings->atInsert(0, newStr(text));
        }
        drawView();
    }
}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(CheckBox)

TF_BOILERPLATE_FUNCTIONS(CheckBox)

TF_EXPORT tf::Error TfCheckBoxSetText(tf::CheckBox* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setText(text);
    return tf::Success;
}

TF_EXPORT tf::Error TfCheckBoxGetText(tf::CheckBox* self, const char** out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getText();
    return tf::Success;
}

TF_EXPORT tf::Error TfCheckBoxSetChecked(tf::CheckBox* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setChecked(value);
    return tf::Success;
}

TF_EXPORT tf::Error TfCheckBoxGetChecked(tf::CheckBox* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getChecked();
    return tf::Success;
}

TF_EXPORT tf::Error TfCheckBoxSetEnabled(tf::CheckBox* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setEnabled(value);
    return tf::Success;
}

TF_EXPORT tf::Error TfCheckBoxGetEnabled(tf::CheckBox* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getEnabled();
    return tf::Success;
}

TF_EXPORT tf::Error TfCheckBoxSetStateChangedEventHandler(
    tf::CheckBox* self,
    tf::EventHandlerFunction function,
    void* userData) {
    if (self == nullptr || function == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setStateChangedEventHandler(function, userData);
    return tf::Success;
}
