using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing;

namespace StepLang.Framework.Web;

public class HttpServerFunction : GenericFunction<ExpressionResult, FunctionResult>
{
    public const string Identifier = "httpServer";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter []
    {
        new(new[] { ResultType.Map, ResultType.Number }, "portOrOptions"),
        new(OnlyFunction, "handler"),
    };

    /// <inheritdoc />
    protected override ExpressionResult Invoke(Interpreter interpreter, ExpressionResult argument1, FunctionResult argument2)
    {
        int port;
        if (argument1 is MapResult { Value: var options })
        {
            if (options.TryGetValue("port", out var portResult))
            {
                if (portResult is not NumberResult portNumber)
                    throw new InvalidResultTypeException(portResult, ResultType.Number);

                if (portNumber < 0 || portNumber > 65535)
                    throw new InvalidArgumentValueException(null, "Port must be between 0 and 65535");

                port = portNumber;
            }
            else
                port = 8080;
        }
        else if (argument1 is NumberResult portNumber)
        {
            port = portNumber;
            options = new Dictionary<string, ExpressionResult>();
        }
        else
            throw new InvalidResultTypeException(argument1, ResultType.Number, ResultType.Map);

        using var cts = new CancellationTokenSource();

        var serveTask = Serve(interpreter, port, options, argument2.Value, cts.Token);

        interpreter.StdOut?.WriteLine("Press Ctrl+C to stop the server");

        // TODO: move logic for handling ctrl+c to interpreter
        Console.CancelKeyPress += Cancel;

        try
        {
            serveTask.Wait(cts.Token);
        }
        catch (OperationCanceledException)
        {
            // Ignore
        }

        Console.CancelKeyPress -= Cancel;

        return VoidResult.Instance;

        void Cancel(object? sender, ConsoleCancelEventArgs args)
        {
            interpreter.StdOut?.WriteLine("Stopping server...");

            args.Cancel = true;

            // ReSharper disable once AccessToDisposedClosure
            cts.Cancel();
        }
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
            if (headersResult is not MapResult { Value: var headers })
                throw new InvalidResultTypeException(headersResult, ResultType.Map);

            foreach (var (key, value) in headers)
                defaultResponseHeaders.Add(key, ToStringFunction.Render(value));
        }

        var handlers = Enumerable
            .Range(1, Environment.ProcessorCount)
            .Select(i => new RequestHandler(i, interpreter, defaultResponseHeaders, callback))
            .ToList();

        try
        {
            await Parallel.ForEachAsync(
                handlers,
                cancellationToken,
                async (handler, c) => await handler.Loop(server, c));
        }
        catch (HttpListenerException e) when (e.ErrorCode == 995)
        {
            // Ignore OPERATION_CANCELLED
        }
        finally
        {
            server.Stop();
        }
    }

    private sealed class RequestHandler
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

        public async Task Loop(HttpListener server, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && server.IsListening)
            {
                var context = await server.GetContextAsync();

                HandleContext(context);
            }
        }

        public void HandleContext(HttpListenerContext context)
        {
            var responseMap = HandleRequest(context);

            HandleResponse(context, responseMap);
        }

        private IDictionary<string, ExpressionResult> HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var requestMap = GetRequestMap(request);

            interpreter.StdOut?.WriteLineAsync($"({workerId}) {request.RemoteEndPoint} -> {request.HttpMethod} {request.Url?.AbsolutePath}");

            var responseResult = InvokeCallback(requestMap);

            return GetResponseMap(responseResult);
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
        private ExpressionResult InvokeCallback(MapResult requestMap)
        {
            try
            {
                return callback.Invoke(interpreter, new ExpressionNode[]
                {
                    requestMap,
                });
            }
            catch (Exception e)
            {
                return new MapResult(new Dictionary<string, ExpressionResult>
                {
                    {
                        "status", new NumberResult(500)
                    },
                    {
                        "body", new StringResult(RenderExceptionPage(e))
                    },
                });
            }
        }

        private void HandleResponse(HttpListenerContext context, IDictionary<string, ExpressionResult> responseMap)
        {
            var response = context.Response;

            response.StatusCode = responseMap.TryGetValue("status", out var statusResult) && statusResult is NumberResult statusNumber ? statusNumber : 200;

            interpreter.StdOut?.WriteLineAsync($"({workerId}) {context.Request.RemoteEndPoint} <- {response.StatusCode}");

            foreach (var (key, value) in defaultResponseHeaders)
                response.Headers.Add(key, value);

            if (responseMap.TryGetValue("headers", out var responseHeaders))
            {
                if (responseHeaders is not MapResult { Value: var headers })
                    throw new InvalidResultTypeException(responseHeaders, ResultType.Map);

                foreach (var (key, value) in headers)
                    response.Headers.Add(key, ToStringFunction.Render(value));
            }

            if (responseMap.TryGetValue("body", out var bodyResult))
            {
                var body = ToStringFunction.Render(bodyResult);
                var buffer = Encoding.UTF8.GetBytes(body);

                response.ContentLength64 = buffer.Length;

                response.OutputStream.Write(buffer);
            }

            response.Close();
        }

        private static IDictionary<string, ExpressionResult> GetResponseMap(ExpressionResult result)
        {
            if (result is MapResult map)
                return map.Value;

            return new Dictionary<string, ExpressionResult>
            {
                ["body"] = result,
            };
        }

        private static MapResult GetRequestMap(HttpListenerRequest request)
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
                requestBody = reader.ReadToEnd();

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
    }
}