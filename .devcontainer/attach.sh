#!/bin/bash
# Usage: attach.sh CONTAINER
# Opens a new shell inside the devcontainer from an external terminal.
set -euo pipefail

exec docker exec \
    -ti \
    --user $UID \
    --workdir "/workspaces/terminalforms" \
    "$1" /bin/bash
