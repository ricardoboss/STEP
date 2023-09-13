# Description

The `contains` function returns `true` if the string, list or map contains the specified value, otherwise `false`.

# Syntax

```step
contains(string subject, string value)
contains(list subject, any value)
contains(map subject, any value)
```

- `subject`: The string, list or map to check.
- `value`: The value to check for.

# Remarks

- if `subject` is of type `string`, the function will check if the string contains the specified substring
- if `subject` is of type `list` or `map`, the function will check if it contains the specified value
- the internal logic uses [`indexOf`](./IndexOf.md) to determine if the value is contained in the subject

# Examples

```step
contains("Hello, World!", "Hello") // returns true
contains("Hello, World!", "hello") // returns false
```

```step
contains([1, 2, 3], 2) // returns true
contains([1, 2, 3], 4) // returns false
```

```step
contains({"a": 1, "b": 2}, 1) // returns true
contains({"a": 1, "b": 2}, 3) // returns false
```
