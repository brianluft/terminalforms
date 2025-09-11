#include "Button.h"

#define Uses_TRect
#include <tvision/tv.h>

namespace tf {

Button::Button() : TButton(TRect(2, 2, 12, 4), "Button", 0, bfNormal) {}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(Button)

TF_BOILERPLATE_FUNCTIONS(Button)
