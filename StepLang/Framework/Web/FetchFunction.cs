using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;
using System.Text;
using System.Text.Json;

namespace StepLang.Framework.Web;

public class FetchFunction : GenericFunction<StringResult, MapResult>
{
	public const string Identifier = "fetch";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "url"),
		new(OnlyMap, "options",
			new MapExpressionNode(new Token(TokenType.OpeningCurlyBracket, "{"),
				new Dictionary<Token, ExpressionNode>())),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

	protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
		StringResult argument1, MapResult argument2)
	{
		var url = argument1.Value;
		var options = argument2.Value;

		using var response = Fetch(callLocation, url, options);
		if (!response.IsSuccessStatusCode)
		{
			return NullResult.Instance;
		}

		var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

		return new StringResult(body);
	}

	private static HttpResponseMessage Fetch(TokenLocation callLocation, string url,
		Dictionary<string, ExpressionResult> options)
	{
		using var client = new HttpClient();

		var method = "GET";
		if (options.TryGetValue("method", out var methodResult) &&
			methodResult is StringResult { Value: var methodValue })
		{
			method = methodValue;
		}

		using var request = new HttpRequestMessage(new HttpMethod(method), url);

		if (options.TryGetValue("headers", out var headersResult) && headersResult is MapResult { Value: var headers })
		{
			foreach (var (key, value) in headers)
			{
				if (value is StringResult { Value: var headerValue })
				{
					request.Headers.Add(key, headerValue);
				}
				else
				{
					throw new InvalidResultTypeException(callLocation, value, ResultType.Str);
				}
			}
		}

		if (options.TryGetValue("body", out var bodyResult))
		{
			var json = JsonSerializer.Serialize(bodyResult, JsonConversionContext.Default.ExpressionResult);

			request.Content = new StringContent(json, Encoding.UTF8, "application/json");
		}

		if (options.TryGetValue("timeout", out var timeoutResult) &&
			timeoutResult is NumberResult { Value: var timeout })
		{
			client.Timeout = TimeSpan.FromMilliseconds(timeout);
		}

		return client.Send(request);
	}
}
