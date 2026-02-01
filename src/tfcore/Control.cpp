#include "Control.h"
#include "Rectangle.h"

#define Uses_TView
#define Uses_TGroup
#include <tvision/tv.h>

namespace tf {

int32_t getViewIndex(TView* view) {
    if (view == nullptr || view->owner == nullptr) {
        return -1;
    }

    TGroup* owner = view->owner;
    TView* first = owner->first();
    if (first == nullptr) {
        return -1;
    }

    int32_t index = 0;
    TView* current = first;
    do {
        if (current == view) {
            return index;
        }
        index++;
        current = current->next;
    } while (current != first);

    return -1;
}

}  // namespace tf

// Bounds property (existing)
TF_EXPORT tf::Error TfControlGetBounds(TView* self, tf::Rectangle* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = tf::Rectangle(self->getBounds());
    return tf::Success;
}

TF_EXPORT tf::Error TfControlSetBounds(TView* self, const tf::Rectangle* value) {
    if (self == nullptr || value == nullptr) {
        return tf::Error_ArgumentNull;
    }

    TRect bounds = value->toTRect();
    self->locate(bounds);
    return tf::Success;
}

// Visible property (sfVisible state flag)
TF_EXPORT tf::Error TfControlGetVisible(TView* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = (self->state & sfVisible) != 0;
    return tf::Success;
}

TF_EXPORT tf::Error TfControlSetVisible(TView* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    // Use setState which handles visibility properly including drawing
    self->setState(sfVisible, value);
    return tf::Success;
}

// Enabled property (sfDisabled state flag, inverted)
TF_EXPORT tf::Error TfControlGetEnabled(TView* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    // Enabled is the inverse of sfDisabled
    *out = (self->state & sfDisabled) == 0;
    return tf::Success;
}

TF_EXPORT tf::Error TfControlSetEnabled(TView* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    // setState with sfDisabled is the inverse of Enabled
    self->setState(sfDisabled, !value);
    return tf::Success;
}

// Focused property (sfFocused state flag, read-only)
TF_EXPORT tf::Error TfControlGetFocused(TView* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = (self->state & sfFocused) != 0;
    return tf::Success;
}

// CanFocus property (ofSelectable option flag)
TF_EXPORT tf::Error TfControlGetCanFocus(TView* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = (self->options & ofSelectable) != 0;
    return tf::Success;
}

TF_EXPORT tf::Error TfControlSetCanFocus(TView* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    if (value) {
        self->options |= ofSelectable;
    } else {
        self->options &= ~ofSelectable;
    }
    return tf::Success;
}

// TabIndex property (get-only, returns Z-order position)
TF_EXPORT tf::Error TfControlGetTabIndex(TView* self, int32_t* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = tf::getViewIndex(self);
    return tf::Success;
}

// Parent property (returns owner TGroup, which is a TView*)
TF_EXPORT tf::Error TfControlGetParent(TView* self, TView** out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    // owner is TGroup* which inherits from TView*
    *out = self->owner;
    return tf::Success;
}

// Focus method - attempts to set focus to this control
TF_EXPORT tf::Error TfControlFocus(TView* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->focus();
    return tf::Success;
}

// ContainerControl methods - these work on TGroup*

// ActiveControl property - returns the currently focused child (TGroup::current)
TF_EXPORT tf::Error TfContainerControlGetActiveControl(TGroup* self, TView** out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->current;
    return tf::Success;
}

// SelectNextControl - moves focus to next/previous control
TF_EXPORT tf::Error TfContainerControlSelectNextControl(TGroup* self, BOOL forward, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->focusNext(forward);
    return tf::Success;
}
