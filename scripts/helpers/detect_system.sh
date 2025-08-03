#!/usr/bin/env bash
set -euo pipefail

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

# Normalize mac and linux architectures if needed
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
