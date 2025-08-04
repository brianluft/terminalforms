#!/usr/bin/env bash

# Downloads and installs dependencies for the project.

# Set NO_MIRROR to use the original URLs instead of the Cloudflare R2 mirror.
# Set NO_HASH to skip hash verification.
# When updating to new versions of deps, set both NO_MIRROR=1 NO_HASH=1 to download fresh.

# Set MAC_ARCH to set the target architecture of the macOS build (x64 or arm64). This is required for macOS builds.
# Other platforms always build for the host architecture.

set -euo pipefail

# Change to the repository root.
cd "$( dirname "${BASH_SOURCE[0]}" )"
cd ..
ROOT_DIR="$PWD"

# Shortcut for running echo_status.sh
status() {
    "$ROOT_DIR/scripts/helpers/echo_status.sh" "$@"
}

# Constants
TVISION_VERSION="df6424f1eee4f5fca9d5530118cab63e0a3c00fa"
TVISION_HASH="32e4b5fceab6257388fc6cd3b3d8d7693c5fd7589bdcb844bf464db3a8d346a1"

CMAKE_VERSION="4.0.3"
CMAKE_MAC_HASH="4e85de4daf1c3e82d7dc6b8ba5683972944b466343aeb9c327a742437bb3ce9a"
CMAKE_LINUX_ARM64_HASH="391da1544ef50ac31300841caaf11db4de3976cdc4468643272e44b3f4644713"
CMAKE_LINUX_X64_HASH="585ae9e013107bc8e7c7c9ce872cbdcbdff569e675b07ef57aacfb88c886faac"
CMAKE_WINDOWS_ARM64_HASH="86ccd6485bbd4bb41a1a858db397be5bca5e0de96858bf8dbba7a64407bd6c00"
CMAKE_WINDOWS_X64_HASH="b59a31dfbfa376a4aaea9ff560ff2b29f78ee5f9fb15447fc71ae7bf9fea9379"

# Global variables
DOWNLOAD_FILE="" # Set by download()
INSTALL_DIR="" # Set by extract_tar_gz() or extract_zip()
CMAKE="" # Set by install_cmake()

# Clean existing build directory.
status "action" "Cleaning build directory..."
rm -rf build
mkdir -p build build/dotnet-artifacts build/native-artifacts

# Create directories.
DOWNLOADS_DIR="$PWD/downloads"
mkdir -p "$DOWNLOADS_DIR"

SOURCES_DIR="$PWD/build/sources"
mkdir -p "$SOURCES_DIR"

PREFIX_DIR="$PWD/build/prefix"
mkdir -p "$PREFIX_DIR"


# Set OS, ARCH, LINUX_LIBC, WINDOWS_MSVC_ARCH.
# Write a script that sets these variables so we don't have to re-detect every time.
source "scripts/helpers/detect_system.sh"
CONFIG_FILE="$ROOT_DIR/build/config.sh"
echo "OS=\"$OS\"" > "$CONFIG_FILE"
echo "ARCH=\"$ARCH\"" >> "$CONFIG_FILE"
echo "LINUX_LIBC=\"$LINUX_LIBC\"" >> "$CONFIG_FILE"
echo "WINDOWS_MSVC_ARCH=\"$WINDOWS_MSVC_ARCH\"" >> "$CONFIG_FILE"
if [ "$OS" == "mac" ]; then
    if [ "$MAC_ARCH" == "x64" ]; then
        echo "CMAKE_FLAGS+=\" -DCMAKE_OSX_ARCHITECTURES=x86_64 -DCMAKE_OSX_DEPLOYMENT_TARGET=13.0\"" >> "$CONFIG_FILE"
    elif [ "$MAC_ARCH" == "arm64" ]; then
        echo "CMAKE_FLAGS+=\" -DCMAKE_OSX_ARCHITECTURES=arm64 -DCMAKE_OSX_DEPLOYMENT_TARGET=13.0\"" >> "$CONFIG_FILE"
    else
        status "error" "Invalid MAC_ARCH: $MAC_ARCH"
        exit 1
    fi
fi
source "$CONFIG_FILE"

status "info" "Detected system: $OS $ARCH $LINUX_LIBC"

