using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Conversion;

public class ToNumberFunction : GenericFunction<StringResult, NumberResult>
{
	public const string Identifier = "toNumber";

	protected override NativeParameter[] NativeParameters { get; } =
	[
		new(OnlyString, "value"),
		new(OnlyNumber, "radix", LiteralExpressionNode.FromInt32(10)),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableNumber;

	protected override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		StringResult argument1, NumberResult argument2)
	{
		var value = argument1.Value;
		var radix = argument2;

		try
		{
			return radix.Value switch
			{
				2 or 8 or 16 => NumberResult.FromInt32(Convert.ToInt32(value, radix)),
				10 => NumberResult.FromString(value),
				_ => NullResult.Instance,
			};
		}
		catch (Exception e) when (e is ArgumentException or FormatException)
		{
			return NullResult.Instance;
		}
	}
}
