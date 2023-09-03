# Description

The `toKeys` function returns a list of keys from a map.

## Syntax

```step
toKeys(map myMap)
```

- `myMap` is the map to get the keys from.

# Remarks

- Only the first level of keys are returned. Nested keys are not returned.
- The order of the keys is not guaranteed.
- If the map is empty, an empty list is returned.

# Examples

```step
list keys = toKeys({a: 1, b: 2, c: 3}) // returns ["a", "b", "c"]
```