# Checks to see if a given URL has been downloaded to the given filename. If not, it downloads it.
# Sets DOWNLOAD_FILE to the absolute path to the downloaded file.
download() {
    local filename="$1"
    local hash="$2"
    local original_url="$3"
    local mirror_url="$4"

    local path="$DOWNLOADS_DIR/$filename"

    if [[ -v NO_MIRROR ]]; then
        local url="$original_url"
    else
        local url="$mirror_url"
    fi

    if [ ! -f "$path" ]; then
        status "action" "Starting download.\n\tURL: $url\n\tSave to: $path"
        curl --fail --location -o "$path" "$url"
    fi

    # Verify the hash of the downloaded file.
    local actual_hash=$(sha256sum "$path" | awk '{print $1}')
    if [[ -v NO_HASH ]]; then
        status "info" "Hash for $filename: $actual_hash"
    else
        if [ "$actual_hash" != "$hash" ]; then
            status "error" "Hash mismatch for $filename. Expected: $hash, Actual: $actual_hash"
            exit 1
        fi
        status "info" "Hash verified: $filename"
    fi

    DOWNLOAD_FILE="$path"
}

# Extracts a tarball to an installation directory, omitting the top-level directory.
# Sets INSTALL_DIR to the absolute path to the installation directory.
extract_tar_gz() {
    local name="$1"
    local tar_gz_file="$2"
    local filename=$(basename "$tar_gz_file")

    status "action" "Extracting: $filename"
    INSTALL_DIR="$SOURCES_DIR/$name"
    mkdir -p "$INSTALL_DIR"
    tar -xzf "$tar_gz_file" -C "$INSTALL_DIR" --strip-components=1
}

