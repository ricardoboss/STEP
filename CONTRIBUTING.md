# Contributing to STEP

Thank you for considering contributing to STEP!
We value your input and appreciate your efforts to make our project better.

## Table of Contents

1. [Ways to Contribute](#ways-to-contribute)
2. [Code of Conduct](#code-of-conduct)
3. [Getting Started](#getting-started)
    - [Forking the Repository](#forking-the-repository)
    - [Setting Up Your Local Environment](#setting-up-your-local-environment)
4. [Making Changes](#making-changes)
    - [Creating a Feature Branch](#creating-a-feature-branch)
    - [Coding Guidelines](#coding-guidelines)
5. [Submitting a Pull Request](#submitting-a-pull-request)
6. [Code Review](#code-review)
7. [Community](#community)
8. [Licensing](#licensing)
9. [Feedback and Questions](#feedback-and-questions)

## Ways to Contribute

There are several ways you can contribute to STEP:

- Reporting issues and suggesting improvements through GitHub Issues.
- Submitting code enhancements or bug fixes through Pull Requests.
- Improving our documentation.

<!-- - Helping others by participating in discussions and answering questions. -->

- Check out the [`good first issue` issues](https://github.com/ricardoboss/STEP/contribute) to choose an issue to work
  on.

## Code of Conduct

Please read and adhere to our [Code of Conduct](CODE_OF_CONDUCT.md) to ensure a positive and inclusive environment for
all contributors.

## Getting Started

### Forking the Repository

1. Fork this repository to your GitHub account.
2. Clone your forked repository to your local machine:
   ```bash
   git clone https://github.com/<username>/STEP.git
   ```

### Setting Up Your Local Environment

1. Install [.NET](https://dotnet.microsoft.com/en-us/download)
2. (optional) Get an IDE and set it up
    - [JetBrains Rider](https://www.jetbrains.com/rider/)
    - [Microsoft Visual Studio](https://visualstudio.microsoft.com/)
3. Open your cloned repository

## Making Changes

### Creating a Feature Branch

1. Create a new branch for your feature or bug fix:
   ```bash
   # if an issue you want to fix exists, use issues/<id>-<description>:
   git checkout -b issues/26-contribution-docs
   
   # if no issue exists or is not applicable, choose an appropriate category and give it a short description:
   git checkout -b meta/change-project-name
   # or:
   git checkout -b workflow/add-build-scripts
   ```
2. Commit your changes
    - Make sure your changes are not out-of-scope for the issue you are working on
    - Create small commits and put the idea behind each change in the commit message

3. Write tests
    - To add integration tests:
        - create a new file in the `StepLang/Examples` folder
        - Add a file with the same name and the extension `.step.out` with the expected output in the
          `StepLang.Tests/Examples` folder
    - To add unit tests, create new Xunit tests in the appropriate `StepLang.Tests` folder

### Coding Guidelines

Make sure your code is properly formatted and follows our coding guidelines:

- Format your code using `dotnet format`
- Follow
  the [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Follow the [.NET Design Guidelines](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/)

## Submitting a Pull Request

1. Commit and push your changes to your forked repository.
2. Create a Pull Request from your branch to the main repository's `main` branch.
3. Provide a clear description of your changes and the problem it solves.

## Code Review

- Your Pull Request will undergo review by maintainers and contributors.
- Address feedback and make necessary changes.

## Community

Join our community on [Discord](https://discord.gg/ySpmcdCqFN) to connect with other contributors, ask questions, and
discuss development.

In case you want to contact the maintainers privately, you can reach out to us here:

- E-Mail:
    - Ricardo Boss: [mail@ricardoboss.de](mailto:mail@ricardoboss.de?subject=STEP%20Contribution)
- Discord:
    - Ricardo boss: [mizzle_de](https://discord.com/users/158966029286899713)

## Licensing

When you submit a contribution to the STEP project, you are agreeing to license your code under the same open-source
license as the rest of the project, as specified in our project's license file (LICENSE.md).

While you retain the copyright to your code, it will be available to the public under the project's license terms.
Ensure that any third-party code or libraries in your contribution are compatible with our project's license.

Note that the project maintainers may update the license in the future, and contributions made after a license change
will be subject to the new terms. Your submission acknowledges and accepts these licensing terms.

Thank you for your contribution to STEP!

## Feedback and Questions

If you have any questions or need assistance, feel free to reach out on [Discord](https://discord.gg/ySpmcdCqFN) or open
an issue on GitHub.

We appreciate your contributions to make STEP better for everyone!
