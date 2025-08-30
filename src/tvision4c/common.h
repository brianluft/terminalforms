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

// Matches `src\TurboVision\Error.cs`
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

// This templated function gets the size and alignment of a type `T`.
// It catches any exception and converts to `Error`.
template <typename T>
Error checkedSize(int32_t* outSize, int32_t* outAlignment) {
    if (!outSize || !outAlignment) {
        return tv::Error_ArgumentNull;
    }

    *outSize = sizeof(T);
    *outAlignment = alignof(T);
    return tv::Success;
}

// This templated function instantiates an object of type `T` at a given address.
// It catches any exception and converts to `Error`.
// If constructor arguments are provided, initialize is skipped.
template <typename T, typename... Args>
Error checkedPlacementNew(T* self, Args&&... args) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }

    // Check alignment of the pointer.
    if (reinterpret_cast<uintptr_t>(self) % alignof(T) != 0) {
        return tv::Error_UnalignedObjectPlacement;
    }

    try {
        new (self) T(std::forward<Args>(args)...);

        // Only call initialize if this was the default constructor (no args)
        if constexpr (sizeof...(args) == 0) {
            tv::initialize<T>{}(self);
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

// This templated function deletes an object of type `T` at a given address.
// It catches any exception and converts to `Error`.
template <typename T>
Error checkedPlacementDelete(T* self) {
    if (!self) {
        return tv::Success;  // Not an error.
    }

    try {
        self->~T();
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

// String utility functions for owned string management
char* newStr(const char* source);
bool stringEquals(const char* a, const char* b);
std::size_t stringHash(const char* str);

}  // namespace tv

// Use this macro if the class has a default parameterless constructor.
#define TV_DEFAULT_CONSTRUCTOR(type)                        \
    EXPORT tv::Error TV_##type##_placementNew(type* self) { \
        return tv::checkedPlacementNew(self);               \
    }                                                       \
    EXPORT tv::Error TV_##type##_new(type** out) {          \
        return tv::checkedNew(out);                         \
    }

// Every class/struct that is exported to C must have these functions.
// Use this macro in the .cpp file.
#define TV_BOILERPLATE_FUNCTIONS(type)                                                    \
    EXPORT tv::Error TV_##type##_placementSize(int32_t* outSize, int32_t* outAlignment) { \
        return tv::checkedSize<type>(outSize, outAlignment);                              \
    }                                                                                     \
    EXPORT tv::Error TV_##type##_placementDelete(type* self) {                            \
        return tv::checkedPlacementDelete(self);                                          \
    }                                                                                     \
    EXPORT tv::Error TV_##type##_delete(type* self) {                                     \
        return tv::checkedDelete(self);                                                   \
    }                                                                                     \
    EXPORT tv::Error TV_##type##_equals(type* self, type* other, BOOL* out) {             \
        return tv::checkedEquals(self, other, out);                                       \
    }                                                                                     \
    EXPORT tv::Error TV_##type##_hash(type* self, int32_t* out) {                         \
        return tv::checkedHash(self, out);                                                \
    }

// Getters and setters for public members are implemented in a standard way.
// Use this macro for primitives: integers and booleans.
// TV_GET_SET_PRIMITIVE is when the way to access the member is simply by the member name.
// TV_GET_SET_PRIMITIVE_EX is when the way to access the member is complicated (i.e. a dotted path).
#define TV_GET_SET_PRIMITIVE_EX(objectType, memberType, memberName, accessor)                 \
    EXPORT tv::Error TV_##objectType##_get_##memberName(objectType* self, memberType* out) {  \
        if (!self || !out) {                                                                  \
            return tv::Error_ArgumentNull;                                                    \
        }                                                                                     \
        *out = self->accessor;                                                                \
        return tv::Success;                                                                   \
    }                                                                                         \
    EXPORT tv::Error TV_##objectType##_set_##memberName(objectType* self, memberType value) { \
        if (!self) {                                                                          \
            return tv::Error_ArgumentNull;                                                    \
        }                                                                                     \
        self->accessor = value;                                                               \
        return tv::Success;                                                                   \
    }

#define TV_GET_SET_PRIMITIVE(objectType, memberType, memberName) \
    TV_GET_SET_PRIMITIVE_EX(objectType, memberType, memberName, memberName)

// Use this macro for copyable objects.
#define TV_DECLARE_GET_SET_COPYABLE_OBJECT(objectType, memberType, memberName)              \
    EXPORT tv::Error TV_##objectType##_get_##memberName(objectType* self, memberType* out); \
    EXPORT tv::Error TV_##objectType##_set_##memberName(objectType* self, memberType* value);

#define TV_GET_SET_COPYABLE_OBJECT(objectType, memberType, memberName)                         \
    EXPORT tv::Error TV_##objectType##_get_##memberName(objectType* self, memberType* out) {   \
        if (!self || !out) {                                                                   \
            return tv::Error_ArgumentNull;                                                     \
        }                                                                                      \
        *out = self->memberName;                                                               \
        return tv::Success;                                                                    \
    }                                                                                          \
    EXPORT tv::Error TV_##objectType##_set_##memberName(objectType* self, memberType* value) { \
        if (!self || !value) {                                                                 \
            return tv::Error_ArgumentNull;                                                     \
        }                                                                                      \
        self->memberName = *value;                                                             \
        return tv::Success;                                                                    \
    }

