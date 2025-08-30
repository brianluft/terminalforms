#include "TResourceItem.h"

TV_DEFAULT_CONSTRUCTOR(TResourceItem)
TV_BOILERPLATE_FUNCTIONS(TResourceItem)

TV_GET_SET_PRIMITIVE(TResourceItem, int32_t, pos)
TV_GET_SET_PRIMITIVE(TResourceItem, int32_t, size)
TV_GET_SET_OWNED_STRING(TResourceItem, key)
