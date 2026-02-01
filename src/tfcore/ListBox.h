#pragma once

#include "common.h"
#include "EventHandler.h"

#define Uses_TListBox
#define Uses_TScrollBar
#define Uses_TStringCollection
#include <tvision/tv.h>

namespace tf {

class ListBox : public TListBox {
   public:
    ListBox();
    virtual ~ListBox();

    // Override to intercept selection events
    virtual void selectItem(short item) override;
    virtual void focusItem(short item) override;

    // Event handlers
    void setSelectedIndexChangedEventHandler(EventHandlerFunction function, void* userData);
    void setItemActivatedEventHandler(EventHandlerFunction function, void* userData);

    // Selection management
    int32_t getSelectedIndex() const;
    void setSelectedIndex(int32_t index);
    void clearSelection();

    // Items management
    int32_t getItemCount() const;
    const char* getItemAt(int32_t index) const;
    void setItemAt(int32_t index, const char* text);
    void addItem(const char* text);
    void insertItemAt(int32_t index, const char* text);
    void removeItemAt(int32_t index);
    void clearItems();

   private:
    void fireSelectedIndexChangedIfNeeded(int32_t oldIndex, int32_t newIndex);
    void updateRange();

    TScrollBar* ownedScrollBar;
    TStringCollection* stringItems;
    EventHandler selectedIndexChangedEventHandler{};
    EventHandler itemActivatedEventHandler{};
    int32_t lastFiredIndex{ -1 };
};

template <>
struct equals<ListBox> {
    bool operator()(const ListBox& self, const ListBox& other) const { return &self == &other; }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::ListBox> {
    std::size_t operator()(const tf::ListBox& p) const noexcept {
        std::size_t x{};
        tf::combineHash(&p, &x);
        return x;
    }
};
}  // namespace std
