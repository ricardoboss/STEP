# AGENTS.md

## Dev environment tips

* Install the .NET 9 SDK (`dotnet`). Run `dotnet --info` and confirm an SDK version that starts with `9.`.
* Run the commands below from the repository root (where `STEP.sln` lives).
* `.editorconfig` defines code style and formatting rules that `dotnet format` will enforce.
* The CLI entry point is `StepLang.CLI/StepLang.CLI.csproj`.
* Creating `Release` builds is unsupported in this environment.

## Build instructions

* Restore dependencies and build the solution in Debug mode:

  ```sh
  dotnet restore
  dotnet build --configuration Debug
  ```

## Testing and formatting

* Match the CI pipeline locally:

  ```sh
  dotnet format --verify-no-changes
  dotnet test --configuration Test
  ```

## Run instructions

* Check the CLI version number (Debug builds report `99.99.99`):

  ```sh
  dotnet run --project ./StepLang.CLI/StepLang.CLI.csproj -- --version
  ```
* Run an example file from `StepLang/Examples/` (see matching `.in`/`.out` fixtures in `StepLang.Tests/Examples/` for
  expected prompts and output):

  ```sh
  dotnet run --project ./StepLang.CLI/StepLang.CLI.csproj -- StepLang/Examples/strings.step
  ```

## CI / Publish

* GitHub workflow files in `.github/workflows/` define the CI plan and publish process.
* The most important workflows are:

    * `dotnet.yml`
    * `verify.yml`
    * `publish.yml`
* Review these workflows to confirm the checks that must pass and the steps taken before publishing.

## Documentation

* Extensive documentation is available in the `docs/StepLang.Wiki` directory as Markdown files.
* Additional information on contributing can be found in `CONTRIBUTING.md`.

## Release notes requirement

* **Every change must add a one-line summary to the `# Unreleased` section of `CHANGELOG.md` before it is merged.**
* Prefix entries with `(internal)` if they only affect contributors, tooling, or infrastructure.
* Reviewers will block pull requests that do not include this update, so double-check before committing.
