# Description

`doPop` removes the last element from a list and returns it.

# Syntax

```step
doPop(list myList)
```

- `myList` is the list to pop from

# Remarks

- `doPop` is a mutating function, so it modifies the list given to it
- if the list is empty, `doPop` returns `null`
- `doPop` is the opposite of [`doAdd`](./DoAdd.md)

# Examples

```step
list l = [1, 2, 3]

doPop(l) // returns 3

println(l) // prints [1, 2]

doPop(l) // returns 2

println(l) // prints [1]

doPop(l) // returns 1

println(l) // prints []

doPop(l) // returns null
```
