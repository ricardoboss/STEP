# Description

`interpolate` calculate a linear interpolation between two values based on a delta.

# Syntax

```step
interpolate(number a, number b, number t)
```

- `a` is the start value.
- `b` is the end value.
- `t` is the delta value.

# Examples

```step
interpolate(0, 10, 0.5) // 5
interpolate(0, 10, 0.25) // 2.5
interpolate(0, 10, 0.75) // 7.5
```
