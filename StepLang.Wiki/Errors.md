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

| Code                 | Description                       |
|----------------------|-----------------------------------|
| [`INT001`](./INT001) | Undefined Identifier Exception    |
| [`INT002`](./INT002) | Invalid Argument Count Exception  |
| [`INT003`](./INT003) | Invalid Expression Type Exception |
| [`INT004`](./INT004) | Index Out Of Bounds Exception     |

### Import Errors (`IMPxxx`)

| Code                 | Description                            |
|----------------------|----------------------------------------|
| [`IMP001`](./IMP001) | Imported File Does Not Exist Exception |
| [`IMP002`](./IMP002) | Imported File Is Self Exception        |

### Type Errors (`TYPxxx`)

| Code                 | Description                                |
|----------------------|--------------------------------------------|
| [`TYP001`](./TYP001) | Invalid Variable Assignment Exception      |
| [`TYP002`](./TYP002) | Incompatible Expression Operands Exception |
| [`TYP003`](./TYP003) | Invalid Argument Type Exception            |
| [`TYP004`](./TYP004) | Invalid Result Type Exception              |
| [`TYP005`](./TYP005) | Variable Already Declared Exception        |

### Parser Errors (`PARxxx`)

| Code                 | Description                        |
|----------------------|------------------------------------|
| [`PAR001`](./PAR001) | Unexpected Token Exception         |
| [`PAR002`](./PAR002) | Unexpected End Of Tokens Exception |
| [`PAR003`](./PAR003) | Missing Expression Exception       |
| [`PAR004`](./PAR004) | Invalid Index Operator Exception   |

### Tokenizer Errors (`TOKxxx`)

| Code                 | Description                   |
|----------------------|-------------------------------|
| [`TOK001`](./TOK001) | Invalid Identifier Exception  |
| [`TOK002`](./TOK002) | Unterminated String Exception |
