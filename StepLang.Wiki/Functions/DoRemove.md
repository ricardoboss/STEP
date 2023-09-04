# Description

`doRemove` removes the first occurrence of a value from a list.

# Syntax

```step
doRemove(list subject, any element)
```

- `subject` is the list to remove the element from.
- `element` is the element to remove from the list.

# Remarks

- `doRemove` is a mutating function, so it doesn't return a value but modifies the list given to it

# Examples

```step
list myList = [1, 2, 3, 4]
doRemove(myList, 3)
print(myList) // prints [1, 2, 4]
```
