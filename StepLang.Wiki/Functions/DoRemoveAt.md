# Description

`doRemoveAt` removes the element at the given index from the list given to it and returns it.

# Syntax

```step
doRemoveAt(list subject, number index)
```

- `subject` is the list to remove the element from.
- `index` is the index of the element to remove.

# Remarks

- `doRemoveAt` is a mutating function, so it modifies the list given to it
- `index` must be a valid index in the list, ranging from 0 to the length of the list minus 1.

# Examples

```step
list myList = [1, 2, 3, 4]
doRemoveAt(myList, 1)
print(myList) // prints [1, 3, 4]
```
