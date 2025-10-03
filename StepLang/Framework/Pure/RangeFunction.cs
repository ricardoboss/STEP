using StepLang.Expressions;
using StepLang.Expressions.Results;
using StepLang.Interpreting;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Framework.Pure;

public class RangeFunction : GenericFunction<NumberResult, NumberResult, NumberResult>
{
	public const string Identifier = "range";

	protected override IEnumerable<NativeParameter> NativeParameters { get; } =
	[
		new(OnlyNumber, "start"),
		new(OnlyNumber, "end"),
		new(OnlyNumber, "step", LiteralExpressionNode.FromInt32(1)),
	];

	protected override IEnumerable<ResultType> ReturnTypes { get; } = OnlyList;

	protected override ExpressionResult Invoke(TokenLocation callLocation, Interpreter interpreter,
		NumberResult argument1, NumberResult argument2, NumberResult argument3)
	{
		var startValue = Convert.ToDecimal(argument1.Value);
		var endValue = Convert.ToDecimal(argument2.Value);
		var stepValue = Convert.ToDecimal(argument3.Value);

		if (stepValue == 0)
		{
			throw new InvalidArgumentValueException(callLocation, "step must not be 0");
		}

		if (stepValue > 0 && startValue > endValue)
		{
			throw new InvalidArgumentValueException(callLocation,
				"Positive step requires start to be less than or equal to end");
		}

		if (stepValue < 0 && startValue < endValue)
		{
			throw new InvalidArgumentValueException(callLocation,
				"Negative step requires start to be greater than or equal to end");
		}

		var values = new List<ExpressionResult>();

		var maxSteps = (int)Math.Truncate(Math.Abs(endValue - startValue) / Math.Abs(stepValue));
		for (var currentStep = 0; currentStep <= maxSteps; currentStep++)
		{
			var currentValue = startValue + stepValue * currentStep;

			values.Add(Convert.ToDouble(currentValue).ToNumberResult());
		}

		return new ListResult(values);
	}
}
