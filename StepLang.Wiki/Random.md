# Description

The `random` function returns a random number between 0 and 1.

# Syntax

```step
random()
```

# Remarks

- returns a random number between 0 and 1
- the generated number is:
    - evenly distributed
    - not cryptographically secure
    - not guaranteed to be unique for every call
- you can use the [`seed`](./Seed.md) function to seed the random number generator

# Examples

```step
random() // 0.123456789
```
