#pragma once

#include "common.h"

#define Uses_TView
#define Uses_TGroup
#include <tvision/tv.h>

// Control.h provides utility functions for common control operations.
// This header doesn't define a tf::Control class because controls inherit from
// various Turbo Vision base classes (TButton, TCheckBoxes, TDialog, etc.).
// Instead, the TF_EXPORT functions in Control.cpp operate directly on TView*.

namespace tf {

// Utility function to get the Z-order index of a view within its owner.
// Returns -1 if the view has no owner.
int32_t getViewIndex(TView* view);

}  // namespace tf
