#!/usr/bin/env bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"
cd $ROOT_DIR

EXE="build/dotnet-artifacts/bin/TerminalFormsDemo/${CONFIGURATION_LOWERCASE}_${RID}/TerminalFormsDemo"

echo -e "Running: ${GREEN}$EXE${RESET}"

# On Linux, when generating expected output (--output flag), use PtyRunner
# to ensure correct 40x12 terminal size. This is needed because tvision
# requires a real terminal to initialize the screen buffer.
if [[ "$OSTYPE" == "linux"* ]] && [[ "$*" == *"--output"* ]]; then
    dotnet run --project "$ROOT_DIR/src/PtyRunner" -- "$EXE" "$@"
else
    "$EXE" "$@"
fi
