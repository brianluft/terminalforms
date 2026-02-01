#!/bin/bash
# Usage: attach.sh CONTAINER
# Opens a new shell inside the devcontainer from an external terminal.
set -euo pipefail

container=$(docker container ls | grep vsc-terminalforms | head -n 1 | awk '{ print $1 }')
echo "Attaching to $container."

exec docker exec \
    -ti \
    --user $UID \
    --workdir "/workspaces/terminalforms" \
    "$container" /bin/bash
