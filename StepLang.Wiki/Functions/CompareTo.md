# Description

The `compareTo` function takes two arguments of the same type and returns a number indicating whether the
first argument is less than, equal to, or greater than compared to the second argument.

# Syntax

```step
compareTo(number a, number b)
compareTo(string a, string b)
compareTo(bool a, bool b)
compareTo(list a, list b)
compareTo(map a, map b)
```

- `a`: The first argument.
- `b`: The second argument.

# Remarks

- also accepts values of types `function`, `null` and `void` but will always return `0` in those cases
- if `a` and `b` are not the same type, the function will throw an error
- if `a` and `b` are of type `list` or `map`, the function will compare the length of the lists or maps
- if `a` and `b` are of type `string`, the function will compare the strings ordinal-wise

# Examples

```step
compareTo(1, 2) // returns -1
compareTo(2, 1) // returns 1
compareTo(1, 1) // returns 0
```

```step
compareTo("a", "b") // returns -1
compareTo("b", "a") // returns 1
compareTo("a", "a") // returns 0
```

```step
compareTo(true, false) // returns 1
compareTo(false, true) // returns -1
compareTo(true, true) // returns 0
```

```step
compareTo([1, 2, 3], [1, 2, 3, 4]) // returns -1
compareTo([1, 2, 3, 4], [1, 2, 3]) // returns 1
compareTo([1, 2, 3], [1, 2, 3]) // returns 0
```

```step
compareTo({"a": 1, "b": 2}, {"a": 1, "b": 2, "c": 3}) // returns -1
compareTo({"a": 1, "b": 2, "c": 3}, {"a": 1, "b": 2}) // returns 1
compareTo({"a": 1, "b": 2}, {"a": 1, "b": 2}) // returns 0
```
