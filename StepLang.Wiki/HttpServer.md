# Description

Starts a HTTP server on the specified port/with the specified options and calls the callback for each incoming request.

# Syntax

```step
httpServer(number port, function requestHandler)
httpServer(map options, function requestHandler)
```

- `port` must be an integer (number) between 1 and 65535
- `options` can contain the following key-value-pairs:
  - `port`: integer (number) between 1 and 65535
  - `headers`: a map of default headers to include in the response
- `requestHandler` must be a function that takes a map as its only argument

# Remarks

- the `requestHandler` function is called for each incoming request
- the return value of the `requestHandler` function is used as the response body, except if it is a map
  - if the return value is a map, the following keys are used to construct the response:
    - `status`: integer (number) between 100 and 599
    - `headers`: a map of headers to include in the response
    - `body`: the response body
  - if the return value is not a map, the following response is constructed:
    - `status`: 200
    - `headers`: a map containing the default headers and a `Content-Type` header with the value `text/plain`
    - `body`: the return value converted to a string using [`toString`](./ToString)

# Examples

The simplest server just returns "Hello World!" for each request:

```step
httpServer(8080, (map request) {
    return "Hello World!"
})
```

A more complex server that returns a JSON response:

```step
httpServer(8080, (map request) {
    return {
        "status": 200,
        "headers": {
            "Content-Type": "application/json"
        },
        "body": {
            "message": "Hello World!"
        }
    }
})
```

Using the `options` map:

```step
httpServer({
    "port": 8080,
    "headers": {
        "Server": "STEP"
    }
}, (map request) {
    return {
        "headers": {
            "Content-Type": "application/json"
        },
        "body": {
            "message": "Hello World!"
        }
    }
})
```
