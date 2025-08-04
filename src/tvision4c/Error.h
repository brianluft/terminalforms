#pragma once

#include "common.h"
#include <cstdint>
#include <string>

namespace tv {

// Matches `src\TerminalForms\Error.cs`
enum Error {
    Success = 0,
    Error_Unknown,
    Error_NativeInteropFailure,
    Error_OutOfMemory,
    Error_ArgumentNull,
    Error_ArgumentOutOfRange,
    Error_BufferTooSmall,

    Error_HasMessage = 0x8000,
};

// Thread-local storage for detailed error messages
extern thread_local std::string lastErrorMessage;

// This templated function instantiates a new object of type `T`.
// It catches any exception and converts to `Error`.
template <typename T>
Error checkedNew(T** out) {
    if (!out) {
        return tv::Error_ArgumentNull;
    }

    try {
        *out = new T();
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

}  // namespace tv

EXPORT tv::Error TV_getLastErrorMessageLength(int32_t* out);
EXPORT tv::Error TV_getLastErrorMessage(char* buffer, int32_t bufferSize);
