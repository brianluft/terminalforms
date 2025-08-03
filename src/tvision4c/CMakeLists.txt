cmake_minimum_required(VERSION 3.15)
project(tvision4c)

# Set C++ standard
set(CMAKE_CXX_STANDARD 11)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Create the DLL
add_library(tvision4c SHARED tvision4c.cpp)

# Link to tvision library (platform-specific)
if(WIN32)
    target_link_libraries(tvision4c "${CMAKE_SOURCE_DIR}/../../build/prefix/lib/tvision.lib")
elseif(APPLE)
    target_link_libraries(tvision4c "${CMAKE_SOURCE_DIR}/../../build/prefix/lib/libtvision.a" ncurses)
else()
    target_link_libraries(tvision4c "${CMAKE_SOURCE_DIR}/../../build/prefix/lib/libtvision.a" ncursesw)
endif()

# Set output directories
set_target_properties(tvision4c PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../../build/native-artifacts/tvision4c/bin"
    LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../../build/native-artifacts/tvision4c/bin"
    ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../../build/native-artifacts/tvision4c/bin"
)
