#pragma once

#include "common.h"

#define Uses_TApplication
#include <tvision/tv.h>

namespace tf {

class Application : public TApplication {
   public:
    static Application instance;

    Application();
    virtual ~Application();
};

}  // namespace tf
