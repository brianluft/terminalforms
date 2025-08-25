#include "KeyDownEvent.h"

TV_DEFAULT_CONSTRUCTOR(KeyDownEvent)
TV_BOILERPLATE_FUNCTIONS(KeyDownEvent)
TV_GET_SET_PRIMITIVE(KeyDownEvent, uint16_t, keyCode)
TV_GET_SET_PRIMITIVE_EX(KeyDownEvent, uint8_t, charCode, charScan.charCode)
TV_GET_SET_PRIMITIVE_EX(KeyDownEvent, uint8_t, scanCode, charScan.scanCode)
TV_GET_SET_PRIMITIVE(KeyDownEvent, uint16_t, controlKeyState)
TV_GET_SET_STRING_BUFFER_WITH_LENGTH(KeyDownEvent, text, textLength, maxCharSize)
