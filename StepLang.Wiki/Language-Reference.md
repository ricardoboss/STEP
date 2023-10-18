The STEP language is a simple, easy-to-learn language that is designed to be easy to use for beginners.

# Language Anatomy

## Tokens

The STEP language, like all programming languages, is made up of tokens.
Tokens are the smallest building blocks of a program.

A token is best explained by example.

The following is a simple STEP program:

```step
println("Hello, world!")
```

This program is made up of the following tokens:

- `println`
- `(`
- `"Hello, world!"`
- `)`

The first token is the name of the function that is called (`println`).
The second token is the opening parenthesis (`(`).
The third token is the string that is passed to the function (`"Hello, world!"`).
The fourth token is the closing parenthesis (`)`).

Between tokens, there can be whitespace (spaces, tabs, newlines, etc.).
They are ignored by the interpreter.

A token always has a type.
STEP has the following token types:

- identifier (e.g. `println`)
- literals:
  - string (e.g. `"Hello, world!"`)
  - number (e.g. `123`, `1.23` or `-1.2`)
  - boolean (`true` or `false`)
- built-in type names (`string`, `number`, `bool`, `function`, `list`, `map`)
- symbols (`(`, `)`, `{`, `}`, `[`, `]`, `,`, `;`, `:`, `=`, `_`, `+`, `-`, `*`, `^`, `~`, `/`, `%`, `!`, `?`, `&`, `|`, `<`, `>`)
- keywords:
  - `if`
  - `else`
  - `while`
  - `break`
  - `continue`
  - `return`
  - `import`
  - `foreach`
  - `in`
- comments (e.g. `// This is a comment`)

Keywords cannot be used as identifiers.
Neither can built-in type names nor literals.

## Expressions

An expression is a combination of tokens that can be evaluated to a value when the program is executed.

For example, the following is an expression:

```step
1 + 2
```

This expression will be evaluated to the value `3`.

Expressions can be combined to form more complex expressions.

For example, the following is a more complex expression:

```step
1 + 2 * (3 - 4)
```

This expression will be evaluated to the value `-1`.

Expressions consist of operators and operands.
Operators are the tokens that combine the operands.

For example, in the expression `1 + 2`, the operator is `+` and the operands are `1` and `2`.

The following operators are supported by STEP:

Binary operators (taking two operands):

- `a + b` (addition)
- `a - b` (subtraction)
- `a * b` (multiplication)
- `a / b` (division)
- `a % b` (modulo)
- `a ^ b` (power)
- `a < b` (less than)
- `a > b` (greater than)
- `a <= b` (less than or equal to)
- `a >= b` (greater than or equal to)
- `a == b` (equal to)
- `a != b` (not equal to)
- `a ?? b` (null coalescing)

Unary operators (taking one operand):

- `! a` (logical not)

## Statements

A statement is a combination of tokens that can be executed on its own when the program is executed.
A program is made up of one or more statements.

For example, the following is a statement:

```step
println("Hello, world!")
```

This statement will print `Hello, world!` to the terminal.

The logic is as follows:

- The interpreter looks up the definition of `println` and expects to find a function.
- The expression `"Hello, world!"` evaluates to a literal string.
- The interpreter calls the function `println` with the string as an argument.
- The function prints the string to the standard output (most likely the terminal).

Using different statements allows you to build complex programs.

Statements, like Tokens, have a type.

STEP has the following statement types:
- variable declaration (e.g. `number a = 1`)
- variable assignment (e.g. `a = 2`)
- function call (e.g. `println("Hello, world!")`)
- if statement (e.g. `if (a == 1) { println("a is 1") }`)
- if-else statement (e.g. `if (a == 1) { println("a is 1") } else { println("a is not 1") }`)
- while loop (e.g. `while (a < 10) { println(a) }`)
- anonymous code block (e.g. `{ println("Hello, world!") }`)
- return statement (e.g. `return 1`)
- discard assignment (e.g. `_ = calculate()`)
- break statement (e.g. `break`)
- continue statement (e.g. `continue`)
- index assignment (e.g. `a[0] = 1`)
- import statement (e.g. `import "path/to/file.step"`)

# Built-ins

The STEP language includes some built-in functions to interact with the environment in which a program is run in.

## Types

### `string`

The `string` type is used to represent a string of characters.

```step
string a = "Hello, world!"
```

This program will create a variable `a` of type `string` with the value `Hello, world!`.

### `number`

The `number` type is used to represent a number.

```step
number a = 1
number b = 2.3
number c = -4.5
```

This program will create three variables of type `number` with the values `1`, `2.3` and `-4.5`.

### `bool`

The `bool` type is used to represent a boolean value.

```step
bool a = true
bool b = false
```

This program will create two variables of type `bool` with the values `true` and `false`.

### `function`

The `function` type is used to represent a function.

```step
function calc = (number a, number b) {
  return a + b
}
```

This program will create a variable `calc` of type `function` with the value of a function that takes two numbers and returns their sum.

### `list`

The `list` type is used to represent a list of values.

```step
list a = [1, 2, 3]

a[0] = 4
```

This program will create a variable `a` of type `list` with the value `[1, 2, 3]`.
It will then assign the value `4` to the first element of the list.

### `map`

The `map` type is used to represent a mapping from keys to values.

```step
map a = {
  "key1": 1,
  "key2": 2,
  "key3": 3
}

a["key1"] = 4
```

This program will create a variable `a` of type `map` with the value `{"key1": 1, "key2": 2, "key3": 3}`.
It will then assign the value `4` to the key `"key1"`.

## Constants

### `pi`

The `pi` constant is a number that represents the mathematical constant π.
It represents the ratio of a circle's circumference to its diameter.

```step
println(pi) // prints 3.141592653589793
```

### `e`

The `e` constant is a number that represents the mathematical constant e.
It represents the base of the natural logarithm.

```step
println(e) // prints 2.718281828459045
```

### `EOL`

The `EOL` constant is a string that contains the end-of-line character(s) for the current platform.

```step
println("Hello", EOL, "world!")
```

This program will print `Hello` and `world!` on separate lines.

### `null`

The `null` constant is a special value that represents the absence of a value.
You can check for `null` using the `??` (null coalescing) operator or using the `toTypeName` function.

```step
string a = readline()

if (a ?? "null" != "null") {
  println("You entered: ", a)
} else {
  println("You didn't enter anything!")
}

if (toTypeName(a) == "null") {
  println("You didn't enter anything!")
} else {
  println("You entered: ", a)
}
```

## Functions

Check out the [Functions](https://github.com/ricardoboss/STEP/tree/main/StepLang.Wiki/Functions) folder for more
information on the built-in functions.
