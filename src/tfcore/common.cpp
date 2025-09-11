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

EXPORT tf::Error TfGetLastErrorMessage(const char** out) {
    if (!out) {
        return tf::Error_ArgumentNull;
    }

    *out = lastErrorMessage.c_str();
    return tf::Success;
}
