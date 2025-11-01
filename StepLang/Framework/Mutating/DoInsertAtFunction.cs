using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Tokenizing;

namespace StepLang.Framework.Mutating;

public class DoInsertAtFunction : GenericFunction<ListResult, NumberResult, ExpressionResult>
{
	public const string Identifier = "doInsertAt";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyList, "list"),
		new(OnlyNumber, "index"),
		new(AnyValueType, "value"),
	];

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		ListResult argument1, NumberResult argument2, ExpressionResult argument3)
	{
		var list = argument1.Value;
		var index = argument2;
		var value = argument3;

		if (index < 0 || index > list.Count)
		{
			throw new IndexOutOfBoundsException(callLocation, index, list.Count);
		}

		list.Insert(index, value);

		return VoidResult.Instance;
	}
}
