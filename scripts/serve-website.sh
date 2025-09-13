#!/usr/bin/env bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"
cd $ROOT_DIR/src
dotnet dotnet-serve --directory "$ROOT_DIR/build/website" -p 8080
