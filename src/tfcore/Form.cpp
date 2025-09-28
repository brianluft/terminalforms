#include "Form.h"

#define Uses_TProgram
#define Uses_TDeskTop
#define Uses_TRect
#define Uses_TFrame
#include <tvision/tv.h>

namespace tf {

Form::Form() : TDialog(TRect(0, 0, 20, 8), "Form"), TWindowInit(TDialog::initFrame) {}

const char* Form::getText() const {
    return title ? title : "";
}

void Form::setText(const char* text) {
    delete[] title;
    title = newStr(text);
    frame->drawView();
}

void Form::getBounds(Rectangle* out) const {
    TRect rect = TView::getBounds();
    *out = Rectangle(rect);
}

void Form::setBounds(const Rectangle& bounds) {
    TRect rect = bounds.toTRect();
    locate(rect);
}

BOOL Form::getControlBox() const {
    return (flags & wfClose) != 0;
}

void Form::setControlBox(BOOL value) {
    if (value) {
        flags |= wfClose;
    } else {
        flags &= ~wfClose;
    }
    if (frame) {
        frame->drawView();
    }
}

BOOL Form::getMaximizeBox() const {
    return (flags & wfZoom) != 0;
}

void Form::setMaximizeBox(BOOL value) {
    if (value) {
        flags |= wfZoom;
    } else {
        flags &= ~wfZoom;
    }
    if (frame) {
        frame->drawView();
    }
}

BOOL Form::getResizable() const {
    return (flags & wfGrow) != 0;
}

void Form::setResizable(BOOL value) {
    if (value) {
        flags |= wfGrow;
    } else {
        flags &= ~wfGrow;
    }
    if (frame) {
        frame->drawView();
    }
}

void Form::close() {
    TProgram::deskTop->remove(this);
}

}  // namespace tf

TF_DEFAULT_CONSTRUCTOR(Form)
TF_BOILERPLATE_FUNCTIONS(Form)

TF_EXPORT tf::Error TfFormShow(tf::Form* self) {
    TProgram::deskTop->insert(self);
    return tf::Success;
}

TF_EXPORT tf::Error TfFormSetText(tf::Form* self, const char* text) {
    if (self == nullptr || text == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setText(text);
    return tf::Success;
}

TF_EXPORT tf::Error TfFormGetText(tf::Form* self, const char** out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getText();
    return tf::Success;
}

TF_EXPORT tf::Error TfFormSetBounds(tf::Form* self, const tf::Rectangle* bounds) {
    if (self == nullptr || bounds == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setBounds(*bounds);
    return tf::Success;
}

TF_EXPORT tf::Error TfFormGetBounds(tf::Form* self, tf::Rectangle* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->getBounds(out);
    return tf::Success;
}

TF_EXPORT tf::Error TfFormSetControlBox(tf::Form* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setControlBox(value);
    return tf::Success;
}

TF_EXPORT tf::Error TfFormGetControlBox(tf::Form* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getControlBox();
    return tf::Success;
}

TF_EXPORT tf::Error TfFormSetMaximizeBox(tf::Form* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setMaximizeBox(value);
    return tf::Success;
}

TF_EXPORT tf::Error TfFormGetMaximizeBox(tf::Form* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getMaximizeBox();
    return tf::Success;
}

TF_EXPORT tf::Error TfFormSetResizable(tf::Form* self, BOOL value) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->setResizable(value);
    return tf::Success;
}

TF_EXPORT tf::Error TfFormGetResizable(tf::Form* self, BOOL* out) {
    if (self == nullptr || out == nullptr) {
        return tf::Error_ArgumentNull;
    }

    *out = self->getResizable();
    return tf::Success;
}

TF_EXPORT tf::Error TfFormClose(tf::Form* self) {
    if (self == nullptr) {
        return tf::Error_ArgumentNull;
    }

    self->close();
    return tf::Success;
}
