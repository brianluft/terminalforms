#include "Form.h"

#define Uses_TProgram
#define Uses_TDeskTop
#include <tvision/tv.h>

namespace tf {

Form::Form() : TWindow(TRect(0, 1, 20, 20), "Form", wnNoNumber), TWindowInit(TWindow::initFrame) {}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(Form)
TF_BOILERPLATE_FUNCTIONS(Form)

EXPORT tf::Error TfFormShow(tf::Form* self) {
    TProgram::deskTop->insert(self);
    return tf::Success;
}
