# syntax=docker/dockerfile:1.4

# Ubuntu 24.04
FROM mcr.microsoft.com/devcontainers/base:noble

RUN <<EOF
    apt-get update

    apt-get install -y \
        clang-format-20 \
        dotnet-sdk-8.0 \
        libncurses-dev \
        tmux

    update-alternatives \
        --install /usr/bin/clang-format clang-format \
        /usr/bin/clang-format-20 100

    curl -fsSL https://claude.ai/install.sh | bash
EOF

# devcontainer.json will mount the host's ~/.claude to the container's /.claude
ENV CLAUDE_CONFIG_DIR=/.claude
