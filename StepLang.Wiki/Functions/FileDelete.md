# Description

`fileDelete` deletes a file from the file system.

# Syntax

```step
fileDelete(string path)
```

- `path` is the path to the file to delete.

# Remarks

- If the file does not exist, the function does nothing.
- If the file is a directory, the function does nothing.
- If the file is open, the function does nothing.
- If the file is read-only, the function does nothing.
- Returns `true` if the file was deleted, `false` otherwise.

# Examples

```step
fileDelete("C:/Temp/MyFile.txt")
```
