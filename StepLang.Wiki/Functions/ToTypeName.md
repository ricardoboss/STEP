# Description

The `toTypeName` function returns the type name of the given value.

# Syntax

```step
toTypeName(any value)
```

- `value` is the value to get the type name from (`number`, `string`, `bool`, `list`, `map`, `null` or `function`).

# Examples

```step
toTypeName(1) // returns "number"
toTypeName("a") // returns "string"
toTypeName(true) // returns "bool"
toTypeName([]) // returns "list"
toTypeName({}) // returns "map"
toTypeName(null) // returns "null"
toTypeName(toTypeName) // returns "function"
```
