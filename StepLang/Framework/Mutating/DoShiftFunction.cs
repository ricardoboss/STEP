using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

public class DoShiftFunction : GenericFunction<ListResult>
{
	public const string Identifier = "doShift";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyList, "subject"),
	];

	protected override IEnumerable<ResultType> ReturnTypes => AnyValueType;

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ListResult argument1)
	{
		using var span = Telemetry.Profile();

		var list = argument1.Value;
		if (list.Count == 0)
		{
			return NullResult.Instance;
		}

		var value = list[0];
		list.RemoveAt(0);

		return value;
	}
}
