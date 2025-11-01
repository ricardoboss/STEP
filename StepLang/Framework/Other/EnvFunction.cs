using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Framework.Conversion;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Other;

public class EnvFunction : NativeFunction
{
	public const string Identifier = "env";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyString, "key"),
		new(NullableString, "newValue", LiteralExpressionNode.Null),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = NullableString;

	public override ExpressionResult Invoke(TokenLocation callLocation, IInterpreter interpreter,
		IReadOnlyList<ExpressionNode> arguments)
	{
		CheckArgumentCount(callLocation, arguments, 1, 2);

		var keyExpression = arguments[0].EvaluateUsing(interpreter);
		if (keyExpression is not StringResult { Value: var key })
		{
			throw new InvalidArgumentTypeException(callLocation, OnlyString, keyExpression);
		}

		if (arguments.Count == 1)
		{
			var value = Environment.GetEnvironmentVariable(key);

			return value is null ? NullResult.Instance : new StringResult(value);
		}

		var newValueExpression = arguments[1].EvaluateUsing(interpreter);
		var stringValue = ToStringFunction.Render(newValueExpression);

		Environment.SetEnvironmentVariable(key, stringValue);

		return VoidResult.Instance;
	}
}
