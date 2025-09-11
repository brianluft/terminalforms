#include "common.h"

#define Uses_TGroup
#define Uses_TView
#include <tvision/tv.h>

EXPORT tf::Error TfControlCollectionInsert(void* groupPtr, void* controlPtr) {
    if (groupPtr == nullptr || controlPtr == nullptr) {
        return tf::Error_ArgumentNull;
    }

    auto* group = static_cast<TGroup*>(groupPtr);
    auto* control = static_cast<TView*>(controlPtr);

    group->insert(control);
    return tf::Success;
}

EXPORT tf::Error TfControlCollectionInsertAt(void* groupPtr, int32_t index, void* controlPtr) {
    if (groupPtr == nullptr || controlPtr == nullptr) {
        return tf::Error_ArgumentNull;
    }

    if (index < 0) {
        return tf::Error_InvalidArgument;
    }

    auto* group = static_cast<TGroup*>(groupPtr);
    auto* control = static_cast<TView*>(controlPtr);

    // If index is 0, insert at beginning (before first)
    if (index == 0) {
        TView* first = group->first();
        group->insertBefore(control, first);
    } else {
        // Get the view at the target index to insert before
        TView* target = group->at(static_cast<short>(index));
        if (target == nullptr) {
            // Index is at the end, just insert normally
            group->insert(control);
        } else {
            group->insertBefore(control, target);
        }
    }

    return tf::Success;
}

EXPORT tf::Error TfControlCollectionRemoveAt(void* groupPtr, int32_t index) {
    if (groupPtr == nullptr) {
        return tf::Error_ArgumentNull;
    }

    if (index < 0) {
        return tf::Error_InvalidArgument;
    }

    auto* group = static_cast<TGroup*>(groupPtr);

    // Get the view at the specified index
    TView* viewToRemove = group->at(static_cast<short>(index));
    if (viewToRemove == nullptr) {
        return tf::Error_InvalidArgument;
    }

    group->remove(viewToRemove);
    return tf::Success;
}
