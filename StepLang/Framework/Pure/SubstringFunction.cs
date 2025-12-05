using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class SubstringFunction : GenericFunction<StringResult, NumberResult, ExpressionResult>
{
	public const string Identifier = "substring";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "subject"),
		new(OnlyNumber, "start"),
		new(NullableNumber, "length", LiteralExpressionNode.Null),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyString;

	protected override StringResult Invoke(TokenLocation callLocation, IInterpreter interpreter, StringResult argument1,
		NumberResult argument2, ExpressionResult argument3)
	{
		using var span = Telemetry.Profile();

		int? length;
		if (argument3 is NumberResult number)
		{
			length = number;
		}
		else if (argument3 is NullResult)
		{
			length = null;
		}
		else
		{
			throw new InvalidArgumentTypeException(callLocation, [ResultType.Number, ResultType.Null], argument3);
		}

		return argument1.Value.GraphemeSubstring(argument2, length);
	}
}
