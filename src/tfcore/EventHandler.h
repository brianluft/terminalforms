#pragma once

#include "common.h"

namespace tf {

typedef void(TF_CDECL* EventHandlerFunction)(void* userData);

class EventHandler {
   public:
    inline EventHandler() : function(nullptr), userData(nullptr) {}

    inline EventHandler(EventHandlerFunction function, void* userData) : function(function), userData(userData) {}

    inline void operator()() const {
        if (function != nullptr) {
            function(userData);
        }
    }

   private:
    EventHandlerFunction function;
    void* userData;
};

}  // namespace tf
