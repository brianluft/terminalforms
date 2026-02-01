#include "ListBox.h"

#define Uses_TRect
#define Uses_TListBox
#define Uses_TScrollBar
#define Uses_TStringCollection
#include <tvision/tv.h>

namespace tf {

ListBox::ListBox() : TListBox(TRect(2, 2, 22, 8), 1, nullptr), ownedScrollBar(nullptr), stringItems(nullptr) {
    // Create a vertical scrollbar on the right edge
    TRect scrollBarBounds(size.x - 1, 0, size.x, size.y);
    ownedScrollBar = new TScrollBar(scrollBarBounds);

    // Connect scrollbar to this list box
    vScrollBar = ownedScrollBar;

    // Initialize scrollbar step values
    short pgStep = size.y - 1;
    short arStep = 1;
    vScrollBar->setStep(pgStep, arStep);

    // Create empty string collection
    stringItems = new TStringCollection(10, 5);
    items = stringItems;

    // Start with no selection
    focused = -1;
    range = 0;
}

ListBox::~ListBox() {
    // TListBox::~TListBox does not delete items, so we must
    // Note: vScrollBar is not owned by TListBox either
    delete ownedScrollBar;
    ownedScrollBar = nullptr;
    vScrollBar = nullptr;

    // stringItems is pointed to by items, delete via stringItems
    delete stringItems;
    stringItems = nullptr;
    items = nullptr;
}

void ListBox::selectItem(short item) {
    // selectItem is called on double-click or Enter/Space
    // This fires the ItemActivated event
    TListBox::selectItem(item);
    itemActivatedEventHandler();
}

void ListBox::focusItem(short item) {
    int32_t oldIndex = getSelectedIndex();
    TListBox::focusItem(item);
    int32_t newIndex = getSelectedIndex();
    fireSelectedIndexChangedIfNeeded(oldIndex, newIndex);
}

void ListBox::fireSelectedIndexChangedIfNeeded(int32_t oldIndex, int32_t newIndex) {
    if (oldIndex != newIndex && lastFiredIndex != newIndex) {
        lastFiredIndex = newIndex;
        selectedIndexChangedEventHandler();
    }
}

void ListBox::setSelectedIndexChangedEventHandler(EventHandlerFunction function, void* userData) {
    selectedIndexChangedEventHandler = EventHandler(function, userData);
}

void ListBox::setItemActivatedEventHandler(EventHandlerFunction function, void* userData) {
    itemActivatedEventHandler = EventHandler(function, userData);
}

int32_t ListBox::getSelectedIndex() const {
    if (range <= 0) {
        return -1;
    }
    return static_cast<int32_t>(focused);
}

void ListBox::setSelectedIndex(int32_t index) {
    if (range <= 0) {
        // No items, can only be -1
        return;
    }

    int32_t oldIndex = getSelectedIndex();

    if (index < 0) {
        // Clear selection - but TListViewer always has a focused item
        // We'll set focused to 0 but track "no selection" via -1 semantics
        // Actually, TListViewer needs focused >= 0, so we keep focus but report -1
        // For simplicity, we'll just clamp to valid range
        index = 0;
    } else if (index >= range) {
        index = range - 1;
    }

    focused = static_cast<short>(index);
    if (vScrollBar != nullptr) {
        vScrollBar->setValue(index);
    }
    drawView();

    fireSelectedIndexChangedIfNeeded(oldIndex, index);
}

void ListBox::clearSelection() {
    // TListViewer always has a focused item when range > 0
    // We can set focused to -1 if range is 0, but otherwise
    // we need to keep a valid focus. For "clear selection",
    // we'll just fire the event if transitioning.
    // Note: After discussion, TListViewer doesn't really support "no selection"
    // when items exist. We'll keep focus at 0 but could track separately.
    // For now, clearSelection resets to first item if items exist.
    if (range > 0) {
        setSelectedIndex(0);
    }
}

int32_t ListBox::getItemCount() const {
    return stringItems ? static_cast<int32_t>(stringItems->getCount()) : 0;
}

const char* ListBox::getItemAt(int32_t index) const {
    if (stringItems && index >= 0 && index < static_cast<int32_t>(stringItems->getCount())) {
        return static_cast<const char*>(stringItems->at(index));
    }
    return nullptr;
}

void ListBox::setItemAt(int32_t index, const char* text) {
    if (stringItems && index >= 0 && index < static_cast<int32_t>(stringItems->getCount())) {
        stringItems->atFree(index);
        stringItems->atInsert(index, newStr(text));
        drawView();
    }
}

void ListBox::addItem(const char* text) {
    if (stringItems) {
        stringItems->atInsert(stringItems->getCount(), newStr(text));
        updateRange();
    }
}

void ListBox::insertItemAt(int32_t index, const char* text) {
    if (stringItems && index >= 0 && index <= static_cast<int32_t>(stringItems->getCount())) {
        int32_t oldIndex = getSelectedIndex();
        stringItems->atInsert(index, newStr(text));

        // Adjust selection if inserting before or at current selection
        if (oldIndex >= 0 && index <= oldIndex) {
            focused = static_cast<short>(oldIndex + 1);
            lastFiredIndex = focused;
        }

        updateRange();
    }
}

void ListBox::removeItemAt(int32_t index) {
    if (stringItems && index >= 0 && index < static_cast<int32_t>(stringItems->getCount())) {
        int32_t oldIndex = getSelectedIndex();
        stringItems->atFree(index);
        int32_t count = static_cast<int32_t>(stringItems->getCount());

        // Adjust selection
        if (count == 0) {
            focused = -1;
            lastFiredIndex = -1;
            if (oldIndex >= 0) {
                selectedIndexChangedEventHandler();
            }
        } else if (oldIndex == index) {
            // Selected item was removed
            if (index >= count) {
                focused = static_cast<short>(count - 1);
            }
            // Else keep same index (now points to next item)
            lastFiredIndex = focused;
            selectedIndexChangedEventHandler();
        } else if (oldIndex > index) {
            // Selection was after removed item, adjust index
            focused = static_cast<short>(oldIndex - 1);
        }

        updateRange();
    }
}

void ListBox::clearItems() {
    if (stringItems) {
        int32_t oldIndex = getSelectedIndex();
        while (stringItems->getCount() > 0) {
            stringItems->atFree(0);
        }
        focused = -1;
        range = 0;
        if (vScrollBar != nullptr) {
            vScrollBar->setParams(0, 0, 0, vScrollBar->pgStep, vScrollBar->arStep);
        }
        drawView();
        if (oldIndex >= 0) {
            lastFiredIndex = -1;
            selectedIndexChangedEventHandler();
        }
    }
}

void ListBox::updateRange() {
    int32_t count = getItemCount();
    range = static_cast<short>(count);

    // If we had no selection and now have items, select first and fire event
    if (focused < 0 && count > 0) {
        focused = 0;
        lastFiredIndex = 0;
        selectedIndexChangedEventHandler();
    }

    if (vScrollBar != nullptr) {
        vScrollBar->setParams(
            focused >= 0 ? focused : 0, 0, count > 0 ? count - 1 : 0, vScrollBar->pgStep, vScrollBar->arStep);
    }
    drawView();
}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(ListBox)

TF_BOILERPLATE_FUNCTIONS(ListBox)

TF_EXPORT tf::Error TfListBoxSetSelectedIndexChangedEventHandler(
    tf::ListBox* self,
    tf::EventHandlerFunction function,
    void* userData) {
    if (self == nullptr || function == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setSelectedIndexChangedEventHandler(function, userData);
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxSetItemActivatedEventHandler(
    tf::ListBox* self,
    tf::EventHandlerFunction function,
    void* userData) {
    if (self == nullptr || function == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setItemActivatedEventHandler(function, userData);
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxGetSelectedIndex(tf::ListBox* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getSelectedIndex();
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxSetSelectedIndex(tf::ListBox* self, int32_t index) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    int32_t count = self->getItemCount();
    if (count == 0) {
        // No items, only -1 is valid
        if (index != -1) {
            return tf::Error_InvalidArgument;
        }
        return tf::Success;
    }
    if (index < -1 || index >= count) {
        return tf::Error_InvalidArgument;
    }
    self->setSelectedIndex(index);
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxClearSelection(tf::ListBox* self) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->clearSelection();
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxGetItemCount(tf::ListBox* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getItemCount();
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxGetItemAt(tf::ListBox* self, int32_t index, const char** out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    if (index < 0 || index >= self->getItemCount()) {
        return tf::Error_InvalidArgument;
    }
    const char* item = self->getItemAt(index);
    if (item == nullptr) {
        return tf::Error_InvalidArgument;
    }
    *out = TF_STRDUP(item);
    if (*out == nullptr) {
        return tf::Error_OutOfMemory;
    }
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxSetItemAt(tf::ListBox* self, int32_t index, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    if (index < 0 || index >= self->getItemCount()) {
        return tf::Error_InvalidArgument;
    }
    self->setItemAt(index, text);
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxAddItem(tf::ListBox* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->addItem(text);
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxInsertItemAt(tf::ListBox* self, int32_t index, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    if (index < 0 || index > self->getItemCount()) {
        return tf::Error_InvalidArgument;
    }
    self->insertItemAt(index, text);
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxRemoveItemAt(tf::ListBox* self, int32_t index) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    if (index < 0 || index >= self->getItemCount()) {
        return tf::Error_InvalidArgument;
    }
    self->removeItemAt(index);
    return tf::Success;
}

TF_EXPORT tf::Error TfListBoxClearItems(tf::ListBox* self) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->clearItems();
    return tf::Success;
}
