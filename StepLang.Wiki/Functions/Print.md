# Description

`print` writes a value to `StdOut`.

# Syntax

```step
print(any ...value)
```

- `value` a list of values to be printed to `StdOut`.

# Remarks

- the given value is converted to a string using the [`toString`](./ToString.md) function.
- the function is variadic, so it can be called with any number of arguments.

# Examples

```step
print("Hello World!") // prints "Hello World!" to StdOut
print(1, 2, 3) // prints "123" to StdOut

var a = 1
var b = 2

print(a, b, a) // prints "121" to StdOut
```
