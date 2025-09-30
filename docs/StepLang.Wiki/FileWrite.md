# Description

`fileWrite` is a function that writes a string to a file and returns a boolean indicating whether it was successful.

# Syntax

```step
fileWrite(string path, string content, bool append = false)
```

- `path` is the path to the file to write to
- `content` is the content to write to the file
- `append` is a boolean that indicates whether to append the content to the file or overwrite the file

# Remarks

- If the file does not exist, it will be created
- If the file exists and `append` is `true`, the content will be appended to the file
- If the file exists and `append` is `false`, the file will be overwritten with the content
- If the content was successfully written to the file, `true` will be returned, `false` otherwise

# Examples

```step
fileWrite("C:/test.txt", "Hello, world!") // creates or overwrites the file
fileWrite("C:/test.txt", "Another hello", true) // appends to the file
fileWrite("C:/test.txt", "Hello, world again!", false) // overwrites the file
```
