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

# Examples

```step
toJson(1) // returns "1"
toJson("hello") // returns "\"hello\""
toJson(true) // returns "true"
toJson(null) // returns "null"
toJson([1, 2, 3]) // returns "[1, 2, 3]"
toJson({"a": 1, "b": 2}) // returns "{\"a\": 1, \"b\": 2}"
```
