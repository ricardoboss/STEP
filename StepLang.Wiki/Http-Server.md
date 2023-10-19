# HTTP Server

STEP has the ability to serve HTTP requests.
This can be used for creating websites and toying around with web technologies.

## "Hello World" Server

This script will start a server on [localhost:8080](http://localhost:8080) and respond to all requests with
"Hello World":

```step
httpServer(8080, (Map request) {
    return stringResponse("Hello World")
})
```

You can also pass an existing function:

```step
function handleRequest = (Map request) {
    return stringResponse("Hello World")
}

httpServer(8080, handleRequest)
```

## Serving Files

This script will start a server on [localhost:8080](http://localhost:8080) and serve files from the current
directory. It will also serve `index.html` when the user requests `/`:

```step
httpServer(8080, (Map request) {
    if (request["path"] == "/") {
        request["path"] = "/index.html"
    }

    if (!startsWith(request["path"], "/")) {
        request["path"] = "/" + request["path"]
    }

    if (fileExists("." + request["path"])) {
        return fileResponse(request["path"]);
    }

    return stringResponse("404 Not Found", 404)
})
```
