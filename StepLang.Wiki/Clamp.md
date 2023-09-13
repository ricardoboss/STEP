# Description

`clamp` is a function that returns a value that is clamped between two values.

# Syntax

```step
clamp(number min, number max, number x)
```

- `min` is the minimum value.
- `max` is the maximum value.
- `x` is the value to clamp.

# Remarks

- If `x` is less than `min`, `min` is returned.
- If `x` is greater than `max`, `max` is returned.
- Otherwise, `x` is returned.

# Examples

```step
clamp(0, 10, 5) // 5

clamp(0, 10, -5) // 0

clamp(0, 10, 15) // 10
```
