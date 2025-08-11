#pragma once

#include "common.h"
#include "Error.h"

#define Uses_TView
#include <tvision/tv.h>

EXPORT tv::Error TV_WriteArgs_placementSize(int32_t* outSize, int32_t* outAlignment);
EXPORT tv::Error TV_WriteArgs_placementNew(write_args* self);
EXPORT tv::Error TV_WriteArgs_placementDelete(write_args* self);
EXPORT tv::Error TV_WriteArgs_new(write_args** out);
EXPORT tv::Error TV_WriteArgs_delete(write_args* self);
EXPORT tv::Error TV_WriteArgs_equals(write_args* self, write_args* other, BOOL* out);
EXPORT tv::Error TV_WriteArgs_hash(write_args* self, int32_t* out);
EXPORT tv::Error TV_WriteArgs_get_self(write_args* self, void** out);
EXPORT tv::Error TV_WriteArgs_set_self(write_args* self, void* value);
EXPORT tv::Error TV_WriteArgs_get_target(write_args* self, void** out);
EXPORT tv::Error TV_WriteArgs_set_target(write_args* self, void* value);
EXPORT tv::Error TV_WriteArgs_get_buf(write_args* self, void** out);
EXPORT tv::Error TV_WriteArgs_set_buf(write_args* self, void* value);
EXPORT tv::Error TV_WriteArgs_get_offset(write_args* self, uint16_t* out);
EXPORT tv::Error TV_WriteArgs_set_offset(write_args* self, uint16_t value);
