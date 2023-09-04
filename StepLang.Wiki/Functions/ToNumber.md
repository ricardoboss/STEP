# Description

`toNumber` converts a string to a number.

# Syntax

```step
toNumber(string value, number radix = 10)
```

- `value`: The string to convert to a number.
- `radix`: The radix to use when converting the string to a number. Defaults to `10`. Supported are `2`, `8`, `10` and `16`.

# Remarks

- If `value` cannot be converted to a number, `null` is returned.
- If `radix` is not supported, `null` is returned.
- If `radix` is not specified, `10` is used.

# Examples

```step
toNumber("123") // 123
toNumber("123", 10) // 123
toNumber("FF", 16) // 255
toNumber("FF", 10) // null
toNumber("9", 8) // null
toNumber("1010", 2) // 10
```
