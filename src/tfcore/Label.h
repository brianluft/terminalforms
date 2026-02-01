#pragma once

#include "common.h"

#define Uses_TLabel
#include <tvision/tv.h>

namespace tf {

class Label : public TLabel {
   public:
    Label();
    Label(const TRect& bounds, TStringView text);

    virtual void handleEvent(TEvent& event) override;

    // Text property methods
    using TStaticText::getText;  // Bring base class overload into scope
    const char* getText() const;
    void setText(const char* text);

    // UseMnemonic property methods
    BOOL getUseMnemonic() const;
    void setUseMnemonic(BOOL value);

   private:
    BOOL useMnemonic = 1;  // Default to true like Windows Forms
};

template <>
struct equals<Label> {
    bool operator()(const Label& self, const Label& other) const { return &self == &other; }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::Label> {
    std::size_t operator()(const tf::Label& p) const noexcept {
        std::size_t x{};
        tf::combineHash(&p, &x);
        return x;
    }
};
}  // namespace std
