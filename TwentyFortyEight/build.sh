#!/usr/bin/env bash
set -e

# Install .NET 10 SDK
curl -fsSL https://dot.net/v1/dotnet-install.sh -o dotnet-install.sh
chmod +x dotnet-install.sh

./dotnet-install.sh --channel 10.0 --install-dir "$PWD/.dotnet"

export PATH="$PWD/.dotnet:$PATH"

# Publish Blazor WebAssembly
dotnet publish ./TwentyFortyEight/TwentyFortyEight.csproj \
    -c Release \
    -p:OverrideHtmlAssetPlaceholders=true \
    -o output


INDEX="output/wwwroot/index.html"

if grep -q '#\[.{fingerprint}\]' "$INDEX"; then
    echo "ERROR: HTML asset fingerprint placeholders were not replaced!"
    exit 1
fi

if grep -q '<link rel="preload" id="webassembly" />' "$INDEX"; then
    echo "ERROR: WebAssembly preload placeholder was not replaced!"
    exit 1
fi

echo "Blazor publish verification successful."