#!/usr/bin/env bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"
cd $ROOT_DIR

build_terminalformsnative() {
    status "header" "TerminalFormsNative"

    # Create directories
    mkdir -p "build/native-artifacts/TerminalFormsNative/build"
    mkdir -p "build/native-artifacts/TerminalFormsNative/bin"

    # Configure cmake
    cd "src/TerminalFormsNative"
    if [ "$OS" == "windows" ]; then
        cmake \
            -G "Visual Studio 17 2022" \
            -A "$WINDOWS_MSVC_ARCH" \
            -B "../../build/native-artifacts/TerminalFormsNative/build" \
            -S . \
            -DCMAKE_PREFIX_PATH="$ROOT_DIR/build/prefix" \
            -DCMAKE_INSTALL_PREFIX="$ROOT_DIR/build/prefix" \
            -DCMAKE_BUILD_TYPE="$CONFIGURATION"
    else
        cmake \
            -G "Unix Makefiles" \
            -B "../../build/native-artifacts/TerminalFormsNative/build" \
            -S . \
            -DCMAKE_PREFIX_PATH="$ROOT_DIR/build/prefix" \
            -DCMAKE_INSTALL_PREFIX="$ROOT_DIR/build/prefix" \
            -DCMAKE_BUILD_TYPE="$CONFIGURATION"
    fi

    # Build
    cmake --build "../../build/native-artifacts/TerminalFormsNative/build" --config "$CONFIGURATION"

    cd "$ROOT_DIR"
}

build_turbovision() {
    status "header" "TerminalForms"
    cd "$ROOT_DIR/src"
    dotnet build TerminalForms/TerminalForms.csproj --runtime "$RID"
    cd "$ROOT_DIR"
}

build_turbovision_demo() {
    status "header" "TerminalFormsDemo"
    cd "$ROOT_DIR/src"
    dotnet build TerminalFormsDemo/TerminalFormsDemo.csproj --runtime "$RID"
    cd "$ROOT_DIR"
}

run_tests() {
    status "header" "Tests"
    cd "$ROOT_DIR/src"
    dotnet test Tests/Tests.csproj --runtime "$RID"
    cd "$ROOT_DIR"
}

build_terminalformsnative
build_turbovision
build_turbovision_demo
run_tests
status "success" "Build complete."
