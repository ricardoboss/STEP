# Description

The `seed` function seeds the random number generator.

# Syntax

```step
seed(number seed)
```

- `seed` is the seed value to use. It must be an integer.

# Remarks

- the seed value is used to initialize the random number generator (see [`random`](./Random.md))

# Examples

```step
seed(123456789)
println(random()) // 0.5083497583439339
```
