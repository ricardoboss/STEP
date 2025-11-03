# Release Management

This document describes how releases are managed for STEP.

## Branch Model

| Branch        | Purpose                                                                                           | Examples                 |
|---------------|---------------------------------------------------------------------------------------------------|--------------------------|
| `main`        | Next **major** version under active development. All new features and breaking changes land here. | `main` → 3.0-dev         |
| `release/x.y` | Current **minor** line in stabilization (bug fixes, polish, no breaking changes).                 | `release/2.2`            |
| `maint/x.y`   | **Maintenance** line for an already released version. Only fixes and security updates.            | `maint/2.1`, `maint/2.0` |

All branches are protected. Force-pushes and direct commits are disallowed.

## Day-to-day flow

- Almost every change goes to `main` via pull requests (merged via rebase to keep linear history).  
  Conventional commits determine semantic meaning of change (for example `feat`, `perf`, `fix`).
- If a change should ship in the next minor version, add a label `backport:release/<version>` to the PR before or after
  merge.  
  The `backport.yml` workflow opens a cherry-pick PR to `release/<version>`.  
  Review and merge manually.
- Fixes for already released versions go to the matching `maint/x.y` first.  
  If relevant for `main`, cherry-pick forward and open a PR to `main`.
- PRs to `maint/*` are validated by the `maintenance.yml` workflow, which blocks commits starting with `feat:` or
  `perf:`.

## Pre-Release versions

To prepare a new minor or major release:

1. Cut a new `release/x.y` branch from main at the commit representing feature freeze. (For majors, `y` must be 0.)
2. Continue polishing and stabilizing on that branch.
   You may publish pre-releases (`vX.Y.0-alpha.N`, `-beta.N`, `-rc.N`) from this branch.
3. Only fixes and documentation changes are allowed once the first release candidate is tagged.

## Release ceremony

### Major Versions

Same flow as for Minor Versions, except that the `y` component must also be `0`.

### Minor Versions

When finalizing a new minor version, follow this process:

1. Check out the corresponding `release/x.y` branch.
2. Tag the final commit as `vX.Y.0`
3. Push the tag.  
   The `publish.yml` workflow will then proceed to build and publish the release artifacts and a GitHub release.
4. Rename the branch `release/<version>` to `maint/<version>` (create a new branch, delete the old one).
5. Any open PRs still targeting `release/x.y` should be closed or retargeted.

### Patch Versions

Only create patches on `maint/x.y` branches.
Tag releases normally (`vX.Y.Z`) and push it normally to trigger `publish.yml`.

## Changelog Policy

Every PR targeting `main`, `release/*` or `maint/*` must update `CHANGELOG.md` _or_ include the label `no changelog`.
The `verify.yml` workflow will fail if not done.

Allowed change types per branch:

| Target Branch | Allowed commit types                                        | Purpose                 |
|---------------|-------------------------------------------------------------|-------------------------|
| `main`        | `feat`, `fix`, `perf`, `refactor`, `docs`, `build`, `chore` | Next major development  |
| `release/x.y` | `fix`, `docs`, `refactor` (non-breaking)                    | Stabilization           |
| `maint/x.y`   | `fix`, `chore`, `security`                                  | Maintenance and patches |
