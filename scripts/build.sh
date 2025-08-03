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

# Set OS, ARCH, LINUX_LIBC, WINDOWS_MSVC_ARCH.
source "build/detected_system.sh"

# Builds tvision4c
build_tvision4c() {
    status "header" "tvision4c"

    #TODO

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
