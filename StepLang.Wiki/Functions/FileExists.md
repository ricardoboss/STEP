# Description

The `fileExists` function checks if a file at the given path exists and returns a boolean indicating the result.

# Syntax

```step
fileExists(string path)
```

- `path` is the path to a file or link/junction to a file.

# Remarks

- The function only checks if the file exists, not if it can be read using `fileRead` (so it may exist but is currently not readable)
- The path can be relative and is interpreted relative to the current working directory
- Absolute paths may require a drive letter (system dependent)

# Examples

```
/
	data/
		a.txt
		b.txt
	lib/
		main.step
		c.txt
```

`lib/main.step` run in `/lib/`:

```step
if (fileExists("/data/a.txt")) {
	println("/data/a.txt found") // will get printed
}

if (fileExists("b.txt")) {
	println("b.txt in current dir found") // will NOT get printed
}

if (fileExists("c.txt")) {
	println("c.txt in current dir found") // will get printed
}
```
