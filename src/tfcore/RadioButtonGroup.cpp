#include "RadioButtonGroup.h"

#define Uses_TRect
#define Uses_TRadioButtons
#define Uses_TSItem
#define Uses_TStringCollection
#include <tvision/tv.h>
#include <tvision/dialogs.h>

namespace tf {

RadioButtonGroup::RadioButtonGroup() : TRadioButtons(TRect(2, 2, 22, 4), new TSItem("Option 1", nullptr)) {
    // TCluster constructor creates TStringCollection with delta=0, which cannot grow.
    // We need to replace it with a growable collection.
    auto* oldStrings = strings;
    auto* newStrings = new TStringCollection(10, 10);  // Initial capacity 10, grow by 10

    // Copy items from old to new collection
    for (ccIndex i = 0; i < oldStrings->getCount(); i++) {
        newStrings->atInsert(i, newStr(static_cast<const char*>(oldStrings->at(i))));
    }

    // Replace and cleanup
    strings = newStrings;
    destroy(static_cast<TCollection*>(oldStrings));

    // value starts at 0 (first item selected)
}

void RadioButtonGroup::fireEventIfChanged(int32_t oldIndex, int32_t newIndex) {
    if (oldIndex != newIndex) {
        lastFiredIndex = newIndex;
        selectedIndexChangedEventHandler();
    }
}

void RadioButtonGroup::press(int32_t item) {
    int32_t oldIndex = getSelectedIndex();
    TRadioButtons::press(item);
    fireEventIfChanged(oldIndex, getSelectedIndex());
}

void RadioButtonGroup::movedTo(int32_t item) {
    int32_t oldIndex = getSelectedIndex();
    TRadioButtons::movedTo(item);
    fireEventIfChanged(oldIndex, getSelectedIndex());
}

void RadioButtonGroup::setSelectedIndexChangedEventHandler(EventHandlerFunction function, void* userData) {
    selectedIndexChangedEventHandler = EventHandler(function, userData);
}

int32_t RadioButtonGroup::getSelectedIndex() const {
    return static_cast<int32_t>(value);
}

void RadioButtonGroup::setSelectedIndex(int32_t index) {
    int32_t oldIndex = getSelectedIndex();
    value = static_cast<uint32_t>(index);
    sel = index;
    drawView();
    if (oldIndex != index && lastFiredIndex != index) {
        lastFiredIndex = index;
        selectedIndexChangedEventHandler();
    }
}

int32_t RadioButtonGroup::getItemCount() const {
    return strings ? static_cast<int32_t>(strings->getCount()) : 0;
}

const char* RadioButtonGroup::getItemAt(int32_t index) const {
    if (strings && index >= 0 && index < static_cast<int32_t>(strings->getCount())) {
        return static_cast<const char*>(strings->at(index));
    }
    return nullptr;
}

void RadioButtonGroup::setItemAt(int32_t index, const char* text) {
    if (strings && index >= 0 && index < static_cast<int32_t>(strings->getCount())) {
        strings->atFree(index);
        strings->atInsert(index, newStr(text));
        drawView();
    }
}

void RadioButtonGroup::addItem(const char* text) {
    if (strings) {
        strings->atInsert(strings->getCount(), newStr(text));
        drawView();
    }
}

void RadioButtonGroup::insertItemAt(int32_t index, const char* text) {
    if (strings && index >= 0 && index <= static_cast<int32_t>(strings->getCount())) {
        strings->atInsert(index, newStr(text));
        // Adjust selection if inserting before or at current selection
        if (index <= static_cast<int32_t>(value)) {
            value++;
            sel = static_cast<int32_t>(value);
        }
        drawView();
    }
}

void RadioButtonGroup::removeItemAt(int32_t index) {
    if (strings && index >= 0 && index < static_cast<int32_t>(strings->getCount())) {
        strings->atFree(index);
        int32_t count = static_cast<int32_t>(strings->getCount());

        // Adjust selection
        if (count == 0) {
            value = 0;
            sel = 0;
        } else if (static_cast<int32_t>(value) == index) {
            // Selected item was removed, select previous or first
            if (index >= count) {
                value = static_cast<uint32_t>(count - 1);
            }
            sel = static_cast<int32_t>(value);
            lastFiredIndex = static_cast<int32_t>(value);
            selectedIndexChangedEventHandler();
        } else if (static_cast<int32_t>(value) > index) {
            // Selection was after removed item, adjust index
            value--;
            sel = static_cast<int32_t>(value);
        }
        drawView();
    }
}

void RadioButtonGroup::clearItems() {
    if (strings) {
        int32_t oldIndex = getSelectedIndex();
        while (strings->getCount() > 0) {
            strings->atFree(0);
        }
        value = 0;
        sel = 0;
        drawView();
        if (oldIndex != 0) {
            lastFiredIndex = 0;
            selectedIndexChangedEventHandler();
        }
    }
}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(RadioButtonGroup)

TF_BOILERPLATE_FUNCTIONS(RadioButtonGroup)

TF_EXPORT tf::Error TfRadioButtonGroupSetSelectedIndexChangedEventHandler(
    tf::RadioButtonGroup* self,
    tf::EventHandlerFunction function,
    void* userData) {
    if (self == nullptr || function == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->setSelectedIndexChangedEventHandler(function, userData);
    return tf::Success;
}

TF_EXPORT tf::Error TfRadioButtonGroupGetSelectedIndex(tf::RadioButtonGroup* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getSelectedIndex();
    return tf::Success;
}

TF_EXPORT tf::Error TfRadioButtonGroupSetSelectedIndex(tf::RadioButtonGroup* self, int32_t index) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    int32_t count = self->getItemCount();
    if (index < 0 || index >= count) {
        return tf::Error_InvalidArgument;
    }
    self->setSelectedIndex(index);
    return tf::Success;
}

TF_EXPORT tf::Error TfRadioButtonGroupGetItemCount(tf::RadioButtonGroup* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }
    *out = self->getItemCount();
    return tf::Success;
}

TF_EXPORT tf::Error TfRadioButtonGroupGetItemAt(tf::RadioButtonGroup* self, int32_t index, const char** out) {
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

TF_EXPORT tf::Error TfRadioButtonGroupSetItemAt(tf::RadioButtonGroup* self, int32_t index, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    if (index < 0 || index >= self->getItemCount()) {
        return tf::Error_InvalidArgument;
    }
    self->setItemAt(index, text);
    return tf::Success;
}

TF_EXPORT tf::Error TfRadioButtonGroupAddItem(tf::RadioButtonGroup* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->addItem(text);
    return tf::Success;
}

TF_EXPORT tf::Error TfRadioButtonGroupInsertItemAt(tf::RadioButtonGroup* self, int32_t index, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }
    if (index < 0 || index > self->getItemCount()) {
        return tf::Error_InvalidArgument;
    }
    self->insertItemAt(index, text);
    return tf::Success;
}

TF_EXPORT tf::Error TfRadioButtonGroupRemoveItemAt(tf::RadioButtonGroup* self, int32_t index) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    if (index < 0 || index >= self->getItemCount()) {
        return tf::Error_InvalidArgument;
    }
    self->removeItemAt(index);
    return tf::Success;
}

TF_EXPORT tf::Error TfRadioButtonGroupClearItems(tf::RadioButtonGroup* self) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }
    self->clearItems();
    return tf::Success;
}
