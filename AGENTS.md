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

## Testing and formatting

* **Before proposing any change, run both checks and confirm they pass:**

  ```sh
  dotnet format --verify-no-changes
  dotnet test --configuration Test
  ```

* Do not propose changes that break either check.

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
* If unsure whether a change is user-facing or internal, mark it `(internal)`.

## Agent behavior

* Be direct. Lead with the answer or the code change, then explain only if needed.
* Never guess. If context is missing or something is ambiguous, stop and ask rather than fabricating an answer or making
  assumptions.
* Verify every change against the build and test commands below before proposing it.
* Follow existing code patterns, naming conventions, and formatting as established in the codebase and `.editorconfig`.

## Commit messages

* Use Conventional Commits format: `type: description`.
* Allowed types: `feat`, `fix`, `perf`, `refactor`, `test`, `docs`, `chore`, `ci`, `build`, `style`.
* The description must explain **why** the change was made, not what was changed. The diff shows the what; the commit message provides the context that the diff cannot.
* Use lowercase, imperative mood, no trailing period.
* If a commit includes a breaking change, add `!` after the type.

Examples:
* `fix: prevent crash when input file is empty`  (why: crash prevention)
* `perf: reduce parser allocations during large file processing`  (why: performance problem)
* `refactor: simplify token matching to prepare for regex support`  (why: future extensibility)

Bad examples:
* `fix: change null check in Parser.cs`  (this just restates the diff)
* `feat: add new method to StringHelper`  (says what, not why)
