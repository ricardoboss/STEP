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
- For `text`, the length is the number of so called grapheme clusters, not the
number of bytes. So, a single character can be multiple bytes. For example,
'🤷‍♂️' is treated as one character, not 5.

# Examples

```step
length("Hello!")                   // 6
length([1, 2, 3, 4])               // 4
length({ "a": 1, "b": 2, "c": 3 }) // 3

length("🤷‍♂️")                      // 1
```
