# Description

`read` reads a single character from `StdIn` and returns it as a string.

# Syntax

```step
read()
```

# Remarks

- the function is blocking until a character is read from `StdIn`
- if `StdIn` cannot be read, the function returns an empty string
- it does not wait for the user to press "Enter", it returns directly after a single character is read
- the function doesn't mask the users input (so is not safe for passwords)

# Examples

```step
var c = read()

println("You pressed: " + c)
```
