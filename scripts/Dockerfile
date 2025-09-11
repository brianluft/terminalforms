FROM ubuntu:24.04

RUN apt-get update && apt-get install -y \
    build-essential \
    git \
    curl \
    bash \
    libicu74 \
    ncurses-dev
    
RUN curl -sSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh \
    && bash dotnet-install.sh --channel 9.0

ENV PATH=/root/.dotnet:/root/.dotnet/tools:$PATH
ENV DOTNET_ROOT=/root/.dotnet
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_NOLOGO=1
ENV CONFIGURATION=Debug

VOLUME /repo
WORKDIR /repo/scripts
