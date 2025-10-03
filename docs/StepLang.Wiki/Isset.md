# Description

The `isset` function takes a string as its first parameter and returns a boolean, indicating whether or not a
variable with the given name exists in the current scope.

# Syntax

```step
isset(string variableName)
```

- `variableName` is the name of the variable to check for.

# Remarks

- Any parent scope is included in the search for variable names

# Examples

```step
number a = 123

println(isset("a")) // True
println(isset("b")) // False
```
