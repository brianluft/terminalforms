#include "TextBox.h"

#define Uses_TRect
#define Uses_TInputLine
#define Uses_TEvent
#include <tvision/tv.h>

#include <algorithm>
#include <cstring>

namespace tf {

TextBox::TextBox() : TInputLine(TRect(0, 0, 20, 1), 256, nullptr, ilMaxBytes), previousText(data ? data : "") {}

void TextBox::handleEvent(TEvent& event) {
    // Save state before processing
    previousText = data;

    // Let TInputLine process the event
    TInputLine::handleEvent(event);

    // Detect text changes
    if (previousText != data) {
        textChangedEventHandler();
    }
}

void TextBox::setTextChangedEventHandler(EventHandlerFunction function, void* userData) {
    textChangedEventHandler = EventHandler(function, userData);
}

const char* TextBox::getText() const {
    return data ? data : "";
}

void TextBox::setText(const char* text) {
    if (!text)
        text = "";

    // TInputLine data buffer is maxLen+1 bytes
    int32_t len = std::min(static_cast<int32_t>(strlen(text)), maxLen);
    strncpy(data, text, len);
    data[len] = '\0';

    // Reset selection and cursor
    curPos = len;
    selStart = selEnd = 0;
    firstPos = 0;

    drawView();

    // Fire TextChanged if different from previous
    if (previousText != data) {
        previousText = data;
        textChangedEventHandler();
    }
}

int32_t TextBox::getMaxLength() const {
    return maxLen;
}

int32_t TextBox::clampIndex(int32_t index) const {
    int32_t textLen = static_cast<int32_t>(strlen(data));
    return std::max(0, std::min(index, textLen));
}

int32_t TextBox::getSelectionStart() const {
    // TInputLine may have selStart > selEnd after certain operations.
    // Normalize to the smaller value.
    return std::min(selStart, selEnd);
}

void TextBox::setSelectionStart(int32_t value) {
    int32_t start = clampIndex(value);
    int32_t length = getSelectionLength();
    selectRange(start, length);
}

int32_t TextBox::getSelectionLength() const {
    return std::abs(selEnd - selStart);
}

void TextBox::setSelectionLength(int32_t value) {
    int32_t start = getSelectionStart();
    int32_t length = std::max(0, value);
    selectRange(start, length);
}

void TextBox::getSelectedText(char* buffer, int32_t bufferSize) const {
    if (!buffer || bufferSize <= 0)
        return;

    int32_t start = getSelectionStart();
    int32_t length = getSelectionLength();

    if (length == 0 || !data) {
        buffer[0] = '\0';
        return;
    }

    int32_t copyLen = std::min(length, bufferSize - 1);
    strncpy(buffer, data + start, copyLen);
    buffer[copyLen] = '\0';
}

void TextBox::setSelectedText(const char* text) {
    if (!text)
        text = "";

    int32_t start = getSelectionStart();
    int32_t length = getSelectionLength();

    // Delete current selection
    if (length > 0) {
        int32_t textLen = static_cast<int32_t>(strlen(data));
        memmove(data + start, data + start + length, textLen - start - length + 1);
    }

    // Insert new text at selection start
    int32_t insertLen = static_cast<int32_t>(strlen(text));
    int32_t currentLen = static_cast<int32_t>(strlen(data));
    int32_t available = maxLen - currentLen;
    insertLen = std::min(insertLen, available);

    if (insertLen > 0) {
        memmove(data + start + insertLen, data + start, currentLen - start + 1);
        memcpy(data + start, text, insertLen);
    }

    // Position cursor after inserted text
    curPos = start + insertLen;
    selStart = selEnd = 0;

    drawView();

    // Fire TextChanged
    if (previousText != data) {
        previousText = data;
        textChangedEventHandler();
    }
}

void TextBox::selectRange(int32_t start, int32_t length) {
    int32_t clampedStart = clampIndex(start);
    int32_t textLen = static_cast<int32_t>(strlen(data));
    int32_t clampedLength = std::max(0, std::min(length, textLen - clampedStart));

    selStart = clampedStart;
    selEnd = clampedStart + clampedLength;
    curPos = selEnd;

    drawView();
}

void TextBox::selectAllText() {
    selectAll(True);
}

void TextBox::clearText() {
    setText("");
}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(TextBox)

TF_BOILERPLATE_FUNCTIONS(TextBox)

// Text property
TF_EXPORT tf::Error TfTextBoxGetText(tf::TextBox* self, const char** out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getText();
    return tf::Success;
}

TF_EXPORT tf::Error TfTextBoxSetText(tf::TextBox* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setText(text);
    return tf::Success;
}

// MaxLength property (readonly)
TF_EXPORT tf::Error TfTextBoxGetMaxLength(tf::TextBox* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getMaxLength();
    return tf::Success;
}

// Selection properties
TF_EXPORT tf::Error TfTextBoxGetSelectionStart(tf::TextBox* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getSelectionStart();
    return tf::Success;
}

TF_EXPORT tf::Error TfTextBoxSetSelectionStart(tf::TextBox* self, int32_t value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setSelectionStart(value);
    return tf::Success;
}

TF_EXPORT tf::Error TfTextBoxGetSelectionLength(tf::TextBox* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getSelectionLength();
    return tf::Success;
}

TF_EXPORT tf::Error TfTextBoxSetSelectionLength(tf::TextBox* self, int32_t value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setSelectionLength(value);
    return tf::Success;
}

// SelectedText property
TF_EXPORT tf::Error TfTextBoxGetSelectedText(tf::TextBox* self, char* buffer, int32_t bufferSize) {
    if (self == nullptr || buffer == nullptr) {
        return tf::Error_ArgumentNull;
    }
    if (bufferSize <= 0) {
        return tf::Error_InvalidArgument;
    }
    self->getSelectedText(buffer, bufferSize);
    return tf::Success;
}

TF_EXPORT tf::Error TfTextBoxSetSelectedText(tf::TextBox* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setSelectedText(text);
    return tf::Success;
}

// Methods
TF_EXPORT tf::Error TfTextBoxSelect(tf::TextBox* self, int32_t start, int32_t length) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->selectRange(start, length);
    return tf::Success;
}

TF_EXPORT tf::Error TfTextBoxSelectAll(tf::TextBox* self) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->selectAllText();
    return tf::Success;
}

TF_EXPORT tf::Error TfTextBoxClear(tf::TextBox* self) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->clearText();
    return tf::Success;
}

// Event handler setup
TF_EXPORT tf::Error TfTextBoxSetTextChangedEventHandler(
    tf::TextBox* self,
    tf::EventHandlerFunction function,
    void* userData) {
    if (self == nullptr || function == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setTextChangedEventHandler(function, userData);
    return tf::Success;
}
