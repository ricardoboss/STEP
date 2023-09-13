# Description

`converted` returns a new list with the elements converted using the given callback.

# Syntax

```step
converted(list subject, function callback)
```

- `subject` is the list to be converted.
- `callback` is the function to be called for each element in the list.  
  The function must either have one parameter or two parameters whereas the second parameter must accept a number.

# Remarks

- If the callback has one parameter, the parameter will be the current element in the list.
- If the callback has two parameters, the first parameter will be the current element in the list and the second parameter will be the index of the current element in the list.

# Examples

```step
converted([1, 2, 3], (number n) {
	return n * 2
}) // returns [2, 4, 6]
```

```step
converted([1, 2, 3], (number n, number i) {
	return n * i
}) // returns [0, 2, 6]
```

```step
converted([1, 2, 3], (number n, number i) {
	return "The " + (i + 1) + ". number is " + n
}) // returns ["The 1. number is 1", "The 2. number is 2", "The 3. number is 3"]
```
