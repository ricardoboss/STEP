> # **Work In Progress**
>
> The current state of this document does not reflect the final design.
> It is merely a draft.

A code style is commonly referred to as the way code is written in the literal sense.
It specifies where and how certain tokens should be placed and dictates use of punctuation and white spaces.
As many programming languages support different styles of writing code, there also exist many different opinions on what is "the correct" or "the best" way.
This code style guide is intended to reduce cognitive friction when reading code from different authors (with different preferences for code style) and to establish a "one-true-way" to write STEP code.
It was inspired by PHPs [PSR-12](https://www.php-fig.org/psr/psr-12/).

Keeping a consistent code style among a projects source code helps understand the code at a glance and ensures a high level of technical interoperability between shared code.

# 1. General

## 1.1 Files

- Files MUST use only UTF-8 encoding without BOM for STEP code
- Files MUST end with the `.step` extension
- Files SHOULD use Unix LF (linefeed) line ending

## 1.2 Lines

- Lines SHOULD not be longer than 80 characters; longer lines should be wrapped
- There MUST NOT be trailing whitespace at the end of lines
- Blank lines MAY be added to improve readability and to indicate related blocks of code
- A line MUST not contain more than one statement

## 1.3 Casing

- Variable names should be declared in `camelCase` (including functions)
- All STEP keywords must be in lower case (DON'T: `IF` `If` `While` `ImPort`, DO: `if`, `while`, `import`)

## 1.4 Indenting

- Code MUST be indented using tabs (for accessibility; [ref1](https://adamtuttle.codes/blog/2021/tabs-vs-spaces-its-an-accessibility-issue/); [ref2](https://alexandersandberg.com/articles/default-to-tabs-instead-of-spaces-for-an-accessible-first-environment/))

# 2. Statements

## 2.1 `if` and `else`

An `if` structure looks like the following.
Note the placement of parentheses, indentation and braces.
Also notice that the `if` and `else` keywords are on the same line as the braces.

```step
if (name != "John") {
	println("Hello, ", name)
} else {
	println("Not you again, John")
}
```

