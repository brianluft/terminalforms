#include "TReplaceDialogRec.h"

// Non-default constructor: TReplaceDialogRec(const char *str, const char *rep, ushort flgs)
EXPORT tv::Error TV_TReplaceDialogRec_placementNew2(
    TReplaceDialogRec* self,
    const char* str,
    const char* rep,
    uint16_t flgs) {
    return tv::checkedPlacementNew(self, str, rep, flgs);
}

EXPORT tv::Error TV_TReplaceDialogRec_new2(TReplaceDialogRec** out, const char* str, const char* rep, uint16_t flgs) {
    return tv::checkedNew(out, str, rep, flgs);
}

TV_BOILERPLATE_FUNCTIONS(TReplaceDialogRec)
TV_GET_SET_STRING_BUFFER(TReplaceDialogRec, find)
TV_GET_SET_STRING_BUFFER(TReplaceDialogRec, replace)
TV_GET_SET_PRIMITIVE(TReplaceDialogRec, uint16_t, options)
