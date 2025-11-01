using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class ZippedFunction : GenericFunction<ListResult, ListResult, FunctionResult>
{
	public const string Identifier = "zipped";

	protected override IEnumerable<NativeParameter> NativeParameters =>
	[
		new(OnlyList, "left"),
		new(OnlyList, "right"),
		new(OnlyFunction, "zipper"),
	];

	protected override IEnumerable<ResultType> ReturnTypes => OnlyList;

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ListResult argument1,
		ListResult argument2,
		FunctionResult argument3
	)
	{
		return argument1.DeepClone().Value
			.Zip(
				argument2.DeepClone().Value,
				(a, b) => argument3.Value.Invoke(callLocation, interpreter, [a, b])
			)
			.ToListResult();
	}
}
