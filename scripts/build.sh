#!/usr/bin/env bash
set -euo pipefail

# Change to the repository root.
cd "$( dirname "${BASH_SOURCE[0]}" )"
cd ..
ROOT_DIR="$PWD"

# Shortcut for running echo_status.sh
status() {
    "$ROOT_DIR/scripts/helpers/echo_status.sh" "$@"
}

# Set OS, ARCH, etc.
source "build/config.sh"

build_tvision4c() {
    status "header" "tvision4c"

    # Create directories
    mkdir -p "build/native-artifacts/tvision4c/build"
    mkdir -p "build/native-artifacts/tvision4c/bin"

    # Configure cmake
    cd "src/tvision4c"
    if [ "$OS" == "windows" ]; then
        "$CMAKE" \
            -G "Visual Studio 17 2022" \
            -A "$WINDOWS_MSVC_ARCH" \
            -B "../../build/native-artifacts/tvision4c/build" \
            -S . \
            -DCMAKE_PREFIX_PATH="$ROOT_DIR/build/prefix" \
            -DCMAKE_INSTALL_PREFIX="$ROOT_DIR/build/prefix" \
            -DCMAKE_BUILD_TYPE="$CONFIGURATION"
    else
        "$CMAKE" \
            -G "Unix Makefiles" \
            -B "../../build/native-artifacts/tvision4c/build" \
            -S . \
            -DCMAKE_PREFIX_PATH="$ROOT_DIR/build/prefix" \
            -DCMAKE_INSTALL_PREFIX="$ROOT_DIR/build/prefix" \
            -DCMAKE_BUILD_TYPE="$CONFIGURATION"
    fi

    # Build
    "$CMAKE" --build "../../build/native-artifacts/tvision4c/build" --config "$CONFIGURATION"

    cd "$ROOT_DIR"
}

build_turbovision() {
    status "header" "TurboVision"
    cd "$ROOT_DIR/src"
    dotnet build TurboVision/TurboVision.csproj --runtime "$RID"
    cd "$ROOT_DIR"
}

build_turbovision_demo() {
    status "header" "TurboVision.Demo"
    cd "$ROOT_DIR/src"
    dotnet build TurboVision.Demo/TurboVision.Demo.csproj --runtime "$RID"
    cd "$ROOT_DIR"
}

build_tvision4c
build_turbovision
build_turbovision_demo

status "success" "Build complete."
