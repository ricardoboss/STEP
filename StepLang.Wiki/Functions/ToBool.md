# Description

The `toBool` function converts a value to a boolean value.

# Syntax

```
toBool(any value)
```

- `value` is the value to convert to a boolean value.

# Remarks

- The function returns `true` if the value is truthy (true-like), `false` otherwise.  
  Whether or not a value is truthy can be determined using the following table:

  | Value Type | Truthy                               | Falsy     |
  |------------|--------------------------------------|-----------|
  | `bool`     | `true`                               | `false`   |
  | `number`   | `> 0`                                | `<= 0`    |
  | `string`   | `"true"` (case insensitive) or `"1"` | otherwise |
  | `list`     | if not empty                         | otherwise |
  | `map`      | if not empty                         | otherwise |
  | `null`     | never                                | always    |
  | `void`     | never                                | always    |
  | `function` | always                               | never     |

# Examples

```step
println(toBool(""))              // False
println(toBool("this is falsy")) // False
println(toBool("true"))          // True
println(toBool("false"))         // False
println(toBool("True"))          // True
println(toBool("1"))             // True
println(toBool("0"))             // False
println(toBool(0))               // False
println(toBool(1))               // True
println(toBool(2))               // True
println(toBool(-1))              // False
```