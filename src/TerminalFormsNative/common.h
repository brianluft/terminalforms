#pragma once

#include <cstdint>
#include <functional>
#include <string>

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

typedef int32_t BOOL;

namespace tv {

// Matches `src\TerminalForms\Error.cs`
enum Error {
    Success = 0,
    Error_Unknown,
    Error_NativeInteropFailure,
    Error_OutOfMemory,
    Error_UnalignedObjectPlacement,
    Error_ArgumentNull,
    Error_ArgumentOutOfRange,
    Error_BufferTooSmall,

    Error_HasMessage = 0x8000,
};

// Thread-local storage for detailed error messages
extern thread_local std::string lastErrorMessage;

// This is a policy for initializing members of a type `T`.
// It is used by checkedNew and checkedPlacementNew to zero-initialize members that don't get initialized by the
// default constructor.
template <typename T>
struct initialize {
    void operator()(T*) const {
        // Do nothing by default.
    }
};

// This is a policy for comparing two objects of type `T`.
// It is used by checkedEquals to compare two objects.
// It assumes that &self != nullptr && &other != nullptr.
template <typename T>
struct equals {
    bool operator()(const T& self, const T& other) const {
        // Reference equality by default.
        return &self == &other;
    }
};

// Use this in your std::hash<T> specialization to combine the hash of a field with the running seed.
// Call it multiple times to build up the hash.
template <typename T>
void combineHash(const T& v, std::size_t* seed) noexcept {
    auto x = *seed;
    x ^= std::hash<T>{}(v) + 0x9e3779b9 + (x << 6) + (x >> 2);
    *seed = x;
}

// This templated function instantiates a new object of type `T`.
// It catches any exception and converts to `Error`.
// If constructor arguments are provided, initialize is skipped.
template <typename T, typename... Args>
Error checkedNew(T** out, Args&&... args) {
    if (!out) {
        return tv::Error_ArgumentNull;
    }

    try {
        *out = new T(std::forward<Args>(args)...);

        // Only call initialize if this was the default constructor (no args)
        if constexpr (sizeof...(args) == 0) {
            tv::initialize<T>{}(*out);
        }

        return tv::Success;
    } catch (const std::bad_alloc&) {
        return tv::Error_OutOfMemory;
    } catch (const std::exception& e) {
        lastErrorMessage = e.what();
        return static_cast<tv::Error>(Error_Unknown | Error_HasMessage);
    }
}

// This templated function deletes an object of type `T`.
// It catches any exception and converts to `Error`.
template <typename T>
Error checkedDelete(T* self) {
    if (!self) {
        return tv::Success;  // Not an error.
    }

    try {
        delete self;
        return tv::Success;
    } catch (const std::bad_alloc&) {
        return tv::Error_OutOfMemory;
    } catch (const std::exception& e) {
        lastErrorMessage = e.what();
        return static_cast<tv::Error>(Error_Unknown | Error_HasMessage);
    }
}

// This templated function compares two objects of type `T`.
// It catches any exception and converts to `Error`.
template <typename T>
Error checkedEquals(T* self, T* other, BOOL* out) {
    if (!out) {
        return tv::Error_ArgumentNull;
    }

    // Both null
    if (!self && !other) {
        *out = TRUE;
        return tv::Success;
    }

    // One null, one not
    if (!self || !other) {
        *out = FALSE;
        return tv::Success;
    }

    // Reference equality
    if (self == other) {
        *out = TRUE;
        return tv::Success;
    }

    try {
        // Call the policy to compare the objects which are known to be non-null and different pointers.
        *out = tv::equals<T>{}(*self, *other) ? TRUE : FALSE;
        return tv::Success;
    } catch (const std::exception& e) {
        lastErrorMessage = e.what();
        return static_cast<tv::Error>(Error_Unknown | Error_HasMessage);
    }
}

// This templated function hashes an object of type `T`.
// It catches any exception and converts to `Error`.
template <typename T>
Error checkedHash(T* self, int32_t* out) {
    if (!self || !out) {
        return tv::Error_ArgumentNull;
    }

    try {
        // Use std::hash to hash the object.
        std::size_t hash_value = std::hash<T>{}(*self);
        *out = static_cast<int32_t>(hash_value);
        return tv::Success;
    } catch (const std::exception& e) {
        lastErrorMessage = e.what();
        return static_cast<tv::Error>(Error_Unknown | Error_HasMessage);
    }
}

}  // namespace tv

// Use this macro if the class has a default parameterless constructor.
#define TV_DEFAULT_CONSTRUCTOR(type)               \
    EXPORT tv::Error TV_##type##_new(type** out) { \
        return tv::checkedNew(out);                \
    }

// Every non-static class/struct that is exported to C must have these functions.
#define TV_BOILERPLATE_FUNCTIONS(type)                                        \
    EXPORT tv::Error TV_##type##_delete(type* self) {                         \
        return tv::checkedDelete(self);                                       \
    }                                                                         \
    EXPORT tv::Error TV_##type##_equals(type* self, type* other, BOOL* out) { \
        return tv::checkedEquals(self, other, out);                           \
    }                                                                         \
    EXPORT tv::Error TV_##type##_hash(type* self, int32_t* out) {             \
        return tv::checkedHash(self, out);                                    \
    }

EXPORT int32_t TV_healthCheck();
EXPORT tv::Error TV_getLastErrorMessageLength(int32_t* out);
EXPORT tv::Error TV_getLastErrorMessage(char* buffer, int32_t bufferSize);
