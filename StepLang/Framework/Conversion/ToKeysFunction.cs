using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

public class ToKeysFunction : GenericFunction<MapResult>
{
	public const string Identifier = "toKeys";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyMap, "source"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyList;

	protected override ListResult Invoke(TokenLocation callLocation, IInterpreter interpreter, MapResult argument1)
	{
		using var span = Telemetry.Profile();

		var keys = argument1.Value.Keys
			.Select(k => new StringResult(k))
			.Cast<ExpressionResult>()
			.ToList();

		return new ListResult(keys);
	}
}
