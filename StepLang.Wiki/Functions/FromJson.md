# Description

`fromJson` is a function that converts a JSON string to a STEP-type.

# Syntax

```step
fromJson(string json)
```

- `json` must be a string containing valid JSON.

# Remarks

- any type (`number`, `string`, `bool`, `null`, `list`, `map`) can be returned
- if the JSON string is invalid, `null` is returned
- if the JSON string is empty, `null` is returned
- a JSON object is converted to a STEP map (order is not guaranteed to be preserved)
- a JSON array is converted to a STEP list (order is preserved)

# Examples

```step
fromJson("{\"a\": 1, \"b\": 2}") // returns a map: {"a": 1, "b": 2}
fromJson("[1, 2, 3]') // returns a list: [1, 2, 3]
fromJson("{\"a\": 1, \"b\": 2") // returns null
```
