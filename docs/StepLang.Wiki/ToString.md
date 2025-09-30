# Description

The `toString` function converts any value to a string.

# Syntax

```step
toString(any value)
```

- `value` is the value to convert to a string.

# Remarks

- If `value` is a string, it is returned as-is.

# Examples

```step
toString(123) // returns "123"
toString(true) // returns "True"
toString(false) // returns "False"
toString(null) // returns "null"
toString("Hello") // returns "Hello"
toString({"a": 1, "b": 2}) // returns "{\"a\":1,\"b\":2}"
toString([1, 2, 3]) // returns "[1, 2, 3]"
```
