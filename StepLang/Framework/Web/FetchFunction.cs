using System.Text;
using System.Text.Json;
using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing;
using StepLang.Tokenizing;

namespace StepLang.Framework.Web;

/// <summary>
/// Fetches a resource from the web.
/// </summary>
public class FetchFunction : GenericFunction<StringResult, MapResult>
{
    /// <summary>
    /// The identifier of the <see cref="FetchFunction"/> function.
    /// </summary>
    public const string Identifier = "fetch";

    /// <inheritdoc />
    protected override IEnumerable<NativeParameter> NativeParameters { get; } = new NativeParameter[]
    {
        new(OnlyString, "url"),
        new(OnlyMap, "options", new MapExpressionNode(new(TokenType.OpeningCurlyBracket, "{"), new Dictionary<Token, ExpressionNode>())),
    };

    /// <inheritdoc />
    protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

    /// <inheritdoc />
    protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
        StringResult argument1, MapResult argument2)
    {
        var url = argument1.Value;
        var options = argument2.Value;

        using var response = Fetch(callLocation, url, options);
        if (!response.IsSuccessStatusCode)
            return NullResult.Instance;

        var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        return new StringResult(body);
    }

    /// <summary>
    /// Fetches a resource from the web.
    /// </summary>
    /// <param name="callLocation">The location of the call.</param>
    /// <param name="url">The URL to fetch.</param>
    /// <param name="options">The options for the fetch.</param>
    /// <returns>The response from the fetch.</returns>
    /// <exception cref="InvalidResultTypeException">Thrown when the <paramref name="options"/> contains an invalid type.</exception>
    private static HttpResponseMessage Fetch(TokenLocation callLocation, string url, Dictionary<string, ExpressionResult> options)
    {
        using var client = new HttpClient();

        var method = "GET";
        if (options.TryGetValue("method", out var methodResult) && methodResult is StringResult { Value: var methodValue })
            method = methodValue;

        using var request = new HttpRequestMessage(new(method), url);

        if (options.TryGetValue("headers", out var headersResult) && headersResult is MapResult { Value: var headers })
        {
            foreach (var (key, value) in headers)
            {
                if (value is StringResult { Value: var headerValue })
                    request.Headers.Add(key, headerValue);
                else
                    throw new InvalidResultTypeException(callLocation, value, ResultType.Str);
            }
        }

        if (options.TryGetValue("body", out var bodyResult))
        {
            var json = JsonSerializer.Serialize(bodyResult, JsonConversionContext.Default.ExpressionResult);

            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        if (options.TryGetValue("timeout", out var timeoutResult) && timeoutResult is NumberResult { Value: var timeout })
            client.Timeout = TimeSpan.FromMilliseconds(timeout);

        return client.Send(request);
    }
}