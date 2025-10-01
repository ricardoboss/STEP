# Description

The `toJson` function converts a STEP value to a JSON string.

# Syntax

```step
toJson(any value)
```

- `value` can be any STEP value (`number`, `string`, `bool`, `list`, `map`, `null`).

# Remarks

- the `toJson` function is the inverse of the [`fromJson`](./FromJson.md) function
- the returned value is always a string
- complex values (`list`, `map`) are serialized with indentation and new lines so the
  output is human-readable JSON rather than a single line

# Examples

```step
toJson(1) // returns "1"
toJson("hello") // returns "\"hello\""
toJson(true) // returns "true"
toJson(null) // returns "null"
toJson([1, 2, 3]) // returns "[\n  1,\n  2,\n  3\n]"
toJson({"a": 1, "b": 2}) // returns "{\n  \"a\": 1,\n  \"b\": 2\n}"
```
