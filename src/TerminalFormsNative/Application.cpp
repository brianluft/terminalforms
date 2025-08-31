#include "Application.h"

namespace tf {

Application Application::instance;

Application::Application() : TProgInit(TProgram::initStatusLine, TProgram::initMenuBar, TProgram::initDeskTop) {}

Application::~Application() {}

}  // namespace tf

EXPORT tf::Error TfApplicationStaticRun() {
    tf::Application::instance.run();
    tf::Application::instance.shutDown();
    return tf::Success;
}
