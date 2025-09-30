# Description

`fetch` makes a request to a URL and returns the response as a string or `null` if the request failed.

# Syntax

```step
fetch(string url, map options)
```

- `url` is the URL to fetch.
- `options` is a map of options to pass to the request.
    - possible keys include:
    - `method` is the HTTP method to use (default: `GET`)
    - `headers` is a map of headers to send with the request
    - `body` is the body to send with the request
        - if the value is not a string, it is converted to JSON
    - `timeout` is the timeout in milliseconds
    - all keys are optional, other keys will be ignored

# Remarks

- `fetch` is a blocking operation, meaning that the program will wait for the request to complete before continuing.

# Examples

```step
string response = fetch("https://example.com")
println(response)
```

```step
map options = {
  "method": "POST",
  "headers": {
    "Content-Type": "application/json"
  },
  "body": {
    "message": "Hello, World!"
  }
}
string response = fetch("https://example.com", options)
println(response)
```

```step
string response = fetch("https://example.com")

// for JSON response bodies
println(fromJSON(response))
```
