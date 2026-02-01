#pragma once

#include "common.h"
#include "EventHandler.h"

#define Uses_TInputLine
#define Uses_TEvent
#include <tvision/tv.h>

#include <string>

namespace tf {

/// Wraps TInputLine for single-line text input with change detection and selection support.
class TextBox : public TInputLine {
   public:
    TextBox();

    virtual void handleEvent(TEvent& event) override;

    void setTextChangedEventHandler(EventHandlerFunction function, void* userData);

    // Text property
    const char* getText() const;
    void setText(const char* text);

    // MaxLength property (readonly - TInputLine doesn't support dynamic resize)
    int32_t getMaxLength() const;

    // Selection properties
    int32_t getSelectionStart() const;
    void setSelectionStart(int32_t value);

    int32_t getSelectionLength() const;
    void setSelectionLength(int32_t value);

    // SelectedText property
    void getSelectedText(char* buffer, int32_t bufferSize) const;
    void setSelectedText(const char* text);

    // Methods
    void selectRange(int32_t start, int32_t length);
    void selectAllText();
    void clearText();

   private:
    EventHandler textChangedEventHandler{};
    std::string previousText;

    // Helper: Clamp index to valid range [0, textLength]
    int32_t clampIndex(int32_t index) const;
};

template <>
struct equals<TextBox> {
    bool operator()(const TextBox& self, const TextBox& other) const { return &self == &other; }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::TextBox> {
    std::size_t operator()(const tf::TextBox& p) const noexcept {
        std::size_t x{};
        tf::combineHash(&p, &x);
        return x;
    }
};
}  // namespace std
