#!/usr/bin/env bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"
cd $ROOT_DIR
clear
exec claude --dangerously-skip-permissions "$@"
