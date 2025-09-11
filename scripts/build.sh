#!/usr/bin/env bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"
cd $ROOT_DIR

build_terminalformsnative() {
    status "header" "tfcore"

    # Create directories
    mkdir -p "build/native-artifacts/tfcore/build"
    mkdir -p "build/native-artifacts/tfcore/bin"

    # Configure cmake
    cd "src/tfcore"
    if [ "$OS" == "windows" ]; then
        cmake \
            -G "Visual Studio 17 2022" \
            -A "$WINDOWS_MSVC_TARGET_ARCH" \
            -B "../../build/native-artifacts/tfcore/build" \
            -S . \
            -DCMAKE_PREFIX_PATH="$ROOT_DIR/build/prefix" \
            -DCMAKE_INSTALL_PREFIX="$ROOT_DIR/build/prefix" \
            -DCMAKE_BUILD_TYPE="$CONFIGURATION"
    else
        cmake \
            -G "Unix Makefiles" \
            -B "../../build/native-artifacts/tfcore/build" \
            -S . \
            -DCMAKE_PREFIX_PATH="$ROOT_DIR/build/prefix" \
            -DCMAKE_INSTALL_PREFIX="$ROOT_DIR/build/prefix" \
            -DCMAKE_BUILD_TYPE="$CONFIGURATION"
    fi

    # Build
    cmake --build "../../build/native-artifacts/tfcore/build" --config "$CONFIGURATION"

    cd "$ROOT_DIR"
}

restore() {
    status "header" "dotnet restore"
    cd "$ROOT_DIR/src"
    dotnet restore --runtime "$RID"
    status "success" "Restored package dependencies"
    cd "$ROOT_DIR"
}

build_terminalforms() {
    status "header" "TerminalForms"
    cd "$ROOT_DIR/src"
    dotnet build TerminalForms/TerminalForms.csproj --runtime "$RID" --no-restore
    cd "$ROOT_DIR"
}

build_terminalforms_demo() {
    status "header" "TerminalFormsDemo"
    cd "$ROOT_DIR/src"
    dotnet build TerminalFormsDemo/TerminalFormsDemo.csproj --runtime "$RID" --no-restore
    cd "$ROOT_DIR"
}

run_tests() {
    status "header" "Tests"
    cd "$ROOT_DIR/src"

    # Is TARGET_DOTNET_ROOT set? If so, switch over to it for test running.
    if [ -n "${TARGET_DOTNET_ROOT:-}" ]; then
        export DOTNET_ROOT="$TARGET_DOTNET_ROOT"
        export PATH="$DOTNET_ROOT:$DOTNET_ROOT/tools:$PATH"
        echo "Using dotnet: $(which dotnet)"
    fi

    dotnet build Tests/Tests.csproj --runtime "$RID" --no-restore
    dotnet test Tests/Tests.csproj --runtime "$RID" --no-build
    cd "$ROOT_DIR"
}

restore
build_terminalformsnative
build_terminalforms
build_terminalforms_demo

if [ -z "${NO_TESTS:-}" ]; then
    run_tests
fi

status "success" "Build complete."
