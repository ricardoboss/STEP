# Description

The `range` function returns a list of numbers starting at `start` and ending at
`end`, advancing by `step` between values.

# Syntax

```step
range(number start, number end, number step = 1)
```

- `start` is the first number in the sequence.
- `end` is the last number in the sequence.
- `step` controls how much to add (or subtract) between values. It defaults to `1`.

# Remarks

- The resulting list always includes `start`.
- The `end` value is only included when repeatedly applying `step` reaches it (allowing for a very small rounding tolerance). When the increment would overshoot `end`, the sequence stops before it.
- `step` can be positive or negative, but it cannot be `0`.
- When `step` is positive, `start` must be less than or equal to `end`.
- When `step` is negative, `start` must be greater than or equal to `end`.
- Fractional step sizes are supported.

# Examples

```step
range(0, 3)           // [0, 1, 2, 3]
range(3, 0, -1)       // [3, 2, 1, 0]
range(0, 1, 0.25)     // [0, 0.25, 0.5, 0.75, 1]
range(0, 1, 0.3)      // [0, 0.3, 0.6, 0.9]
range(5, 5)           // [5]
```
