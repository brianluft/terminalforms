#include "tvision4c.h"
#include <array>

namespace tv {

void VirtualMethodTable::set(VirtualMethod virtualMethod, void* functionPointer) {
    methods_[static_cast<size_t>(virtualMethod)] = functionPointer;
}

void* VirtualMethodTable::get(VirtualMethod virtualMethod) const {
    return methods_[static_cast<size_t>(virtualMethod)];
}

VirtualMethodTable virtualMethods;

}  // namespace tv

EXPORT int32_t TvHealthCheck() {
    // The C# side will verify this value.
    return 123;
}

EXPORT void TvOverrideMethod(tv::Type type, tv::VirtualMethod virtualMethod, void* functionPointer) {
    tv::virtualMethods.set(virtualMethod, functionPointer);
}
