# Description

`doInsertAt` is a function that inserts a value into a list at a specified index.

# Syntax

```step
doInsertAt(list myList, number index, any value)
```

- `myList` is the list to insert the value into.
- `index` is the index to insert the value at. It must be an integer between 0 and the length of the list.
- `value` is the value to insert into the list.

# Remarks

- If the index is less than 0 or greater than the length of the list, an error will be thrown.
- If the index is equal to the length of the list, the value will be appended to the end of the list.
- The 0th index is the first element in the list.

# Examples

```step
list myList = [1, 2, 3]
doInsertAt(myList, 1, 1.5)
println(myList) // prints [1, 1.5, 2, 3]
```
