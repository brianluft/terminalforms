#include "MouseEventType.h"
#include "../Objects/TPoint.h"

TV_BOILERPLATE_FUNCTIONS(MouseEventType)
TV_GET_SET_COPYABLE_OBJECT(MouseEventType, TPoint, where)
TV_GET_SET_PRIMITIVE(MouseEventType, uint16_t, eventFlags)
TV_GET_SET_PRIMITIVE(MouseEventType, uint16_t, controlKeyState)
TV_GET_SET_PRIMITIVE(MouseEventType, uint8_t, buttons)
TV_GET_SET_PRIMITIVE(MouseEventType, uint8_t, wheel)
