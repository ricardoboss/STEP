using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

public class DoPopFunction : GenericFunction<ListResult>
{
	public const string Identifier = "doPop";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyList, "subject"),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = AnyValueType;

	protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
		ListResult argument1)
	{
		var list = argument1.Value;

		if (list.Count == 0)
		{
			return NullResult.Instance;
		}

		var value = list.Last();

		list.RemoveAt(list.Count - 1);

		return value;
	}
}
