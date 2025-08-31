#!/usr/bin/env bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"
cd $ROOT_DIR

# Set CLANG_FORMAT to the path of the clang-format executable.
CLANG_FORMAT=""
find_clang_format() {
    # If clang-format is on the PATH, simply use that.
    if command -v clang-format &> /dev/null; then
        CLANG_FORMAT=$(command -v clang-format)
        return
    fi

    # If this is non-Windows, bail out.
    if [ "$OS" != "windows" ]; then
        status "error" "clang-format not found."
        exit 1
    fi

    # If this is Windows, we expect it has been installed via Visual Studio Installer.
    local vswhere="$PROGRAMFILES (x86)\Microsoft Visual Studio\Installer\vswhere.exe"
    local vsdir="$("$vswhere" -latest -property installationPath)"
    CLANG_FORMAT="$vsdir\\VC\\Tools\\Llvm\\$ARCH\\bin\\clang-format.exe"

    # Did we find it?
    if [ ! -f "$CLANG_FORMAT" ]; then
        status "error" "clang-format not found at $CLANG_FORMAT."
        exit 1
    fi
}
find_clang_format

# Format C++ code.
status "header" "clang-format"
find src/TerminalFormsNative/ -type f \( -iname \*.h -o -iname \*.cpp \) | xargs "$CLANG_FORMAT" -i

# Format C# code.
status "header" "csharpier"
(cd src && dotnet tool run csharpier format .)

# Make all scripts executable. On Windows we can't just use chmod.
status "header" "chmod +x"
find scripts/ -type f -name "*.sh" -exec chmod +x {} \;
find scripts/ -type f -name "*.sh" -exec git update-index --chmod=+x {} \;

status "success" "Formatting complete."
