#pragma once

#include <cstdint>
#include <functional>

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

template <typename T>
void hash(const T& v, int32_t* seed) {
    auto x = *seed;
    x ^= std::hash<T>{}(v) + 0x9e3779b9 + (x << 6) + (x >> 2);
    *seed = x;
}

}  // namespace tv

EXPORT int32_t TV_healthCheck();
