#include "TDirEntry.h"

// TDirEntry has a non-default constructor that takes two TStringView parameters
// We'll create a constructor that takes const char* instead per user requirements
EXPORT tv::Error TV_TDirEntry_placementNew2(TDirEntry* self, const char* text, const char* dir) {
    if (!text || !dir) {
        return tv::Error_ArgumentNull;
    }

    return tv::checkedPlacementNew(self, TStringView(text), TStringView(dir));
}

EXPORT tv::Error TV_TDirEntry_new2(TDirEntry** out, const char* text, const char* dir) {
    if (!text || !dir) {
        return tv::Error_ArgumentNull;
    }

    return tv::checkedNew(out, TStringView(text), TStringView(dir));
}

TV_BOILERPLATE_FUNCTIONS(TDirEntry)

// Accessor methods for dir() and text()
EXPORT tv::Error TV_TDirEntry_dir(TDirEntry* self, char** out) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }
    if (!out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->dir();
    return tv::Success;
}

EXPORT tv::Error TV_TDirEntry_text(TDirEntry* self, char** out) {
    if (!self) {
        return tv::Error_ArgumentNull;
    }
    if (!out) {
        return tv::Error_ArgumentNull;
    }

    *out = self->text();
    return tv::Success;
}
