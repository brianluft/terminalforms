#!/usr/bin/env bash
set -euo pipefail

LEVEL="$1"
MESSAGE="$2"

# Based on LEVEL, print the message in the appropriate color.
# header: purple
# info: gray
# error: red
# action: cyan
# success: green

# ANSI color codes
PURPLE='\033[0;35m'
GRAY='\033[0;37m'
RED='\033[0;31m'
CYAN='\033[0;36m'
GREEN='\033[0;32m'
RESET='\033[0m'

case "$LEVEL" in
    "header")
        echo -e "${PURPLE}────── ${MESSAGE} ──────${RESET}"
        ;;
    "info")
        echo -e "${GRAY}${MESSAGE}${RESET}"
        ;;
    "error")
        echo -e "${RED}⛔ ${MESSAGE}${RESET}"
        ;;
    "action")
        echo -e "${CYAN}${MESSAGE}${RESET}"
        ;;
    "success")
        echo -e "${GREEN}✅ ${MESSAGE}${RESET}"
        ;;
    *)
        echo "Unknown level: $LEVEL"
        echo "Usage: $0 {info|error|action|success} \"message\""
        exit 1
        ;;
esac
