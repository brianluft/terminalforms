#pragma once

#include "common.h"
#include "EventHandler.h"

#define Uses_TRadioButtons
#define Uses_TSItem
#define Uses_TStringCollection
#include <tvision/tv.h>

namespace tf {

class RadioButtonGroup : public TRadioButtons {
   public:
    RadioButtonGroup();

    virtual void press(int32_t item) override;
    virtual void movedTo(int32_t item) override;

    void setSelectedIndexChangedEventHandler(EventHandlerFunction function, void* userData);

    // Selection management
    int32_t getSelectedIndex() const;
    void setSelectedIndex(int32_t index);

    // Items management
    int32_t getItemCount() const;
    const char* getItemAt(int32_t index) const;
    void setItemAt(int32_t index, const char* text);
    void addItem(const char* text);
    void insertItemAt(int32_t index, const char* text);
    void removeItemAt(int32_t index);
    void clearItems();

   private:
    void fireEventIfChanged(int32_t oldIndex, int32_t newIndex);
    EventHandler selectedIndexChangedEventHandler{};
    int32_t lastFiredIndex{ 0 };
};

template <>
struct equals<RadioButtonGroup> {
    bool operator()(const RadioButtonGroup& self, const RadioButtonGroup& other) const { return &self == &other; }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::RadioButtonGroup> {
    std::size_t operator()(const tf::RadioButtonGroup& p) const noexcept {
        std::size_t x{};
        tf::combineHash(&p, &x);
        return x;
    }
};
}  // namespace std
