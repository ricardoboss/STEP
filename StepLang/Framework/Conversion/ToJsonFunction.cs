using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;
using System.Text.Json;

namespace StepLang.Framework.Conversion;

public class ToJsonFunction : GenericFunction<ExpressionResult>
{
	public const string Identifier = "toJson";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(AnyValueType, "value"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

	protected override StringResult Invoke(TokenLocation callLocation, Interpreter interpreter,
		ExpressionResult argument1)
	{
		return JsonSerializer.Serialize(argument1, JsonConversionContext.Default.ExpressionResult);
	}
}
