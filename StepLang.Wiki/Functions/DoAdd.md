# Description

`doAdd` adds an element to the end of a list.

# Syntax

```step
doAdd(list subject, any element)
```

- `subject` is the list to add the element to.
- `element` is the element to add to the list.

# Remarks

- `doAdd` is a mutating function, so it doesn't return a value but modifies the list given to it

# Examples

```step
list myList = [1, 2, 3]
doAdd(myList, 4)
print(myList) # prints [1, 2, 3, 4]
```
