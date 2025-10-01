# AGENTS.md

## Dev environment tips

* .NET 9 SDK is available as `dotnet`.
* Repository root contains the solution file (`*.sln`).
* `.editorconfig` defines code style and formatting rules.
* Main entrypoint is the CLI project at `StepLang.CLI/StepLang.CLI.csproj`.
* You cannot create `Release` builds, because that requires a regular clone of the repository.

## Build instructions

* Build the solution in debug mode:

  ```sh
  dotnet build --configuration Debug
  ```

## Testing instructions

* Run all test projects:

  ```sh
  dotnet test --configuration Test
  ```
* All tests must pass before merging.
* Use `dotnet format --verify-no-changes` to ensure code style compliance. This must succeed before committing (warnings allowed).

## Run instructions

* Check the version:

  ```sh
  dotnet run --project ./StepLang.CLI/StepLang.CLI.csproj -- --version
  ```
* Run an example file from `StepLang/Examples/`:

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
