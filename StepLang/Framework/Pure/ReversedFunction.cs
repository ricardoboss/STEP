using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class ReversedFunction : GenericFunction<ExpressionResult>
{
	public const string Identifier = "reversed";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new([ResultType.List, ResultType.Str], "subject"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = [ResultType.List, ResultType.Str];

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ExpressionResult argument1)
	{
		return argument1 switch
		{
			ListResult list => new ListResult(list.DeepClone().Value.Reverse().ToList()),
			StringResult str => new StringResult(str.Value.ReverseGraphemes()),
			_ => throw new InvalidResultTypeException(callLocation, argument1, ResultType.List, ResultType.Str),
		};
	}
}
