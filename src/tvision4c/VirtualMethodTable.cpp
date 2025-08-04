#include "VirtualMethodTable.h"

namespace tv {

void VirtualMethodTable::set(VirtualMethod virtualMethod, void* functionPointer) {
    methods_[static_cast<size_t>(virtualMethod)] = functionPointer;
}

void* VirtualMethodTable::get(VirtualMethod virtualMethod) const {
    return methods_[static_cast<size_t>(virtualMethod)];
}

VirtualMethodTable virtualMethods;

}  // namespace tv

EXPORT void TV_overrideMethod(tv::VirtualMethod virtualMethod, void* functionPointer) {
    tv::virtualMethods.set(virtualMethod, functionPointer);
}
