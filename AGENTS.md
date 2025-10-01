# AGENTS.md

## Dev environment tips

* Requires the .NET 9 SDK (`dotnet`). Run `dotnet --info` if you need to confirm the version.
* The repository root contains the solution file (`STEP.sln`).
* `.editorconfig` defines code style and formatting rules.
* The CLI entry point is `StepLang.CLI/StepLang.CLI.csproj`.
* Creating `Release` builds is unsupported in this environment.

## Build instructions

* Restore dependencies and build the solution in debug mode:

  ```sh
  dotnet restore
  dotnet build --configuration Debug
  ```

## Testing instructions

* Run all test projects (matches CI configuration):

  ```sh
  dotnet test --configuration Test
  ```
* All tests must pass before merging.
* Use `dotnet format --verify-no-changes` to ensure code style compliance. This must succeed before committing (warnings are allowed).

## Run instructions

* Check the CLI version number (Debug builds report `99.99.99`):

  ```sh
  dotnet run --project ./StepLang.CLI/StepLang.CLI.csproj -- --version
  ```
* Run an example file from `StepLang/Examples/` (see matching `.in`/`.out` fixtures in `StepLang.Tests/Examples/` for
  expected prompts and output):

  ```sh
  dotnet run --project ./StepLang.CLI/StepLang.CLI.csproj -- StepLang/Examples/<filename>
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
* When finalizing your changes, always add a one-liner summary of what you changed to `CHANGELOG.md`
