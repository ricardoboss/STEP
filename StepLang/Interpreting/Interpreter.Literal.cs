using StepLang.Expressions.Results;
using StepLang.Parsing.Nodes.Expressions;
using StepLang.Tokenizing;

namespace StepLang.Interpreting;

public partial class Interpreter
{
	public ExpressionResult Evaluate(LiteralExpressionNode expressionNode)
	{
		using var span = Telemetry.Profile(nameof(LiteralExpressionNode));

		var literal = expressionNode.Literal;

		return literal.Type switch
		{
			TokenType.LiteralBoolean => (BoolResult)literal.Value,
			TokenType.LiteralNumber => (NumberResult)literal.Value,
			TokenType.LiteralString => (StringResult)literal.StringValue,
			TokenType.LiteralNull => NullResult.Instance,
			_ => throw new NotSupportedException("Unimplemented literal type " + literal),
		};
	}

	public ExpressionResult Evaluate(ListExpressionNode expressionNode)
	{
		using var span = Telemetry.Profile(nameof(ListExpressionNode));

		var results = expressionNode
			.Expressions
			.Select(expression => expression.EvaluateUsing(this))
			.ToList();

		return new ListResult(results);
	}

	public ExpressionResult Evaluate(MapExpressionNode expressionNode)
	{
		using var span = Telemetry.Profile(nameof(MapExpressionNode));

		var results = expressionNode
			.Expressions
			.ToDictionary(
				kvp => kvp.Key.StringValue,
				kvp => kvp.Value.EvaluateUsing(this)
			);

		return new MapResult(results);
	}
}
