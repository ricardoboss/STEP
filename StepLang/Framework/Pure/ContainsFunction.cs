using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class ContainsFunction : GenericFunction<ExpressionResult, ExpressionResult>
{
	public const string Identifier = "contains";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new([ResultType.List, ResultType.Map, ResultType.Str], "subject"),
		new(AnyValueType, "value"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyBool;

	protected override BoolResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ExpressionResult argument1, ExpressionResult argument2)
	{
		var result = IndexOfFunction.GetResult(argument1, argument2);

		return result switch
		{
			NumberResult { Value: >= 0 } => true,
			StringResult => true,
			_ => false,
		};
	}
}
