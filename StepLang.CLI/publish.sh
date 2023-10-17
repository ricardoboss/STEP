#!/usr/bin/env bash

set -e

platforms=(
#  "linux-x64"
#  "win-x64"
#  "osx-x64"
#  "linux-arm64"
#  "win-arm64"
  "osx-arm64"
)
frameworkVersion="net7.0"

echo "🗑️ Cleaning up..."
dotnet clean --verbosity quiet --nologo

# clean publish folder
rm -rf publish
mkdir -p publish

echo "📦 Restoring dependencies..."
dotnet restore --verbosity quiet --nologo --locked-mode

# iterate over all platforms to build and publish each
for platform in "${platforms[@]}"; do
    echo "🔨 Building for $platform..."

    dotnet publish --configuration Release --runtime "$platform" --framework "$frameworkVersion" --verbosity quiet --nologo --self-contained true /p:PublishTrimmed=true /p:PublishSingleFile=true

    sourceFilename="step"
    if [[ "$platform" == "win-x64" || "$platform" == "win-arm64" ]]; then
        sourceFilename="$sourceFilename.exe"
    fi

    targetFilename="step-$platform"
    if [[ "$platform" == "win-x64" || "$platform" == "win-arm64" ]]; then
        targetFilename="$targetFilename.exe"
    fi

    outputPath="publish/$targetFilename"

    # move built binary to publish folder
    mv "bin/Release/$frameworkVersion/$platform/publish/$sourceFilename" "$outputPath"

    # clean up publish folder
    rm -rf "bin/Release/$frameworkVersion/$platform"

    chmod +x "$outputPath"

    echo -e "\t✅ built $outputPath"
done

echo "🔨 Building platform-independent binary..."

# build platform-independent binary
dotnet publish --configuration Release --framework "$frameworkVersion" --verbosity quiet --nologo

# remove platform-dependent binary
rm bin/Release/$frameworkVersion/publish/step

# zip framework-dependent libraries
zip -jq publish/step bin/Release/$frameworkVersion/publish/*

echo -e "\t✅ built publish/step.zip"

echo "✅ Done!"
