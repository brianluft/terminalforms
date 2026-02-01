#pragma once

#include <cstdint>
#include <cstring>
#include <string>

#ifdef _WIN32
#define TF_EXPORT extern "C" __declspec(dllexport)
#define TF_CDECL __cdecl
#else
#define TF_EXPORT extern "C" __attribute__((visibility("default")))
#define TF_CDECL
#endif

#ifndef FALSE
#define FALSE 0
#endif

#ifndef TRUE
#define TRUE 1
#endif

typedef int32_t BOOL;

namespace tf {

// Matches `src\TerminalForms\Error.cs`
enum Error {
    Success = 0,
    Error_Unknown,
    Error_NativeInteropFailure,
    Error_OutOfMemory,
    Error_ArgumentNull,
    Error_InvalidArgument,

    Error_HasMessage = 0x8000,
};

// Thread-local storage for detailed error messages
void setLastErrorMessage(const std::string& message);

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

// Use TF_STRDUP when returning strings to C# via StringMarshalling.Utf8 with `out string`.
// The .NET marshaller will free the memory after copying to a managed string.
// This allocates with malloc(), which the marshaller expects.
#ifdef _WIN32
#define TF_STRDUP _strdup
#else
#define TF_STRDUP strdup
#endif

// This templated function instantiates a new object of type `T`.
// It catches any exception and converts to `Error`.
template <typename T, typename... Args>
Error checkedNew(T** out, Args&&... args) {
    if (!out) {
        return tf::Error_ArgumentNull;
    }

    try {
        *out = new T(std::forward<Args>(args)...);
        return tf::Success;
    } catch (const std::bad_alloc&) {
        return tf::Error_OutOfMemory;
    } catch (const std::exception& e) {
        setLastErrorMessage(e.what());
        return static_cast<tf::Error>(Error_Unknown | Error_HasMessage);
    }
}

// This templated function deletes an object of type `T`.
// It catches any exception and converts to `Error`.
template <typename T>
Error checkedDelete(T* self) {
    if (!self) {
        return tf::Success;  // Not an error.
    }

    try {
        delete self;
        return tf::Success;
    } catch (const std::bad_alloc&) {
        return tf::Error_OutOfMemory;
    } catch (const std::exception& e) {
        setLastErrorMessage(e.what());
        return static_cast<tf::Error>(Error_Unknown | Error_HasMessage);
    }
}

// This templated function compares two objects of type `T`.
// It catches any exception and converts to `Error`.
template <typename T>
Error checkedEquals(T* self, T* other, BOOL* out) {
    if (!out) {
        return tf::Error_ArgumentNull;
    }

    // Both null
    if (!self && !other) {
        *out = TRUE;
        return tf::Success;
    }

    // One null, one not
    if (!self || !other) {
        *out = FALSE;
        return tf::Success;
    }

    // Reference equality
    if (self == other) {
        *out = TRUE;
        return tf::Success;
    }

    try {
        // Call the policy to compare the objects which are known to be non-null and different pointers.
        *out = tf::equals<T>{}(*self, *other) ? TRUE : FALSE;
        return tf::Success;
    } catch (const std::exception& e) {
        setLastErrorMessage(e.what());
        return static_cast<tf::Error>(Error_Unknown | Error_HasMessage);
    }
}

// This templated function hashes an object of type `T`.
// It catches any exception and converts to `Error`.
template <typename T>
Error checkedHash(T* self, int32_t* out) {
    if (!self || !out) {
        return tf::Error_ArgumentNull;
    }

    try {
        // Use std::hash to hash the object.
        std::size_t hash_value = std::hash<T>{}(*self);
        *out = static_cast<int32_t>(hash_value);
        return tf::Success;
    } catch (const std::exception& e) {
        setLastErrorMessage(e.what());
        return static_cast<tf::Error>(Error_Unknown | Error_HasMessage);
    }
}

}  // namespace tf

// Use this macro if the class has a default parameterless constructor.
#define TF_DEFAULT_CONSTRUCTOR(type)                    \
    TF_EXPORT tf::Error Tf##type##New(tf::type** out) { \
        return tf::checkedNew(out);                     \
    }

// Every non-static class/struct that is exported to C must have these functions.
#define TF_BOILERPLATE_FUNCTIONS(type)                                                 \
    TF_EXPORT tf::Error Tf##type##Delete(tf::type* self) {                             \
        return tf::checkedDelete(self);                                                \
    }                                                                                  \
    TF_EXPORT tf::Error Tf##type##Equals(tf::type* self, tf::type* other, BOOL* out) { \
        return tf::checkedEquals(self, other, out);                                    \
    }                                                                                  \
    TF_EXPORT tf::Error Tf##type##Hash(tf::type* self, int32_t* out) {                 \
        return tf::checkedHash(self, out);                                             \
    }
