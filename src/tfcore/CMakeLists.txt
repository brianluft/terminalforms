cmake_minimum_required(VERSION 3.31)
project(tfcore)

# Set C++ standard to C++14 for tvision compatibility
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Define compiler and linker flags based on target OS
if(WIN32)
    set(COMMON_COMPILE_OPTIONS
        /utf-8
        /Zc:__cplusplus     # Enable proper __cplusplus macro
        /Zc:preprocessor    # Enable standards-conforming preprocessor
        /EHsc               # Standard C++ exception handling
        /permissive-        # Strict standards conformance
        /W3                 # Warning level
        /wd4996             # Disable deprecation warnings
    )
    # Add preprocessor definitions needed for MSVC compatibility
    add_compile_definitions(
        _CRT_SECURE_NO_WARNINGS
        NOMINMAX            # Don't define min/max macros
        WIN32_LEAN_AND_MEAN # Trim Windows headers
        FMT_CONSTEXPR=inline
        FMT_USE_NONTYPE_TEMPLATE_PARAMETERS=0
    )
else()
    set(COMMON_COMPILE_OPTIONS
        -Wall
        -Werror
        -Winvalid-pch
        -Wno-unknown-pragmas
        -Wno-reorder
    )
endif()

# Create the DLL
add_library(tfcore SHARED
    Application.cpp
    Button.cpp
    CheckBox.cpp
    common.cpp
    Control.cpp
    ControlCollection.cpp
    Form.cpp
    Point.cpp
    Rectangle.cpp
)

# Apply compiler options
target_compile_options(tfcore PRIVATE ${COMMON_COMPILE_OPTIONS})

# Add include directories for tvision headers
target_include_directories(tfcore PRIVATE "${CMAKE_SOURCE_DIR}/../../build/prefix/include")

# Link to tvision library (platform-specific). On Windows, it's either "tvision" or "tvision-debug".
if(CMAKE_BUILD_TYPE STREQUAL "Debug")
    set(TVISION_SUFFIX "-debug")
else()
    set(TVISION_SUFFIX "")
endif()

if(WIN32)
    target_link_libraries(tfcore "${CMAKE_SOURCE_DIR}/../../build/prefix/lib/tvision${TVISION_SUFFIX}.lib")
elseif(APPLE)
    target_link_libraries(tfcore "${CMAKE_SOURCE_DIR}/../../build/prefix/lib/libtvision.a" ncurses)
else()
    target_link_libraries(tfcore "${CMAKE_SOURCE_DIR}/../../build/prefix/lib/libtvision.a" ncursesw)
endif()

# Set output directories
set_target_properties(tfcore PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../../build/native-artifacts/tfcore/bin"
    LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../../build/native-artifacts/tfcore/bin"
    ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../../build/native-artifacts/tfcore/bin"
)
