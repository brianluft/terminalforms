#include "common.h"
#include <array>
#include <cstring>
#include <limits>

namespace {

thread_local std::string lastErrorMessage;

}

namespace tf {

void setLastErrorMessage(const std::string& message) {
    lastErrorMessage = message;
}

}  // namespace tf

TF_EXPORT tf::Error TfGetLastErrorMessage(const char** out) {
    if (!out) {
        return tf::Error_ArgumentNull;
    }

    *out = TF_STRDUP(lastErrorMessage.c_str());
    if (*out == nullptr) {
        return tf::Error_OutOfMemory;
    }
    return tf::Success;
}

TF_EXPORT tf::Error TfHealthCheck(int32_t* out) {
    *out = 123;
    return tf::Success;
}