// Use this macro for a null-terminated char array with no associated length field.
#define TV_GET_SET_STRING_BUFFER(objectType, stringMemberName)                                       \
    EXPORT tv::Error TV_##objectType##_get_##stringMemberName(objectType* self, const char** out) {  \
        if (!self || !out) {                                                                         \
            return tv::Error_ArgumentNull;                                                           \
        }                                                                                            \
        *out = self->stringMemberName;                                                               \
        return tv::Success;                                                                          \
    }                                                                                                \
    EXPORT tv::Error TV_##objectType##_set_##stringMemberName(objectType* self, const char* value) { \
        if (!self || !value) {                                                                       \
            return tv::Error_ArgumentNull;                                                           \
        }                                                                                            \
        strncpy(self->stringMemberName, value, sizeof(self->stringMemberName) - 1);                  \
        self->stringMemberName[sizeof(self->stringMemberName) - 1] = '\0';                           \
        return tv::Success;                                                                          \
    }

// Use this macro for a char array with an associated length field.
#define TV_GET_SET_STRING_BUFFER_WITH_LENGTH(objectType, stringMemberName, lengthMemberName, bufferSize) \
    EXPORT tv::Error TV_##objectType##_get_##stringMemberName(                                           \
        objectType* self, const char** out, uint8_t* outTextLength) {                                    \
        if (!self || !out || !outTextLength) {                                                           \
            return tv::Error_ArgumentNull;                                                               \
        }                                                                                                \
        *outTextLength = self->lengthMemberName;                                                         \
        *out = self->stringMemberName;                                                                   \
        return tv::Success;                                                                              \
    }                                                                                                    \
    EXPORT tv::Error TV_##objectType##_set_##stringMemberName(                                           \
        objectType* self, const char* value, uint8_t textLength) {                                       \
        if (!self || (!value && textLength > 0)) {                                                       \
            return tv::Error_ArgumentNull;                                                               \
        }                                                                                                \
        if (textLength > bufferSize) {                                                                   \
            return tv::Error_BufferTooSmall;                                                             \
        }                                                                                                \
        self->lengthMemberName = textLength;                                                             \
        memset(self->stringMemberName, 0, bufferSize);                                                   \
        if (textLength > 0) {                                                                            \
            memcpy(self->stringMemberName, value, textLength);                                           \
        }                                                                                                \
        return tv::Success;                                                                              \
    }

// Use this macro for a C string pointer that is owned by the object and must be freed when changing.
#define TV_GET_SET_OWNED_STRING(objectType, stringMemberName)                                        \
    EXPORT tv::Error TV_##objectType##_get_##stringMemberName(objectType* self, const char** out) {  \
        if (!self || !out) {                                                                         \
            return tv::Error_ArgumentNull;                                                           \
        }                                                                                            \
        *out = self->stringMemberName;                                                               \
        return tv::Success;                                                                          \
    }                                                                                                \
    EXPORT tv::Error TV_##objectType##_set_##stringMemberName(objectType* self, const char* value) { \
        if (!self || !value) {                                                                       \
            return tv::Error_ArgumentNull;                                                           \
        }                                                                                            \
        delete[] self->stringMemberName;                                                             \
        self->stringMemberName = tv::newStr(value);                                                  \
        return tv::Success;                                                                          \
    }

// Use this macro for a C++ bool type that we have to convert to C BOOL.
#define TV_GET_SET_BOOL(objectType, memberName)                                         \
    EXPORT tv::Error TV_##objectType##_get_##memberName(objectType* self, BOOL* out) {  \
        if (!self || !out) {                                                            \
            return tv::Error_ArgumentNull;                                              \
        }                                                                               \
        *out = self->memberName ? TRUE : FALSE;                                         \
        return tv::Success;                                                             \
    }                                                                                   \
    EXPORT tv::Error TV_##objectType##_set_##memberName(objectType* self, BOOL value) { \
        if (!self) {                                                                    \
            return tv::Error_ArgumentNull;                                              \
        }                                                                               \
        self->memberName = value == TRUE;                                               \
        return tv::Success;                                                             \
    }

// Use this macro for binary operators on copyable objects.
#define TV_BINARY_OPERATOR_COPYABLE_OBJECT(lhsType, rhsType, operatorSymbol, operatorName, resultType)     \
    EXPORT tv::Error TV_##lhsType##_operator_##operatorName(lhsType* lhs, rhsType* rhs, resultType* out) { \
        if (!lhs || !rhs || !out) {                                                                        \
            return tv::Error_ArgumentNull;                                                                 \
        }                                                                                                  \
        *out = *lhs operatorSymbol * rhs;                                                                  \
        return tv::Success;                                                                                \
    }

// Use this macro for in-place binary operators on copyable objects, like += and -=.
#define TV_BINARY_OPERATOR_IN_PLACE(lhsType, rhsType, operatorSymbol, operatorName)                  \
    EXPORT tv::Error TV_##lhsType##_operator_##operatorName##_in_place(lhsType* lhs, rhsType* rhs) { \
        if (!lhs || !rhs) {                                                                          \
            return tv::Error_ArgumentNull;                                                           \
        }                                                                                            \
        *lhs operatorSymbol## = *rhs;                                                                \
        return tv::Success;                                                                          \
    }

EXPORT int32_t TV_healthCheck();
EXPORT tv::Error TV_getLastErrorMessageLength(int32_t* out);
EXPORT tv::Error TV_getLastErrorMessage(char* buffer, int32_t bufferSize);
