#!/usr/bin/env bash

# Sets the following variables:
# CMAKE_FLAGS
# CONFIGURATION (preserved if already set)
# CONFIGURATION_LOWERCASE
# NATIVE_ARCH
# OS
# OS_NATIVE_ARCH
# OS_TARGET_ARCH
# PATH
# RID
# ROOT_DIR
# TARGET_ARCH (preserved if already set)
# WINDOWS_MSVC_TARGET_ARCH

set -euo pipefail

ROOT_DIR=$(cd "$( dirname "${BASH_SOURCE[0]}" )" && cd .. && pwd)
export PATH="$ROOT_DIR/build/prefix/bin:$PATH"

# Set OS (mac, linux, windows), NATIVE_ARCH (arm64, x64), and OS_NATIVE_ARCH ($OS-$NATIVE_ARCH).
NATIVE_ARCH=""
case "$(uname -s)" in
    Darwin)
        OS="mac"
        NATIVE_ARCH="$(uname -m)"
        ;;
    Linux)
        OS="linux"
        NATIVE_ARCH="$(uname -m)"
        ;;
    CYGWIN*|MINGW*|MSYS*)
        OS="windows"
        # Use PowerShell to get architecture
        NATIVE_ARCH=$("powershell.exe" -Command '(Get-CimInstance Win32_OperatingSystem).OSArchitecture.Substring(0, 3)' | tr -d '\r')

        # Normalize architecture names
        case "$NATIVE_ARCH" in
            ARM)
                NATIVE_ARCH="arm64"
                ;;
            *)
                NATIVE_ARCH="x64"
                ;;
        esac
        ;;
    *)
        echo "Unknown OS: ${UNAME_RESULT}"
        exit 1
        ;;
esac

case "$NATIVE_ARCH" in
    x86_64|x64)
        NATIVE_ARCH="x64"
        ;;
    arm64|aarch64)
        NATIVE_ARCH="arm64"
        ;;
    *)
        echo "Unknown architecture: ${NATIVE_ARCH}"
        exit 1
        ;;
esac

# If TARGET_ARCH is already set, don't change it. Otherwise, set it to NATIVE_ARCH.
if [ -z "${TARGET_ARCH:-}" ]; then
    TARGET_ARCH="$NATIVE_ARCH"
fi

# Set OS_NATIVE_ARCH and OS_TARGET_ARCH.
OS_NATIVE_ARCH="$OS-$NATIVE_ARCH"
OS_TARGET_ARCH="$OS-$TARGET_ARCH"

# Set WINDOWS_MSVC_TARGET_ARCH for Visual Studio.
WINDOWS_MSVC_TARGET_ARCH=""
if [ "$OS" == "windows" ]; then
    if [ "$TARGET_ARCH" == "arm64" ]; then
        WINDOWS_MSVC_TARGET_ARCH="ARM64"
    elif [ "$TARGET_ARCH" == "arm" ]; then
        WINDOWS_MSVC_TARGET_ARCH="ARM"
    elif [ "$TARGET_ARCH" == "x86" ]; then
        WINDOWS_MSVC_TARGET_ARCH="Win32"
    else
        WINDOWS_MSVC_TARGET_ARCH="x64"
    fi
fi

# Set RID and CMAKE_FLAGS.
CMAKE_FLAGS=""
if [ "$OS" == "windows" ]; then
    RID="win-$TARGET_ARCH"
elif [ "$OS" == "mac" ]; then
    RID="osx-$TARGET_ARCH"
    if [ "$TARGET_ARCH" == "x64" ]; then
        CMAKE_FLAGS="-DCMAKE_OSX_ARCHITECTURES=x86_64 -DCMAKE_OSX_DEPLOYMENT_TARGET=13.0"
    elif [ "$TARGET_ARCH" == "arm64" ]; then
        CMAKE_FLAGS="-DCMAKE_OSX_ARCHITECTURES=arm64 -DCMAKE_OSX_DEPLOYMENT_TARGET=13.0"
    else
        status "error" "Invalid TARGET_ARCH: $TARGET_ARCH"
        exit 1
    fi
else
    RID="linux-$TARGET_ARCH"
fi

# If $CONFIGURATION is not set, set it to "Debug".
if [ -z "${CONFIGURATION:-}" ]; then
    CONFIGURATION="Debug"
fi

# Set CONFIGURATION_LOWERCASE to the lowercase version of $CONFIGURATION.
CONFIGURATION_LOWERCASE="$(echo "$CONFIGURATION" | tr '[:upper:]' '[:lower:]')"

# ANSI color codes
PURPLE='\033[0;35m'
GRAY='\033[0;90m'
RED='\033[0;31m'
CYAN='\033[0;36m'
GREEN='\033[0;32m'
RESET='\033[0m'

echo -e "Target: ${GREEN}$RID ($CONFIGURATION)${RESET}"

status() {
    LEVEL="$1"
    MESSAGE="$2"

    # Based on LEVEL, print the message in the appropriate color.
    # header: purple
    # info: gray
    # error: red
    # action: cyan
    # success: green

    case "$LEVEL" in
        "header")
            echo -e "${PURPLE}────── ${MESSAGE} ──────${RESET}"
            ;;
        "info")
            echo -e "${GRAY}${MESSAGE}${RESET}"
            ;;
        "error")
            echo -e "${RED}⛔ ${MESSAGE}${RESET}"
            ;;
        "action")
            echo -e "${CYAN}${MESSAGE}${RESET}"
            ;;
        "success")
            echo -e "${GREEN}✅ ${MESSAGE}${RESET}"
            ;;
        *)
            echo "Unknown level: $LEVEL"
            echo "Usage: $0 {info|error|action|success} \"message\""
            exit 1
            ;;
    esac
}
