#include "TNode.h"

EXPORT tv::Error TV_TNode_placementNew(TNode* self, const char* aText) {
    return tv::checkedPlacementNew(self, aText);
}

EXPORT tv::Error TV_TNode_new(TNode** out, const char* aText) {
    return tv::checkedNew(out, aText);
}

EXPORT tv::Error TV_TNode_placementNew2(
    TNode* self,
    const char* aText,
    TNode* aChildren,
    TNode* aNext,
    int32_t initialState) {
    return tv::checkedPlacementNew(self, aText, aChildren, aNext, initialState);
}

EXPORT tv::Error TV_TNode_new2(TNode** out, const char* aText, TNode* aChildren, TNode* aNext, int32_t initialState) {
    return tv::checkedNew(out, aText, aChildren, aNext, initialState);
}

TV_BOILERPLATE_FUNCTIONS(TNode)
TV_GET_SET_PRIMITIVE(TNode, TNode*, next)
TV_GET_SET_OWNED_STRING(TNode, text)
TV_GET_SET_PRIMITIVE(TNode, TNode*, childList)
TV_GET_SET_BOOL(TNode, expanded)
