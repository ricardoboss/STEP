# Description

`readline` reads a line from `StdIn` and returns it as a string.

# Syntax

```step
readline()
```

# Remarks

- the function is blocking until a line is read from `StdIn`
- the returned string does not contain the line terminator
- the function returns an empty string if `StdIn` cannot be read

# Examples

```step
var line = readline()

println("You entered: " + line)
```
