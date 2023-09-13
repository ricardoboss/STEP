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
| [`INT001`](./INT001) | Undefined Identifier Exception    |
| [`INT002`](./INT002) | Invalid Argument Count Exception  |
| [`INT003`](./INT003) | Invalid Depth Result Exception    |
| [`INT004`](./INT004) | Invalid Expression Type Exception |
| [`INT005`](./INT005) | Index Out Of Bounds Exception     |

### Parser Errors (`PARxxx`)

| Code                        | Description                            |
|-----------------------------|----------------------------------------|
| [`PAR001`](./PAR001) | Unexpected Token Exception             |
| [`PAR002`](./PAR002) | Unexpected End Of Tokens Exception     |
| [`PAR003`](./PAR003) | Missing Value Expression Exception     |
| [`PAR004`](./PAR004) | Missing Condition Expression Exception |
| [`PAR005`](./PAR005) | Imported File Does Not Exist Exception |
| [`PAR006`](./PAR006) | Imported File Is Self Exception        |
| [`PAR007`](./PAR007) | Imports No Longer Allowed Exception    |
| [`PAR008`](./PAR008) | Invalid Expression Exception           |
| [`PAR009`](./PAR009) | Invalid Index Operator Exception       |

### Tokenizer Errors (`TOKxxx`)

| Code                        | Description                   |
|-----------------------------|-------------------------------|
| [`TOK001`](./TOK001) | Invalid Identifier Exception  |
| [`TOK002`](./TOK002) | Unterminated String Exception |

### Type Errors (`TYPxxx`)

| Code                        | Description                                |
|-----------------------------|--------------------------------------------|
| [`TYP001`](./TYP001) | Invalid Variable Assignment Exception      |
| [`TYP002`](./TYP002) | Incompatible Expression Operands Exception |
| [`TYP003`](./TYP003) | Invalid Argument Type Exception            |
| [`TYP004`](./TYP004) | Invalid Result Type Exception              |
