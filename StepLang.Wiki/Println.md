# Description

`println` writes a value to `StdOut` and appends a new line.

# Syntax

```step
println(any ...value)
```

- `value` a list of values to be printed to `StdOut`.

# Remarks

- the given value is converted to a string using the [`toString`](./ToString.md) function
- the function is variadic, so it can be called with any number of arguments
- the appended new line is environment specific (`\r\n` on Windows and `\n` otherwise)

# Examples

```step
println("Hello World!") // prints "Hello World!\r\n" to StdOut
print(1, 2, 3) // prints "123\r\n" to StdOut

var a = 1
var b = 2

print(a, b, a) // prints "121\r\n" to StdOut
```
