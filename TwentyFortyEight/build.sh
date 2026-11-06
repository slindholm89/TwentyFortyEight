#!/usr/bin/env bash
set -e

# Install .NET 10 SDK
curl -fsSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh

./dotnet-install.sh --channel 10.0 --install-dir "$PWD/.dotnet"

export PATH="$PWD/.dotnet:$PATH"

# Publish Blazor WebAssembly
dotnet publish TwentyFortyEight.csproj \
    -c Release \
    -o output