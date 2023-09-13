# Description

`doSwap` swaps the values of two list indices or two map keys.

# Syntax

```step
doSwap(list myList, number a, number b)
doSwap(map myMap, string a, string b)
```

- `myList` is the list to swap values in.
- `myMap` is the map to swap values in.
- `a` is the first index or key to swap.
- `b` is the second index or key to swap.

# Remarks

- `a` and `b` must be valid indices or keys in the list or map.
  - If `a` or `b` is not a valid index or key, the function will throw an error.
  - For a list, `a` and `b` must be between `0` and the length of the list minus `1`.
- `doSwap` will swap the values of the two indices or keys.
  - For a list, the values at indices `a` and `b` will be swapped.
  - For a map, the values at keys `a` and `b` will be swapped.
- `doSwap` will return a `bool` indicating whether the swap was successful.
  - If `a` or `b` is not a valid index or key, the function will return `false`.
  - Otherwise, the function will return `true`.

# Examples

```step
// Swap two values in a list
list myList = [1, 2, 3, 4, 5]
doSwap(myList, 0, 4) // Returns true
println(myList) // Prints [5, 2, 3, 4, 1]

// Swap two values in a map
map myMap = {"a": 1, "b": 2, "c": 3, "d": 4, "e": 5}
doSwap(myMap, "a", "e") // Returns true
println(myMap) // Prints {"a": 5, "b": 2, "c": 3, "d": 4, "e": 1}
```
