# Description

The `length` function takes a string, list, or map, and returns the count of
characters or items.

# Syntax

```step
length(string text)

length(list myList)

length(map myMap)
```

- `text` is the string whose length to calculate.
- `myList` or `myMap` are collections whose items to count.

# Remarks

- For `text`, each whitespace counts as one character. So, two spaces are two
characters, a line break is one character, and so on.
- For `text`, the function _technically_ returns the amount of two-byte pairs.
For most text, the result is exactly what you'd expect, but if the text
contains advanced Unicode characters (such as some emoji), the result may be
confusing. For example, the length of `🤷🏻‍♂️` is given as `7`, which is due to
the way some characters work internally.

# Examples

```step
length("Hello!")                   # 6
length([1, 2, 3, 4])               # 4
length({ "a": 1, "b": 2, "c": 3 }) # 3
```
