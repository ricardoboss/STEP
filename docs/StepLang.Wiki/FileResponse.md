# Description

Reads the file contents and returns a map that can be returned from a [`httpServer`](./HttpServer) request handler.

# Syntax

```step
fileResponse(string path)
```

- `path` is the path to the file to read.

# Remarks

- If the file does not exist, a 404 response with an empty body is returned.

# Examples

```step
httpServer(8080, (map request) {
    return fileResponse("./index.html")
})
```