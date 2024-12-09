# Description

`sorted` takes a list and returns a new list with the same elements but sorted in ascending order.

# Syntax

```step
sorted(list list, function compare = compareTo)
```

- `list`: The list to sort.
- `compare`: The function to use for comparing elements. Defaults to [`compareTo`](./CompareTo.md).

# Remarks

- the `compareTo` function will throw if the list contains elements of different types (e.g. `[1, "a"]`)
- if specifying a custom compare function, it needs to take two arguments of the same type and return a number
  indicating whether the first argument is less than, equal to, or greater than compared to the second argument:
  ```step
  function cmp = (number a, number b) {
  	return a - b
  }
  
  sorted([3, 1, 2], cmp) // returns [1, 2, 3]
  ```

# Examples

```step
sorted([3, 1, 2]) // returns [1, 2, 3]
```

```step
sorted(["c", "a", "b"]) // returns ["a", "b", "c"]
```

```step
function cmp = (number a, number b) {
	return a - b
}

sorted([3, 1, 2], cmp) // returns [1, 2, 3]
```
