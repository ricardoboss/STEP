# Description

`env` is used to get or set the environment variables.

# Syntax

```step
env(string key)
env(string key, any value)
```

- `key` is the name of the environment variable to get or set.
- `value` is the value to set the environment variable to.

# Examples

```step
var path = env("PATH")

env("PATH", path + ";~/.bin")
```
