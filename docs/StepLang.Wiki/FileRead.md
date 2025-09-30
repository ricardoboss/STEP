# Description

The `fileRead` function takes a files path as a string and returns the contents of the file as a string or `null` if the
file does not exist.

# Syntax

```step
fileRead(string path)
```

- `path` is the path to a file or a link/junction to a file.

# Remarks

- If the file specified in `path` does not exist, `null` is returned.
- If the file contains binary data the data is decoded as ASCII and returned as a string (so `\u09` is interpreted as an
  HTAB character)
- If the file cannot be opened (e.g. because it is in use by another program), `null` is returned

# Examples

`hello.txt`

```
Hello World!

This is a sample text file.
```

`main.step`

```step
string contents = fileRead("hello.txt")

println(contents) // "Hello World!\n\n..."
```
