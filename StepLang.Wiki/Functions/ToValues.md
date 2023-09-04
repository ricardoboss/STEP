# Description

The `toValues` function returns the values of a map as a list.

# Syntax

```step
toValues(map myMap)
```

- `myMap` is the map to get the values from.

# Remarks

- If `myMap` is empty, the function returns an empty list.
- The order of the elements in the list is not guaranteed.
- Only the first level of the map is considered. Nested map values are not included in the list.

# Examples

```step
list values = toValues({"a": 1, "b": 2, "c": 3}) // returns [1, 2, 3]
```
