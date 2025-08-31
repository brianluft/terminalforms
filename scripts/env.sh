#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$( dirname "${BASH_SOURCE[0]}" )" && cd .. && pwd)
export PATH="$ROOT_DIR/build/prefix/bin:$PATH"

# Set OS (mac, linux, windows), ARCH (arm64, x64), and OS_ARCH ($OS-$ARCH).
# On Linux, set LINUX_LIBC (glibc, musl)
ARCH=""
LINUX_LIBC=""
case "$(uname -s)" in
    Darwin)
        OS="mac"
        ARCH="$(uname -m)"
        ;;
    Linux)
        OS="linux"
        ARCH="$(uname -m)"
        if ldd --version 2>&1 | grep -qi musl; then
            LINUX_LIBC="musl"
        else
            LINUX_LIBC="glibc"
        fi
        ;;
    CYGWIN*|MINGW*|MSYS*)
        OS="windows"
        # Use PowerShell to get architecture
        ARCH=$("powershell.exe" -Command '(Get-CimInstance Win32_OperatingSystem).OSArchitecture.Substring(0, 3)' | tr -d '\r')

        # Normalize architecture names
        case "$ARCH" in
            ARM)
                ARCH="arm64"
                ;;
            *)
                ARCH="x64"
                ;;
        esac
        ;;
    *)
        echo "Unknown OS: ${UNAME_RESULT}"
        exit 1
        ;;
esac

case "$ARCH" in
    x86_64|x64)
        ARCH="x64"
        ;;
    arm64|aarch64)
        ARCH="arm64"
        ;;
    *)
        echo "Unknown architecture: ${ARCH}"
        exit 1
        ;;
esac

# Set OS_ARCH.
OS_ARCH="$OS-$ARCH"

# Set WINDOWS_MSVC_ARCH to "ARM64" or "x64" for Visual Studio.
WINDOWS_MSVC_ARCH=""
if [ "$OS" == "windows" ]; then
    if [ "$ARCH" == "arm64" ]; then
        WINDOWS_MSVC_ARCH="ARM64"
    else
        WINDOWS_MSVC_ARCH="x64"
    fi
fi

# Set RID and CMAKE_FLAGS.
CMAKE_FLAGS=""
if [ "$OS" == "windows" ]; then
    RID="win-$ARCH"
elif [ "$OS" == "mac" ]; then
    RID="osx-$ARCH"
    if [ "$MAC_ARCH" == "x64" ]; then
        CMAKE_FLAGS="-DCMAKE_OSX_ARCHITECTURES=x86_64 -DCMAKE_OSX_DEPLOYMENT_TARGET=13.0"
    elif [ "$MAC_ARCH" == "arm64" ]; then
        CMAKE_FLAGS="-DCMAKE_OSX_ARCHITECTURES=arm64 -DCMAKE_OSX_DEPLOYMENT_TARGET=13.0"
    else
        status "error" "Invalid MAC_ARCH: $MAC_ARCH"
        exit 1
    fi
else
    RID="linux-$ARCH"
fi

# If $CONFIGURATION is not set, set it to "Debug".
if [ -z "${CONFIGURATION:-}" ]; then
    CONFIGURATION="Debug"
fi

# ANSI color codes
PURPLE='\033[0;35m'
GRAY='\033[0;90m'
RED='\033[0;31m'
CYAN='\033[0;36m'
GREEN='\033[0;32m'
RESET='\033[0m'

echo -e "${GRAY}OS=$OS • ARCH=$ARCH • CONFIGURATION=$CONFIGURATION${RESET}"

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
