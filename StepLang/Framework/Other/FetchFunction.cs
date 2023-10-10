using System.Text;
using System.Text.Json;
using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;

namespace StepLang.Framework.Other;

public class FetchFunction : NativeFunction
{
    public const string Identifier = "fetch";

    public override IEnumerable<(ResultType [] types, string identifier)> Parameters => new []
        { (new [] { ResultType.Str }, "url"), (new [] { ResultType.Map }, "options") };

    public override async Task<ExpressionResult> EvaluateAsync(Interpreter interpreter,
        IReadOnlyList<Expression> arguments, CancellationToken cancellationToken = default)
    {
        CheckArgumentCount(arguments, 1, 2);

        var evaluatedArgs =
            await arguments.EvaluateAsync(interpreter, cancellationToken).ToListAsync(cancellationToken);

        var url = evaluatedArgs[0].ExpectString().Value;
        var options = (evaluatedArgs.Count > 1 ? evaluatedArgs[1].ExpectMap() : MapResult.Empty).Value;

        using var response = await FetchAsync(url, options, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return NullResult.Instance;

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        return new StringResult(body);
    }

    private static async Task<HttpResponseMessage> FetchAsync(string url, IDictionary<string, ExpressionResult> options,
        CancellationToken cancellationToken)
    {
        using var client = new HttpClient();

        var method = options.TryGetValue("method", out var methodResult) ? methodResult.ExpectString().Value : "GET";

        using var request = new HttpRequestMessage(new(method), url);

        if (options.TryGetValue("headers", out var headersResult))
        {
            var headers = headersResult.ExpectMap().Value;
            foreach (var (key, value) in headers)
                request.Headers.Add(key, value.ExpectString().Value);
        }

        if (options.TryGetValue("body", out var bodyResult))
        {
            var json = JsonSerializer.Serialize(bodyResult, JsonConversionContext.Default.ExpressionResult);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        if (options.TryGetValue("timeout", out var timeoutResult))
        {
            var timeout = timeoutResult.ExpectNumber().Value;
            client.Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        return await client.SendAsync(request, cancellationToken);
    }
}