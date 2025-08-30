#include "TParagraph.h"

TV_DEFAULT_CONSTRUCTOR(TParagraph)
TV_BOILERPLATE_FUNCTIONS(TParagraph)
TV_GET_SET_PRIMITIVE(TParagraph, TParagraph*, next)
TV_GET_SET_BOOL(TParagraph, wrap)
TV_GET_SET_PRIMITIVE(TParagraph, uint16_t, size)
TV_GET_SET_OWNED_STRING(TParagraph, text)
