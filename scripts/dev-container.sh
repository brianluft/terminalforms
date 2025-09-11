#!/bin/bash
set -euo pipefail
source "$( dirname "${BASH_SOURCE[0]}" )/env.sh"

cd $ROOT_DIR/scripts
docker build -t terminalforms-dev .

docker run -it --rm -v "$ROOT_DIR":/repo terminalforms-dev
