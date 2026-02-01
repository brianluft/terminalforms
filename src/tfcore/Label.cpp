#include "Label.h"

#define Uses_TRect
#define Uses_TLabel
#define Uses_TStaticText
#define Uses_TGroup
#define Uses_TEvent
#define Uses_TInputLine  // For hotKey function
#include <tvision/tv.h>

#if !defined(__CTYPE_H)
#include <ctype.h>
#endif  // __CTYPE_H

namespace tf {

Label::Label() : TLabel(TRect(2, 2, 12, 3), "Label", nullptr) {}

Label::Label(const TRect& bounds, TStringView text) : TLabel(bounds, text, nullptr) {}

void Label::handleEvent(TEvent& event) {
    // Call TStaticText::handleEvent first to handle basic text display
    TStaticText::handleEvent(event);

    // Only process events if UseMnemonic is enabled
    if (!useMnemonic) {
        return;
    }

    // Handle mouse clicks - use Windows Forms behavior
    if (event.what == evMouseDown) {
        if (owner) {
            owner->selectNext(false);  // Select next control in tab order
        }
        clearEvent(event);
    }
    // Handle hotkey presses - use Windows Forms behavior
    else if (event.what == evKeyDown) {
        char c = hotKey(text);
        if (event.keyDown.keyCode != 0 &&
            (getAltCode(c) == event.keyDown.keyCode ||
             (c != 0 && owner->phase == TGroup::phPostProcess &&
              c == (char)toupper(event.keyDown.charScan.charCode)))) {
            if (owner) {
                owner->selectNext(false);  // Select next control in tab order
            }
            clearEvent(event);
        }
    }
    // Skip TLabel's broadcast event handling since we don't have a linked control
    // This prevents the label from trying to highlight based on non-existent link focus
}

const char* Label::getText() const {
    return text;
}

void Label::setText(const char* newText) {
    delete[] const_cast<char*>(text);
    const_cast<const char*&>(text) = newStr(newText);
    drawView();
}

BOOL Label::getUseMnemonic() const {
    return useMnemonic;
}

void Label::setUseMnemonic(BOOL value) {
    useMnemonic = value;
    if (value) {
        options |= ofPreProcess | ofPostProcess;
    } else {
        options &= ~(ofPreProcess | ofPostProcess);
    }
    drawView();
}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(Label)

TF_BOILERPLATE_FUNCTIONS(Label)

TF_EXPORT tf::Error TfLabelNew2(tf::Label** out, TRect* bounds, const char* text) {
    if (out == nullptr || bounds == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }

    return tf::checkedNew(out, *bounds, text);
}

TF_EXPORT tf::Error TfLabelSetText(tf::Label* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setText(text);
    return tf::Success;
}

TF_EXPORT tf::Error TfLabelGetText(tf::Label* self, const char** out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = TF_STRDUP(self->getText());
    if (*out == nullptr) {
        return tf::Error_OutOfMemory;
    }
    return tf::Success;
}

TF_EXPORT tf::Error TfLabelGetUseMnemonic(tf::Label* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getUseMnemonic();
    return tf::Success;
}

TF_EXPORT tf::Error TfLabelSetUseMnemonic(tf::Label* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setUseMnemonic(value);
    return tf::Success;
}
