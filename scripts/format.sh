#!/usr/bin/env bash
set -euo pipefail

# Change to the repository root.
cd "$( dirname "${BASH_SOURCE[0]}" )"
cd ..

# Make all scripts executable. On Windows we can't just use chmod.
find scripts/ -type f -name "*.sh" -exec chmod +x {} \;
find scripts/ -type f -name "*.sh" -exec git update-index --chmod=+x {} \;
