#include "Error.h"

namespace tv {

thread_local std::string lastErrorMessage;

}  // namespace tv

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

    if (bufferSize < length) {
        return tv::Error_BufferTooSmall;
    }

    memcpy(buffer, tv::lastErrorMessage.c_str(), length);
    return tv::Success;
}
