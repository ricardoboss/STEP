using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;
using System.Diagnostics;
using System.Text.Json;

namespace StepLang.Framework.Conversion;

public class FromJsonFunction : GenericFunction<StringResult>
{
	public const string Identifier = "fromJson";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "source"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = AnyValueType;

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		StringResult argument1)
	{
		using var span = Telemetry.Profile();

		try
		{
			return JsonSerializer.Deserialize(argument1.Value, JsonConversionContext.Default.ExpressionResult) ??
				   NullResult.Instance;
		}
		catch (JsonException)
		{
			return NullResult.Instance;
		}
	}
}
