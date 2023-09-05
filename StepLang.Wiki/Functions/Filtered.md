# Description

`filtered` is a function that returns a list of items that match a given condition.

# Syntax

```step
filtered(list subject, function callback)
```

- `subject` is the list to be filtered.
- `callback` is a function that returns a boolean value. The function is called for each item in the list. If the function returns `true`, the item is added to the result list. If the function returns `false`, the item is not added to the result list.

# Remarks

- The `callback` function must have one or two parameters and must return a boolean value.
- If the `callback` function has one parameter, the parameter is the current item in the list.
- If the `callback` function has two parameters, the first parameter is the current item in the list and the second parameter is the index of the current item in the list.

# Examples

```step
filtered([1, 2, 3, 4, 5], (number item) {
    return item % 2 == 0
}) // returns [2, 4]
```

```step
filtered([1, 2, 3, 4, 5], (number item, number index) {
    return index % 2 == 0
}) // returns [1, 3, 5]
```
