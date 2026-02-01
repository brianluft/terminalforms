#pragma once

#include "common.h"
#include "EventHandler.h"

#define Uses_TCheckBoxes
#define Uses_TSItem
#include <tvision/tv.h>

namespace tf {

class CheckBox : public TCheckBoxes {
   public:
    CheckBox();

    virtual void press(int32_t item) override;

    void setStateChangedEventHandler(EventHandlerFunction function, void* userData);

    // Property management methods
    BOOL getChecked() const;
    void setChecked(BOOL value);
    const char* getText() const;
    void setText(const char* text);

   private:
    EventHandler stateChangedEventHandler{};
};

template <>
struct equals<CheckBox> {
    bool operator()(const CheckBox& self, const CheckBox& other) const { return &self == &other; }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::CheckBox> {
    std::size_t operator()(const tf::CheckBox& p) const noexcept {
        std::size_t x{};
        tf::combineHash(&p, &x);
        return x;
    }
};
}  // namespace std
