# Description

The `startsWith` function returns true if the string starts with the specified value.

# Syntax

```step
startsWith(string subject, string prefix)
```

- `subject` is the string to check.
- `prefix` is the value to check for.

# Remarks

- The comparison is case-sensitive.

# Examples

```step
startsWith("Hello World", "Hello") // True
startsWith("Hello World", "World") // False
```
