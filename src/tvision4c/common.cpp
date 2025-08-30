#include "common.h"
#include <array>
#include <cstring>
#include <limits>

namespace tv {

thread_local std::string lastErrorMessage;

// String utility functions for owned string management
char* newStr(const char* source) {
    if (!source) {
        return nullptr;
    }

    size_t len = strlen(source) + 1;
    char* result = new char[len];
    strcpy(result, source);
    return result;
}

bool stringEquals(const char* a, const char* b) {
    if (a == b)
        return true;
    if (!a || !b)
        return false;
    return strcmp(a, b) == 0;
}

std::size_t stringHash(const char* str) {
    if (!str)
        return 0;
    return std::hash<std::string>{}(std::string(str));
}

}  // namespace tv

EXPORT int32_t TV_healthCheck() {
    // The C# side will verify this value.
    return 123;
}

EXPORT tv::Error TV_getLastErrorMessageLength(int32_t* out) {
    auto length = tv::lastErrorMessage.length();

    if (length > std::numeric_limits<int32_t>::max()) {
        // The error message is so long, we can't represent the length in an int32_t!
        return tv::Error_BufferTooSmall;
    }

    *out = static_cast<int32_t>(length);
    return tv::Success;
}

EXPORT tv::Error TV_getLastErrorMessage(char* buffer, int32_t bufferSize) {
    if (!buffer) {
        return tv::Error_ArgumentNull;
    }

    auto length = tv::lastErrorMessage.length();

    if (bufferSize < 0 || static_cast<size_t>(bufferSize) < length) {
        return tv::Error_BufferTooSmall;
    }

    memcpy(buffer, tv::lastErrorMessage.c_str(), length);
    return tv::Success;
}
