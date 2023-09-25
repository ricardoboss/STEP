# STEP CLI (Command Line Interface)

## Installation

### Pre-built Binaries

1. Download the latest release from the [releases page](https://github.com/ricardoboss/STEP/releases)
   - If you're on Windows, download the `step-win-x64.exe` file.
   - If you're on Linux, download the `step-linux-x64` file.
   - If you're on macOS, download the `step-osx-x64` file.
   - If you're on an ARM-based system, download the `step-<platform>-arm64` file.
2. Rename the file to `step` (or `step.exe` on Windows).
3. Move the file to a directory that is in your `PATH` environment variable.
   - On Windows, you can move the file to `C:\Windows` or `C:\Windows\System32`.
   - On Linux, you can move the file to `/usr/bin` or `/usr/local/bin`.
   - On macOS, you can move the file to `/usr/local/bin`.
4. Open a new terminal and run `step --version` to verify that the installation was successful.
   - It should print the version of STEP that you installed.
   - If it prints an error, make sure that the file is in a directory that is in your `PATH` environment variable.

### Platform independent

1. Install the latest [.NET SDK](https://dotnet.microsoft.com/download/dotnet)
2. Download the latest platform independent release (`step.zip`) from the [releases page](https://github.com/ricardoboss/STEP/releases)
3. Extract the contents of the zip file to a directory of your choice
4. Add the directory to your `PATH` environment variable
5. Open a new terminal and run `step --version` to verify that the installation was successful.
   - It should print the version of STEP that you installed.
   - If it prints an error, make sure that the file is in a directory that is in your `PATH` environment variable.

### From Source

1. Install the latest [.NET SDK](https://dotnet.microsoft.com/download/dotnet)
2. Clone the repository
3. Run `dotnet run --project StepLang.CLI/StepLang.CLI.csproj --version` in the root directory of the repository
4. If the command prints the version of STEP, the installation was successful.

### From package managers

Currently, there are no packages available for STEP on any package managers.
An proposal add this is [here](https://github.com/users/ricardoboss/projects/2/views/3?sliceBy%5Bvalue%5D=_noValue&pane=issue&itemId=37606480).

## Usage

```
step <command or path> [options]
```

### Commands

- `run <file>`: Run a .step file
- `format <file-or-dir>`: Format a .step file or a directory of .step files

### Options

- `--version`: Print the version number
- `--info`: Print the version number and other information
- `-h`, `--help`:  Show help and usage information
