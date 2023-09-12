# Description

The `substring` function takes a string and returns a certain part of the string, based on a start and a length.

# Syntax

```step
substring(string subject, number start, number length)
```

- `subject` is the string to take a substring from.
- `start` is the index of the first character to include in the substring (0-based).
- `length` is the number of characters to include in the substring. Must be greater than or equal to 0.

# Remarks

- If `start` is greater than the number of characters in the string - 1, the function will return an empty string.
- If `length` is 0 or negative, the function will return an empty string.
- If `start + length` is greater than the number of characters remaining in the string, the substring will be truncated to the remaining characters.
- If `start` or `length` is not an integer, it will be rounded to the nearest integer (0.5 rounds up).
- If `start` is negative, it is treated as starting from the end of the subject. For example, if `start` is -3 and `length` is 3, the function will return the last 3 characters of the string. If the absolute value is greater than the number of characters in `subject`, an empty string is returned.

# Examples

```step
substring("Hello, world!", 0, 5)   // "Hello"
substring("Hello, world!", 7, 5)   // "world"
substring("Hello, world!", 7, 100) // "world!"
substring("Hello, world!", 7, 0)   // ""
substring("Hello, world!", 7, -1)  // ""
substring("Hello, world!", 7, 1.5) // "wo"
substring("Hello, world!", 7.5, 1) // "o"
substring("Hello, world!", 14, 5)  // ""
substring("Hello, world!", -14, 5) // ""
substring("Hello, world!", -1, 3)  // "!"
substring("Hello, world!", -2, 3)  // "d!"
substring("Hello, world!", -3, 3)  // "ld!"
substring("Hello, world!", -4, 3)  // "rld"
substring("Hello, world!", -13, 5) // "Hello"
```
