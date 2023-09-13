# Description

`clone` can be used to create a copy of a value.

# Syntax

```step
clone(any subject)
```

- `subject` is the value to be cloned.

# Remarks

- `clone` is a deep copy, meaning that all nested values are also cloned (in `list`s and `map`s)

# Examples

```step
list l = [3, 2]
println(l) // [3, 2]
doAdd(l, 1)
println(l) // [3, 2, 1]
list l2 = clone(l)
println(l2) // [3, 2, 1]
doAdd(l, 0)
println(l) // [3, 2, 1, 0]
println(l2) // [3, 2, 1]
```
