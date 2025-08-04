#pragma once

#include "common.h"
#include "VirtualMethod.h"
#include <array>

namespace tv {

class VirtualMethodTable {
   public:
    void set(VirtualMethod virtualMethod, void* functionPointer);
    void* get(VirtualMethod virtualMethod) const;

   private:
    std::array<void*, VirtualMethod_Count> methods_;
};

extern VirtualMethodTable virtualMethods;

template <typename TFunctionPointer>
TFunctionPointer getVirtualMethod(VirtualMethod virtualMethod) {
    return reinterpret_cast<TFunctionPointer>(virtualMethods.get(virtualMethod));
}

}  // namespace tv

EXPORT void TV_overrideMethod(tv::VirtualMethod virtualMethod, void* functionPointer);
