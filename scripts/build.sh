#!/usr/bin/env bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"
cd $ROOT_DIR

build_tfcore() {
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

build_solution() {
    status "header" "dotnet build"
    cd "$ROOT_DIR/src"
    dotnet build TerminalForms.sln "-p:MyRuntimeIdentifier=$RID"
    cd "$ROOT_DIR"
}

run_tests() {
    status "header" "dotnet test"
    cd "$ROOT_DIR/src"

    # Is TARGET_DOTNET_ROOT set? If so, switch over to it for test running.
    if [ -n "${TARGET_DOTNET_ROOT:-}" ]; then
        export DOTNET_ROOT="$TARGET_DOTNET_ROOT"
        export PATH="$DOTNET_ROOT:$DOTNET_ROOT/tools:$PATH"
        echo "Using dotnet: $(which dotnet)"
    fi

    dotnet test Tests/Tests.csproj "-p:MyRuntimeIdentifier=$RID" --no-build --logger "console;verbosity=detailed"
    cd "$ROOT_DIR"
}

build_website() {
    status "header" "website"
    cd "$ROOT_DIR/src/website"
    rm -rf _site api "$ROOT_DIR/build/website"
    dotnet docfx --warningsAsErrors --logLevel warning docfx.json
    mv -f _site "$ROOT_DIR/build/website"
    rm -rf api
    cd "$ROOT_DIR"
}

build_tfcore
build_solution
if [ -z "${NO_TESTS:-}" ]; then
    run_tests
fi
build_website

status "success" "Build complete."
