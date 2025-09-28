#pragma once

#include "common.h"
#include "Rectangle.h"

#define Uses_TDialog
#include <tvision/tv.h>

namespace tf {

class Form : public TDialog {
   public:
    Form();

    // Property management methods
    const char* getText() const;
    void setText(const char* text);
    void getBounds(Rectangle* out) const;
    void setBounds(const Rectangle& bounds);
    BOOL getControlBox() const;
    void setControlBox(BOOL value);
    BOOL getMaximizeBox() const;
    void setMaximizeBox(BOOL value);
    BOOL getResizable() const;
    void setResizable(BOOL value);
    void close();
};

template <>
struct equals<Form> {
    bool operator()(const Form& self, const Form& other) const { return &self == &other; }
};

}  // namespace tf

namespace std {
template <>
struct hash<tf::Form> {
    std::size_t operator()(const tf::Form& p) const noexcept {
        std::size_t x{};
        tf::combineHash(&p, &x);
        return x;
    }
};
}  // namespace std
