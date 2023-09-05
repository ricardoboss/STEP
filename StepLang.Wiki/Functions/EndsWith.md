# Description

The `endsWith` function returns true if the string ends with the specified value.

# Syntax

```step
endsWith(string subject, string suffix)
```

- `subject` is the string to check.
- `suffix` is the value to check for.

# Remarks

- The comparison is case-sensitive.

# Examples

```step
endsWith("Hello World", "World") // True
endsWith("Hello World", "Hello") // False
```
