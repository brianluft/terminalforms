#include "TFindDialogRec.h"

// Non-default constructor: TFindDialogRec(const char *str, ushort flgs)
EXPORT tv::Error TV_TFindDialogRec_placementNew2(TFindDialogRec* self, const char* str, uint16_t flgs) {
    return tv::checkedPlacementNew(self, str, flgs);
}

EXPORT tv::Error TV_TFindDialogRec_new2(TFindDialogRec** out, const char* str, uint16_t flgs) {
    return tv::checkedNew(out, str, flgs);
}

TV_BOILERPLATE_FUNCTIONS(TFindDialogRec)
TV_GET_SET_STRING_BUFFER(TFindDialogRec, find)
TV_GET_SET_PRIMITIVE(TFindDialogRec, uint16_t, options)
