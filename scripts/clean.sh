#!/usr/bin/env bash
set -euo pipefail

# Change to the repository root.
cd "$( dirname "${BASH_SOURCE[0]}" )"
cd ..

rm -rf \
    build/prefix \
    build/sources \
    ;
