# Description

`doShift` removes the first element from a list and returns the removed element.

# Syntax

```step
doShift(list myList)
```

- `myList` is the list to be shifted

# Remarks

- `doShift` is a mutating function, so it modifies the list given to it
- if the list is empty, `null` is returned

# Examples

```step
doShift([1,2,3]) // returns 1
doShift([]) // returns null
```
