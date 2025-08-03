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

# Set OS, ARCH, LINUX_LIBC, WINDOWS_MSVC_ARCH, CMAKE.
source "build/config.sh"

# Builds tvision4c
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
            -DCMAKE_INSTALL_PREFIX="$ROOT_DIR/build/prefix"
    else
        "$CMAKE" \
            -G "Unix Makefiles" \
            -B "../../build/native-artifacts/tvision4c/build" \
            -S . \
            -DCMAKE_PREFIX_PATH="$ROOT_DIR/build/prefix" \
            -DCMAKE_INSTALL_PREFIX="$ROOT_DIR/build/prefix"
    fi

    # Build
    "$CMAKE" --build "../../build/native-artifacts/tvision4c/build" --config Release

    cd "$ROOT_DIR"
}

# Builds TerminalForms
build_terminalforms() {
    status "header" "TerminalForms"

    cd "$ROOT_DIR/src"
    dotnet build TerminalForms.sln --verbosity=quiet

    cd "$ROOT_DIR"
}

build_tvision4c
build_terminalforms
