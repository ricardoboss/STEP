# STEP Errors

When working with STEP, you may encounter errors.
This page lists all errors.

## Format

All errors listed here have their own page.
The page describes the error using an example and provides possible solutions in the form of a diff.

A diff or patch looks like this:

```diff
- This line was removed
+ This line was added
```

## Error codes

### Interpreter Errors (`INTxxx`)

| Code                        | Description                       |
|-----------------------------|-----------------------------------|
| [`INT001`](./Errors/INT001) | Undefined Identifier Exception    |
| [`INT002`](./Errors/INT002) | Invalid Argument Count Exception  |
| [`INT003`](./Errors/INT003) | Invalid Depth Result Exception    |
| [`INT004`](./Errors/INT004) | Invalid Expression Type Exception |
| [`INT005`](./Errors/INT005) | Index Out Of Bounds Exception     |

### Parser Errors (`PARxxx`)

| Code                        | Description                            |
|-----------------------------|----------------------------------------|
| [`PAR001`](./Errors/PAR001) | Unexpected Token Exception             |
| [`PAR002`](./Errors/PAR002) | Unexpected End Of Tokens Exception     |
| [`PAR003`](./Errors/PAR003) | Missing Value Expression Exception     |
| [`PAR004`](./Errors/PAR004) | Missing Condition Expression Exception |
| [`PAR005`](./Errors/PAR005) | Imported File Does Not Exist Exception |
| [`PAR006`](./Errors/PAR006) | Imported File Is Self Exception        |
| [`PAR007`](./Errors/PAR007) | Imports No Longer Allowed Exception    |
| [`PAR008`](./Errors/PAR008) | Invalid Expression Exception           |
| [`PAR009`](./Errors/PAR009) | Invalid Index Operator Exception       |

### Tokenizer Errors (`TOKxxx`)

| Code                        | Description                   |
|-----------------------------|-------------------------------|
| [`TOK001`](./Errors/TOK001) | Invalid Identifier Exception  |
| [`TOK002`](./Errors/TOK002) | Unterminated String Exception |

### Type Errors (`TYPxxx`)

| Code                        | Description                                |
|-----------------------------|--------------------------------------------|
| [`TYP001`](./Errors/TYP001) | Invalid Variable Assignment Exception      |
| [`TYP002`](./Errors/TYP002) | Incompatible Expression Operands Exception |
| [`TYP003`](./Errors/TYP003) | Invalid Argument Type Exception            |
| [`TYP004`](./Errors/TYP004) | Invalid Result Type Exception              |
