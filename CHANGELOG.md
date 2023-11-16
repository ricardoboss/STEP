# v2.1.0

* Changed "power" operator from `^` to `**` by @ricardoboss in https://github.com/ricardoboss/STEP/pull/107
* Added support for nullable type declarations by @ricardoboss in https://github.com/ricardoboss/STEP/pull/102
* Major rewrite of the parser and interpreter to achieve more abstraction and better error reporting by @ricardoboss in https://github.com/ricardoboss/STEP/pull/109
  * Added "parse" command to show the parsed abstract syntax tree
  * All variable declarations now support the nullability indicator (`?`)
  * Function definitions can now specify default parameter values
  * `break` and `continue` no longer allow expressions to break/continue for multiple loops
  * Exceptions that were previously printed with the affected code can now also show the causing token
  * Native functions now support union types to accept more than one value type of parameter

For more info, see https://github.com/ricardoboss/STEP/milestone/4?closed=1

# v2.0.0

* Use strong types for expression results by @ricardoboss in https://github.com/ricardoboss/STEP/pull/12
* Use JSON source generators to enable trimming by @ricardoboss in https://github.com/ricardoboss/STEP/pull/14
* Keep wiki sources in main repository by @ricardoboss in https://github.com/ricardoboss/STEP/pull/21
* Move license to its own file by @ricardoboss in https://github.com/ricardoboss/STEP/issues/27
* Added many functions to the standard library by @ricardoboss & @chucker in https://github.com/ricardoboss/STEP/issues/5
* Exceptions now include help links to helpful Docs by @ricardoboss in https://github.com/ricardoboss/STEP/issues/17
* Added a `format` command to the CLI by @ricardoboss in https://github.com/ricardoboss/STEP/issues/19
* Added contribution documentation by @ricardoboss in https://github.com/ricardoboss/STEP/issues/26

For more info, see https://github.com/ricardoboss/STEP/milestone/2?closed=1

# v1.0.0 - Initial release 🥳

* Added .NET library for parsing and interpreting STEP programs
* Added CLI for running STEP programs from files
* fix TestAdditiveMultiplicativePrecedencesWithParentheses by @chucker in https://github.com/ricardoboss/STEP/pull/1
* move examples to subdir and simplify `.csproj` by @chucker in https://github.com/ricardoboss/STEP/pull/2
* run all example `.hil` files as an integration test of sorts by @chucker in https://github.com/ricardoboss/STEP/pull/3
* Keep track of token locations and improve error reporting by @ricardoboss in https://github.com/ricardoboss/STEP/pull/8
* Add `import` statements by @ricardoboss in https://github.com/ricardoboss/STEP/pull/9
* Added string index operator by @ricardoboss in https://github.com/ricardoboss/STEP/pull/10

For more info, see https://github.com/ricardoboss/STEP/milestone/1?closed=1
