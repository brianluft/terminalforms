#include "TClipboard.h"

EXPORT tv::Error TV_TClipboard_setText(const char* text) {
    if (!text) {
        return tv::Error_ArgumentNull;
    }

    TClipboard::setText(text);
    return tv::Success;
}

EXPORT tv::Error TV_TClipboard_requestText() {
    TClipboard::requestText();
    return tv::Success;
}
