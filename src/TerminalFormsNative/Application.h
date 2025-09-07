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

    void idle() override;
    void enableDebugScreenshot(const std::string& outputFile);

   private:
    bool debugScreenshotEnabled_ = false;
    std::string debugScreenshotOutputFile_;

    void saveDebugScreenshot();
};

}  // namespace tf
