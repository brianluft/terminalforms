#pragma once

#ifdef _WIN32
#define TVEXPORT __declspec(dllexport)
#else
#define TVEXPORT
#endif

extern "C" {

TVEXPORT int healthCheck();

}  // extern "C"
