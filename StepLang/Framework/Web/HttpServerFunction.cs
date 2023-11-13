using System.Net;
using System.Text;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;

namespace StepLang.Framework.Web;

public class HttpServerFunction : NativeFunction
{
    public const string Identifier = "httpServer";

    /// <inheritdoc />
    public override IEnumerable<(ResultType[] types, string identifier)> Parameters => new[] { (new[] { ResultType.Map, ResultType.Number }, "portOrOptions"), (new[] { ResultType.Function }, "handler") };

    /// <inheritdoc />
    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter, IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments);

        var portOrOptions = await arguments[0].EvaluateAsync(interpreter, cancellationToken);

        int port;
        IDictionary<string, ExpressionResult> options;
        if (portOrOptions.ResultType is ResultType.Map)
        {
            options = portOrOptions.ExpectMap().Value;

            if (options.TryGetValue("port", out var portResult))
            {
                port = portResult.ExpectInteger().RoundedIntValue;

                if (port is < 0 or > 65535)
                    throw new ArgumentException("Port must be between 0 and 65535");
            }
            else
                port = 8080;
        }
        else if (portOrOptions.ResultType is ResultType.Number)
        {
            port = portOrOptions.ExpectInteger().RoundedIntValue;
            options = new Dictionary<string, ExpressionResult>();
        }
        else
            throw new NotImplementedException("Expected port or options");

        var handler = await arguments[1].EvaluateAsync(interpreter, r => r.ExpectFunction().Value, cancellationToken);

        await Serve(interpreter, port, options, handler, cancellationToken);

        return VoidResult.Instance;
    }

    private static async Task Serve(Interpreter interpreter, int port, IDictionary<string, ExpressionResult> options, FunctionDefinition callback, CancellationToken cancellationToken)
    {
        var url = $"http://localhost:{port}/";

        using var server = new HttpListener();
        server.Prefixes.Add(url);
        server.Start();

        interpreter.StdOut?.WriteLineAsync($"Listening on {url}");

        var defaultResponseHeaders = new Dictionary<string, string>();
        if (options.TryGetValue("headers", out var headersResult))
        {
            var headers = headersResult.ExpectMap().Value;
            foreach (var (key, value) in headers)
                defaultResponseHeaders.Add(key, ToStringFunction.Render(value));
        }

        try
        {
            var handlers = Enumerable.Range(0, Environment.ProcessorCount - 1).Select(i => new RequestHandler(i, interpreter, defaultResponseHeaders, callback));
            await Parallel.ForEachAsync(
                handlers,
                cancellationToken,
                async (handler, c) => await handler.Loop(server, c));
        }
        finally
        {
            server.Stop();
        }
    }

    private static async Task<MapResult> ToRequestMapAsync(HttpListenerRequest request, CancellationToken cancellationToken)
    {
        var requestMethod = new StringResult(request.HttpMethod);
        ExpressionResult requestUrl = NullResult.Instance;
        ExpressionResult requestPath = NullResult.Instance;
        if (request.Url is not null)
        {
            requestUrl = new StringResult(request.Url.ToString());
            requestPath = new StringResult(request.Url.AbsolutePath);
        }

        var requestHeaderDict = request
            .Headers
            .AllKeys
            .Where(k => k != null)
            .Cast<string>()
            .ToDictionary(k => k, k => (ExpressionResult)new StringResult(request.Headers[k]!));

        var requestHeaderResult = new MapResult(requestHeaderDict);

        string requestBody;
        using (var reader = new StreamReader(request.InputStream))
            requestBody = await reader.ReadToEndAsync(cancellationToken);

        var requestBodyResult = new StringResult(requestBody);

        return new(new Dictionary<string, ExpressionResult>
        {
            ["method"] = requestMethod,
            ["url"] = requestUrl,
            ["path"] = requestPath,
            ["headers"] = requestHeaderResult,
            ["body"] = requestBodyResult,
        });
    }

    private static string RenderExceptionPage(Exception e)
    {
        string? location = null;
        if (e is StepLangException { Location: { } l })
        {
            location = $"<small>thrown at: <code>{l}</code></small>";
        }

        return $$"""
                 <!DOCTYPE html>
                 <html lang="en">
                 <head>
                 	<title>Unhandled Exception: {{e.Message}}</title>
                 	<style rel="stylesheet">
                 		html, body {
                 			margin: 0;
                 			text-align: center;
                 			font-family: sans-serif;
                 		}

                 		body {
                 			padding: 1rem;
                 			border: 5px dashed red;
                 			border-radius: 1rem;
                 		}

                 		@media (prefers-color-scheme: dark) {
                 			body {
                 				background: #202124;
                 				color: #fff;
                 			}
                 		}

                 		hr {
                 			border: 0;
                 			border-bottom: 1px solid #ccc;
                 		}

                 		pre {
                 			overflow: auto;
                 			text-align: left;
                 			padding: 1rem;
                 		}
                 	</style>
                 </head>
                 <body>
                 	<h1>Unhandled Exception</h1>
                 	<p>
                 		<code>{{e.GetType().Name}}</code><br>
                 		<strong>{{e.Message}}</strong><br>
                 		{{location}}
                 	</p>
                 	<hr>
                 	<pre>{{e.StackTrace}}</pre>
                 </body>
                 </html>
                 """;
    }

    private class RequestHandler
    {
        private readonly int workerId;
        private readonly Interpreter interpreter;
        private readonly Dictionary<string, string> defaultResponseHeaders;
        private readonly FunctionDefinition callback;

        public RequestHandler(int workerId, Interpreter interpreter, Dictionary<string, string> defaultResponseHeaders, FunctionDefinition callback)
        {
            this.workerId = workerId;
            this.interpreter = interpreter;
            this.defaultResponseHeaders = defaultResponseHeaders;
            this.callback = callback;
        }

        public async ValueTask Loop(HttpListener server, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var context = await server.GetContextAsync();
                var request = context.Request;
                var response = context.Response;

                var requestMap = await ToRequestMapAsync(request, cancellationToken);

                interpreter.StdOut?.WriteLineAsync($"({workerId}) {request.RemoteEndPoint} -> {request.HttpMethod} {request.Url?.AbsolutePath}");

                ExpressionResult responseResult;
                try
                {
                    responseResult = await callback.EvaluateAsync(interpreter, new Expression[]
                    {
                        requestMap.ToLiteralExpression(),
                    }, cancellationToken);
                }
                catch (StepLangException e)
                {
                    responseResult = new MapResult(new Dictionary<string, ExpressionResult>
                    {
                        {
                            "status", new NumberResult(500)
                        },
                        {
                            "body", new StringResult(RenderExceptionPage(e))
                        },
                    });
                }

                IDictionary<string, ExpressionResult> responseMap;
                if (responseResult is MapResult map)
                {
                    responseMap = map.Value;
                }
                else
                {
                    responseMap = new Dictionary<string, ExpressionResult>
                    {
                        ["body"] = responseResult,
                    };
                }

                response.StatusCode = responseMap.TryGetValue("status", out var statusResult) ? statusResult.ExpectInteger().RoundedIntValue : 200;

                interpreter.StdOut?.WriteLineAsync($"({workerId}) {request.RemoteEndPoint} <- {response.StatusCode}");

                foreach (var (key, value) in defaultResponseHeaders)
                    response.Headers.Add(key, value);

                if (responseMap.TryGetValue("headers", out var responseHeaders))
                {
                    var headers = responseHeaders.ExpectMap().Value;
                    foreach (var (key, value) in headers)
                        response.Headers.Add(key, ToStringFunction.Render(value));
                }

                if (responseMap.TryGetValue("body", out var bodyResult))
                {
                    var body = ToStringFunction.Render(bodyResult);
                    var buffer = Encoding.UTF8.GetBytes(body);

                    response.ContentLength64 = buffer.Length;

                    await response.OutputStream.WriteAsync(buffer, cancellationToken);
                }

                response.Close();
            }
        }
    }
}