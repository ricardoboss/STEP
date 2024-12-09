# Introduction

A code style is commonly referred to as the way code is written in the literal sense.
It specifies where and how certain tokens should be placed and dictates use of punctuation and white spaces.
As many programming languages support different styles of writing code, there also exist many different opinions on what
is "the correct" or "the best" way.
This code style guide is intended to reduce cognitive friction when reading code from different authors (with different
preferences for code style) and to establish a "one-true-way" to write STEP code.
It was inspired by PHPs [PSR-12](https://www.php-fig.org/psr/psr-12/).

Keeping a consistent code style among a projects source code helps understand the code at a glance and ensures a high
level of technical interoperability between shared code.

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

- Variable names SHOULD be declared in `camelCase` (including functions)
- All STEP keywords MUST be in lower case (DON'T: `IF` `If` `While` `ImPort`, DO: `if`, `while`, `import`)

## 1.4 Indenting

- Code MUST be indented using tabs (for
  accessibility; [ref1](https://adamtuttle.codes/blog/2021/tabs-vs-spaces-its-an-accessibility-issue/); [ref2](https://alexandersandberg.com/articles/default-to-tabs-instead-of-spaces-for-an-accessible-first-environment/))
- Code MUST use one tab for every level of indentation

# 2. Statements

## 2.1 Block structures

Block structures are used to group statements together.
They are used in `if`-statements, `while`-loops and anonymous code blocks.

This is an example of an `if`-statement:

```step
if (name != "John") {
	println("Hello, ", name)
} else {
	println("Not you again, John")
}
```

Notice how the braces (`{` and `}`) are placed on the same line as the `if` and `else`.

Since anonymous code blocks have no associated keyword, the braces are placed on their own line:

```step
{
	number a = 1
	number b = 2

	println(a + b)
}
```

## 2.2 Loops

Loops are used to repeat a block of code multiple times until a certain condition is met.

### 2.2.1 While loops

The most common loop is the `while`-loop:

```step
number i = 0
while (i < 10) {
	println(i)
	i = i + 1
}
```

### 2.2.2 Foreach loops

The foreach loop can be used to iterate over a list or map. With a list, you give the current item a local variable.
For example, given a list `names`, you could iterate with a variable `string name`, which will automatically be
assigned the respective value each time.

```step
list names = ["John", "Jane", "Joe"]
foreach (string name in names) {
    println(name)
}
```

You can also assign the index of the current item to a local variable, by using this syntax:

```step
foreach (number index: string name in names) {
    println(index, ": ", name)
}
```

Instead of a `list`, you can also iterate over a `map`:

```step
map ages = {
    "John": 42,
    "Jane": 39,
    "Joe": 21
}
foreach (string name: number age in ages) {
    println(name, ": ", age)
}
```

In this case, the local variables are assigned the key and value of the current item.

## 2.3 Functions

Functions are used to group statements together and to make them reusable.
Functions are declared using the `function` type:

```step
function add = (number a, number b) {
	return a + b
}
```

Functions can be called by using the function name followed by a list of arguments in parentheses:

```step
number result = add(1, 2)
```

# 3. Expressions

## 3.1 Operators

Operator styles are grouped by arity (number of operands).

When space is permitted around an operator, multiple spaces MAY be used for readability.

All operators not listed here are left undefined.

### 3.1.1 Unary operators

Unary operators are operators that take one operand.

Increment/decrement operators MUST NOT have a space between the operator and the operand:

```step
number i = 0
i++
i--
```

### 3.1.2 Binary operators

Binary operators are operators that take two operands.

Binary operators MUST be surrounded by a single space on either side:

```step
number a = 1 + 2
number b = a * 3
```

This also applies to assignment operators:

```step
number a = 1
a += 2
b /= 10
```
