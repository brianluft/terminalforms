#!/usr/bin/env bash
set -euo pipefail

# Change to the repository root.
cd "$( dirname "${BASH_SOURCE[0]}" )"
cd ..

# Set OS, ARCH, etc.
source "build/config.sh"

dotnet test src/Tests/Tests.csproj --runtime "$RID" --no-build

