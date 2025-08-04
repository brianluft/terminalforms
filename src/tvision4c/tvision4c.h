#pragma once

#include <array>
#include <cstdint>

#ifdef _WIN32
#define EXPORT extern "C" __declspec(dllexport)
#else
#define EXPORT extern "C"
#endif

#ifndef FALSE
#define FALSE 0
#endif

#ifndef TRUE
#define TRUE 1
#endif

typedef int BOOL;

namespace tv {

enum Type : int32_t {
    Type_Application = 0,
};

// Matches `src\TerminalForms\VirtualMethod.cs`
enum VirtualMethod : int32_t {
    // Application
    VirtualMethod_Application_destructor = 0,
    VirtualMethod_Application_suspend,

    VirtualMethod_Count,
};

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

template <typename T, typename... Rest>
void combineHashesInPlaceU64(std::size_t& seed, const T& v, const Rest&... rest) {
    seed ^= std::hash<T>{}(v) + 0x9e3779b9 + (seed << 6) + (seed >> 2);
    (combineHashesInPlaceU64(seed, rest), ...);
}

template <typename T, typename... Rest>
int32_t hash(const T& v, const Rest&... rest) {
    std::size_t seed = 0;
    combineHashesInPlaceU64(seed, v, rest...);
    return static_cast<int32_t>(seed);
}

}  // namespace tv

EXPORT int32_t TvHealthCheck();
EXPORT void TvOverrideMethod(tv::Type type, tv::VirtualMethod virtualMethod, void* functionPointer);
