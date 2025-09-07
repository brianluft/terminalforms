#!/usr/bin/env bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"
cd $ROOT_DIR

EXE="build/dotnet-artifacts/bin/TerminalFormsDemo/${CONFIGURATION_LOWERCASE}_${RID}/TerminalFormsDemo.exe"

echo -e "Running: ${GREEN}$EXE${RESET}"

"$EXE" "$@"
