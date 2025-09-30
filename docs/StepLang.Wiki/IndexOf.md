# Description

`indexOf` returns the index of the first occurrence of a substring in a string or of an element in a list or the key of
the first occurence of a value in a map.

# Syntax

```step
indexOf(string subject, string substring)
indexOf(list subject, any element)
indexOf(map subject, any value)
```

- `subject`: the string, list or map to search in
- `substring`: the substring to search for in the string
- `element`: the element to search for in the list
- `value`: the value to search for in the map
- **returns**: the index of the first occurrence of the substring or `-1` if not found
- **returns**: the index of the first occurrence of the element or `-1` if not found
- **returns**: the key of the first occurrence of the value or `null` if not found

# Remarks

- `indexOf` is case-sensitive

# Examples

```step
indexOf("Hello World", "World") // returns 6
indexOf("Hello World", "world") // returns -1 (lowercase "w")

indexOf([1, 2, 3], 2) // returns 1
indexOf([1, 2, 3], 4) // returns -1

indexOf({"a": 1, "b": 2}, 2) // returns "b"
indexOf({"a": 1, "b": 2}, 3) // returns null
```
