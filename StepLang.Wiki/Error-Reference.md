This page explains errors you may encounter while programming.

## `IncompatibleVariableTypeException`

This exception is thrown when a variable is assigned a value of an incompatible type.

For example:

```step
string myVariable = 1
```

This will throw an `IncompatibleVariableTypeException` because `myVariable` is of type `string`, but `1` is of type `number`.

To fix this, make sure the value assigned to the variable is of the same type as the variable:

```step
number myVariable = 1
```

Or:

```step
string myVariable = "1"
```

## `InvalidArgumentCountException`

This exception is thrown when a function is called with an invalid number of arguments.

For example:

```step
jsonDecode()
```

This will throw an `InvalidArgumentCountException` because `jsonDecode()` requires one argument.

Correct usage:

```step
string myJsonString = "{\"hello\": \"world\"}"

jsonDecode(myJsonString)
```

## `InvalidExpressionTypeException`

This exception is thrown when an expression is used in an invalid context.

For example:

```step
_ = typename("string")
```

This will throw an `InvalidExpressionTypeException` because `typename()` is expects an expression of type `variable`, not `string` as shown here.

Another example with a different expression:

```step
_ = typename(1 + 2)
```

In this case, the expression result of `1 + 2` is of type `number` (value: `3`), and not `variable`.

Correct usage:

```step
string myVariable = "string"

_ = typename(myVariable)
```

Or with a different expression:

```step
number myVariable = 1 + 2

_ = typename(myVariable)
```

## `InvalidResultTypeException`

This exception is thrown when an expression results in an unexpected type.

For example:

```step
string myVariable = 1 + 2
```

This will throw an `InvalidResultTypeException` because the expression `1 + 2` results in a `number` (value: `3`), and not a `string` as expected.

Another example:

```step
number myVariable = "hello world"
```

This will throw an `InvalidResultTypeException` because the expression `"hello world"` results in a `string` (value: `"hello world"`), and not a `number` as expected.

Correct usage:

```step
string myVariable = "1" + "2"
```

Or with a different expression:

```step
number myVariable = 1 + 2
```

## `ListIndexOutOfBoundsException`

This exception is thrown when a list is accessed with an invalid index.

For example:

```step
list myList = ["hello", "world"]

_ = myList[2]
```
The list `myList` has two elements, so the index `2` is out of bounds.
The bounds being all whole number from `0` to `1` (inclusive).

Correct usage:

```step
list myList = ["hello", "world"]

_ = myList[0]
```

## `UndefinedIdentifierException`

This exception is thrown when an identifier is used that was not declared.

For example:

```step
println(myVariable)
```

This will throw an `UndefinedIdentifierException` because `myVariable` was not declared.

Correct usage:

```step
string myVariable = "hello world"

println(myVariable)
```

## `IncompatibleExpressionOperandsException`

This exception is thrown when an expression is used with operands that have incompatible types.

For example:

```step
println(1 + "hello")
```

This will throw an `IncompatibleExpressionOperandsException` because the expression `1 + "hello"` has operands of type `number` and `string` respectively, which cannot be used with the `+` operator.

Correct usage:

```step
println("1" + "hello")
```

Or with a different expression:

```step
println(1 + 2)
```

## `InvalidArgumentTypeException`

This exception is thrown when a function is called with an argument of an unexpected type.

For example:

```step
jsonDecode(1)
```

This will throw an `InvalidArgumentTypeException` because `jsonDecode()` expects a `string` argument, not a `number` as shown here.

To fix this, pass an argument of the expected type:

```step
jsonDecode("{\"hello\": \"world\"}")
```

## `InvalidExpressionException`

This exception is thrown when an expression is invalid.

For example:

```step
number a = 1 + ()
```

This will throw an `InvalidExpressionException` because a pair of empty parentheses `()` is not a valid expression.

To fix this, either:

Replace the empty parentheses with a valid expression:

```step
number a = 1 + 2
```

or with a valid expression surrounded by parentheses:

```step
number a = 1 + (2)
```

## `InvalidFunctionCallException`

See [`InvalidArgumentCountException`](#invalidargumentcountexception) and [`InvalidArgumentTypeException`](#invalidargumenttypeexception).

## `InvalidIndexOperatorException`

This exception is thrown when the index operator `[]` is used in an invalid context.

For example:

```step
string a = "hello"

a[0] = "H"
```

This will throw an `InvalidIndexOperatorException` because the index operator `[]` cannot be used with a `string` to set values.

Or:

```step
list a = ["hello", "world"]

println(a["key"])
```

This will throw an `InvalidIndexOperatorException` because the index operator `[]` used on a `list` only accepts `number` indexes.

To fix this, make sure the variable you're trying to use the index operator on can be used with the index operator (`list` or `map`) and that the index value has the correct type (`number` for `list`s and `string` for `map`s).
`string` values can also be indexed using number indexes to read single character of a string, but not to write to a string.

## `ImportedFileDoesNotExistException`

This exception is thrown when a file that is imported does not exist.

If the path is absolute, make sure it exists.
If the path is relative, make sure it exists relative to the file that is importing it.

## `ImportedFileIsSelfException`

This exception is thrown when a file imports itself.

If this was not intended, check the path given to the `import` statement.

## `ImportsNoLongerAllowedException`

This exception is thrown when a file tries to import another file after the first statement.

For example:

```step
println("hello world")

import "other-file.step"
```

This will throw an `ImportsNoLongerAllowedException` because the `import` statement appears after the `println` statement in the file.

To fix this, move the `import` statement to the top of the file:

```step
import "other-file.step"

println("hello world")
```

## `InvalidVariableAssignmentException`

See [`IncompatibleVariableTypeException`](#incompatiblevariabletypeexception).

