#include "MouseEventType.h"
#include "../Objects/TPoint.h"

TV_IMPLEMENT_BOILERPLATE_FUNCTIONS(MouseEventType)
TV_IMPLEMENT_GET_SET_COPYABLE_OBJECT(MouseEventType, TPoint, where)
TV_IMPLEMENT_GET_SET_PRIMITIVE(MouseEventType, uint16_t, eventFlags)
TV_IMPLEMENT_GET_SET_PRIMITIVE(MouseEventType, uint16_t, controlKeyState)
TV_IMPLEMENT_GET_SET_PRIMITIVE(MouseEventType, uint8_t, buttons)
TV_IMPLEMENT_GET_SET_PRIMITIVE(MouseEventType, uint8_t, wheel)
