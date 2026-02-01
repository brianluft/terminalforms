# syntax=docker/dockerfile:1.4

# Ubuntu 24.04
FROM mcr.microsoft.com/devcontainers/base:noble

RUN <<EOF
    # GitHub CLI apt repository
    mkdir -p -m 755 /etc/apt/keyrings \
        && out=$(mktemp) && wget -nv -O$out https://cli.github.com/packages/githubcli-archive-keyring.gpg \
        && cat $out | tee /etc/apt/keyrings/githubcli-archive-keyring.gpg > /dev/null \
        && chmod go+r /etc/apt/keyrings/githubcli-archive-keyring.gpg \
        && mkdir -p -m 755 /etc/apt/sources.list.d \
        && echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" | tee /etc/apt/sources.list.d/github-cli.list > /dev/null

    apt-get update

    apt-get install -y \
        clang-format-20 \
        dotnet-sdk-8.0 \
        gh \
        libncurses-dev \
        tmux

    update-alternatives \
        --install /usr/bin/clang-format clang-format \
        /usr/bin/clang-format-20 100
EOF

USER vscode
RUN curl -fsSL https://claude.ai/install.sh | bash

# devcontainer.json will mount the host's ~/.claude to the container's /.claude
ENV CLAUDE_CONFIG_DIR=/.claude
