#include "TSItem.h"

// Constructor with parameters: TSItem( TStringView aValue, TSItem *aNext )
EXPORT tv::Error TV_TSItem_placementNew2(TSItem* self, const char* aValue, TSItem* aNext) {
    return tv::checkedPlacementNew(self, TStringView(aValue), aNext);
}

EXPORT tv::Error TV_TSItem_new2(TSItem** out, const char* aValue, TSItem* aNext) {
    return tv::checkedNew(out, TStringView(aValue), aNext);
}

TV_BOILERPLATE_FUNCTIONS(TSItem)
TV_GET_SET_OWNED_STRING(TSItem, value)
TV_GET_SET_PRIMITIVE(TSItem, TSItem*, next)
