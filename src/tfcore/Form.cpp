#include "Form.h"

#define Uses_TProgram
#define Uses_TDeskTop
#include <tvision/tv.h>

namespace tf {

Form::Form() : TDialog(TRect(0, 0, 20, 8), "Form"), TWindowInit(TDialog::initFrame) {}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(Form)
TF_BOILERPLATE_FUNCTIONS(Form)

EXPORT tf::Error TfFormShow(tf::Form* self) {
    TProgram::deskTop->insert(self);
    return tf::Success;
}
