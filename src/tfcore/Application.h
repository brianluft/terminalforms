#pragma once

#include "common.h"
#include <queue>

#define Uses_TApplication
#define Uses_TEvent
#include <tvision/tv.h>

namespace tf {

class Application : public TApplication {
   public:
    static Application instance;

    Application();
    virtual ~Application();

    void idle() override;
    void enableDebugScreenshot(const std::string& outputFile);
    void enableDebugEvents(const std::string& inputFile);

   private:
    bool debugScreenshotEnabled_ = false;
    std::string debugScreenshotOutputFile_;

    bool debugEventsEnabled_ = false;
    std::queue<TEvent> debugEventsQueue_;

    void saveDebugScreenshot();
};

}  // namespace tf
