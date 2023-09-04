# Description

`toRadix` converts a number to a string representing the number in the specified radix (base).

# Syntax

```step
toRadix(number value, number radix)
```

- `value` is the number to convert.
- `radix` is the radix (base) to use for the conversion. Must be an integer (supported: `2`, `8`, `10`, `16`).

# Remarks

- Returns `null` for an invalid/unsupported radix.

# Examples

```step
toRadix(255, 16) // "FF"
toRadix(255, 10) // "255"
toRadix(255, 8) // "377"
toRadix(255, 2) // "11111111"

toRadix(255, 1) // null
```