# Extracts a zip file to an installation directory, omitting the top-level directory.
# Sets INSTALL_DIR to the absolute path to the installation directory.
extract_zip() {
    local name="$1"
    local zip_file="$2"
    local filename=$(basename "$zip_file")

    status "action" "Extracting: $filename"
    INSTALL_DIR="$SOURCES_DIR/$name"
    mkdir -p "$INSTALL_DIR"
    unzip -q "$zip_file" -d "$INSTALL_DIR"

    # Remove the top-level directory.
    local top_level_dir=$(ls -d "$INSTALL_DIR"/*)
    mv "$top_level_dir"/* "$INSTALL_DIR"
    rmdir "$top_level_dir"
}

# Installs cmake.
install_cmake() {
    status "header" "cmake"

    local native_tar_gz=""
    local native_zip=""

    download \
        "cmake-mac-$CMAKE_VERSION.tar.gz" \
        "$CMAKE_MAC_HASH" \
        "https://github.com/Kitware/CMake/releases/download/v$CMAKE_VERSION/cmake-$CMAKE_VERSION-macos-universal.tar.gz" \
        "https://brianluft-mirror.com/cmake/cmake-mac-$CMAKE_VERSION.tar.gz"

    if [ "$OS" == "mac" ]; then
        native_tar_gz=$DOWNLOAD_FILE
    fi

    download \
        "cmake-linux-glibc-aarch64-$CMAKE_VERSION.tar.gz" \
        "$CMAKE_LINUX_ARM64_HASH" \
        "https://github.com/Kitware/CMake/releases/download/v$CMAKE_VERSION/cmake-$CMAKE_VERSION-linux-aarch64.tar.gz" \
        "https://brianluft-mirror.com/cmake/cmake-linux-glibc-aarch64-$CMAKE_VERSION.tar.gz"

    if [ "$OS_ARCH" == "linux-arm64" ]; then
        native_tar_gz=$DOWNLOAD_FILE
    fi

    download \
        "cmake-linux-glibc-x86_64-$CMAKE_VERSION.tar.gz" \
        "$CMAKE_LINUX_X64_HASH" \
        "https://github.com/Kitware/CMake/releases/download/v$CMAKE_VERSION/cmake-$CMAKE_VERSION-linux-x86_64.tar.gz" \
        "https://brianluft-mirror.com/cmake/cmake-linux-glibc-x86_64-$CMAKE_VERSION.tar.gz"

    if [ "$OS_ARCH" == "linux-x64" ]; then
        native_tar_gz=$DOWNLOAD_FILE
    fi

    download \
        "cmake-$CMAKE_VERSION-windows-arm64.zip" \
        "$CMAKE_WINDOWS_ARM64_HASH" \
        "https://github.com/Kitware/CMake/releases/download/v$CMAKE_VERSION/cmake-$CMAKE_VERSION-windows-arm64.zip" \
        "https://brianluft-mirror.com/cmake/cmake-$CMAKE_VERSION-windows-arm64.zip"

    if [ "$OS_ARCH" == "windows-arm64" ]; then
        native_zip=$DOWNLOAD_FILE
    fi

    download \
        "cmake-$CMAKE_VERSION-windows-x86_64.zip" \
        "$CMAKE_WINDOWS_X64_HASH" \
        "https://github.com/Kitware/CMake/releases/download/v$CMAKE_VERSION/cmake-$CMAKE_VERSION-windows-x86_64.zip" \
        "https://brianluft-mirror.com/cmake/cmake-$CMAKE_VERSION-windows-x86_64.zip"

    if [ "$OS_ARCH" == "windows-x64" ]; then
        native_zip=$DOWNLOAD_FILE
    fi

    # On musl-based Linux, we expect the system to provide cmake.
    if [ "$LINUX_LIBC" == "musl" ]; then
        # Check that cmake is installed.
        if ! command -v cmake &> /dev/null; then
            status "error" "cmake could not be found. On musl-based Linux, cmake must be preinstalled."
            exit 1
        fi
        return
    fi

    # On all other systems, proceed to extract so we can use this local cmake.
    if [ ! -z "$native_tar_gz" ]; then
        extract_tar_gz "cmake" "$native_tar_gz"
    elif [ ! -z "$native_zip" ]; then
        extract_zip "cmake" "$native_zip"
    else
        status "error" "No cmake found for $OS_ARCH."
        exit 1
    fi

    # On Mac, this is an app bundle that we have to dig the CLI tool out of.
    if [ "$OS" == "mac" ]; then
        mv -f "$INSTALL_DIR/CMake.app/Contents/bin" "$INSTALL_DIR/"
        mv -f "$INSTALL_DIR/CMake.app/Contents/man" "$INSTALL_DIR/"
        mv -f "$INSTALL_DIR/CMake.app/Contents/share" "$INSTALL_DIR/"
        rm -rf "$INSTALL_DIR/CMake.app"
    fi

    # Move everything to the prefix directory, which is blank at this point because we install cmake first.
    status "action" "Moving to prefix directory: $PREFIX_DIR"
    mv -f "$INSTALL_DIR"/* "$PREFIX_DIR"

    # Remove the installation directory.
    status "action" "Removing temporary directory: $INSTALL_DIR"
    rm -rf "$INSTALL_DIR"

    # Set CMAKE and make sure it exists.
    if [ "$OS" == "windows" ]; then
        CMAKE="$PREFIX_DIR/bin/cmake.exe"
    else
        CMAKE="$PREFIX_DIR/bin/cmake"
    fi
    echo "CMAKE=\"$CMAKE\"" >> "$CONFIG_FILE"

    if [ ! -f "$CMAKE" ]; then
        status "error" "cmake could not be found at $CMAKE"
        find "$PREFIX_DIR" -type f -name "cmake"
        exit 1
    fi

    status "success" "Installed cmake"
    cd "$ROOT_DIR"
}

# Installs tvision.
install_tvision() {
    status "header" "tvision"

    download \
        "tvision-$TVISION_VERSION.tar.gz" \
        "$TVISION_HASH" \
        "https://github.com/magiblot/tvision/archive/$TVISION_VERSION.tar.gz" \
        "https://brianluft-mirror.com/tvision/tvision-$TVISION_VERSION.tar.gz"

    extract_tar_gz "tvision" "$DOWNLOAD_FILE"

    mkdir -p "$INSTALL_DIR/build"
    cd "$INSTALL_DIR/build"

    status "action" "Generating cmake files..."
    if [ "$OS" == "windows" ]; then
        "$CMAKE" \
            -G "Visual Studio 17 2022" \
            -A "$WINDOWS_MSVC_ARCH" \
            -DCMAKE_PREFIX_PATH="$PREFIX_DIR" \
            -DCMAKE_INSTALL_PREFIX="$PREFIX_DIR" \
            -DTV_BUILD_EXAMPLES=OFF \
            ..
    else
        "$CMAKE" \
            -G "Unix Makefiles" \
            -DCMAKE_PREFIX_PATH="$PREFIX_DIR" \
            -DCMAKE_INSTALL_PREFIX="$PREFIX_DIR" \
            -DTV_BUILD_EXAMPLES=OFF \
            ..
    fi

    status "action" "Building tvision..."
    "$CMAKE" --build . --config Release

    status "action" "Installing tvision..."
    "$CMAKE" --install . --config Release

    status "success" "Installed tvision"
    cd "$ROOT_DIR"
}

# Restores dotnet tools.
restore_dotnet_tools() {
    status "header" "Restoring dotnet tools..."
    cd "$ROOT_DIR/src"
    dotnet tool restore
    status "success" "Restored dotnet tools"
    cd "$ROOT_DIR"
}

install_cmake # Must be first, as this creates the initial prefix directory.
install_tvision
restore_dotnet_tools
