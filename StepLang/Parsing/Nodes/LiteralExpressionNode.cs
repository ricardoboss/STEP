using StepLang.Expressions.Results;
using StepLang.Tokenizing;
using System.Globalization;

namespace StepLang.Parsing.Nodes;

public sealed record LiteralExpressionNode(Token Literal) : ExpressionNode
{
	public override ExpressionResult EvaluateUsing(IExpressionEvaluator evaluator)
	{
		return evaluator.Evaluate(this);
	}

	public override TokenLocation Location => Literal.Location;

	public static implicit operator LiteralExpressionNode(string literal)
	{
		return new LiteralExpressionNode(new Token(TokenType.LiteralString, $"\"{literal}\""));
	}

	public static implicit operator LiteralExpressionNode(double literal)
	{
		return new LiteralExpressionNode(new Token(TokenType.LiteralNumber,
			literal.ToString(CultureInfo.InvariantCulture)));
	}

	public static implicit operator LiteralExpressionNode(int literal)
	{
		return new LiteralExpressionNode(new Token(TokenType.LiteralNumber,
			literal.ToString(CultureInfo.InvariantCulture)));
	}

	public static implicit operator LiteralExpressionNode(bool literal)
	{
		return new LiteralExpressionNode(new Token(TokenType.LiteralBoolean, literal ? "true" : "false"));
	}

	public static LiteralExpressionNode Null => new(new Token(TokenType.LiteralNull, "null"));

	public static LiteralExpressionNode FromString(string value)
	{
		return value;
	}

	public static LiteralExpressionNode FromDouble(double value)
	{
		return value;
	}

	public static LiteralExpressionNode FromInt32(int value)
	{
		return value;
	}

	public static LiteralExpressionNode FromBoolean(bool value)
	{
		return value;
	}
}
