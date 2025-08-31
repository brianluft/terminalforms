#!/usr/bin/env bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"
cd $ROOT_DIR

"build/dotnet-artifacts/bin/TerminalFormsDemo/debug_$RID/TerminalFormsDemo.exe"
